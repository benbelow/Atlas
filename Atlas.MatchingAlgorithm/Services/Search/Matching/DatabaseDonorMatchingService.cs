﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Atlas.Common.ApplicationInsights;
using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.MatchingAlgorithm.Client.Models;
using Atlas.MatchingAlgorithm.Client.Models.Donors;
using Atlas.MatchingAlgorithm.Common.Config;
using Atlas.MatchingAlgorithm.Common.Models;
using Atlas.MatchingAlgorithm.Common.Models.Matching;
using Atlas.MatchingAlgorithm.Common.Models.SearchResults;
using Atlas.MatchingAlgorithm.Common.Repositories;
using Atlas.MatchingAlgorithm.Data.Models;
using Atlas.MatchingAlgorithm.Data.Models.SearchResults;
using Atlas.MatchingAlgorithm.Data.Repositories.DonorRetrieval;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders.TransientSqlDatabase.RepositoryFactories;

namespace Atlas.MatchingAlgorithm.Services.Search.Matching
{
    public interface IDatabaseDonorMatchingService
    {
        /// <summary>
        /// Searches the pre-processed matching data for matches at the specified loci.
        /// Performs filtering against loci and total mismatch counts.
        /// </summary>
        /// <returns>
        /// A dictionary of PotentialSearchResults, keyed by donor id.
        /// MatchDetails will be populated only for the specified loci.
        /// </returns>
        Task<IDictionary<int, MatchResult>> FindMatchesForLoci(AlleleLevelMatchCriteria criteria, ICollection<Locus> loci);

        Task<IDictionary<int, MatchResult>> FindMatchesForLociFromDonorSelection(
            AlleleLevelMatchCriteria criteria,
            ICollection<Locus> loci,
            IDictionary<int, MatchResult> phase1MatchResults
        );
    }

    public class DatabaseDonorMatchingService : IDatabaseDonorMatchingService
    {
        private readonly IDonorSearchRepository donorSearchRepository;
        private readonly IMatchFilteringService matchFilteringService;
        private readonly IDatabaseFilteringAnalyser databaseFilteringAnalyser;
        private readonly ILogger logger;
        private readonly IPGroupRepository pGroupRepository;

        public DatabaseDonorMatchingService(
            IActiveRepositoryFactory repositoryFactory,
            IMatchFilteringService matchFilteringService,
            IDatabaseFilteringAnalyser databaseFilteringAnalyser,
            ILogger logger
        )
        {
            donorSearchRepository = repositoryFactory.GetDonorSearchRepository();
            pGroupRepository = repositoryFactory.GetPGroupRepository();
            this.matchFilteringService = matchFilteringService;
            this.databaseFilteringAnalyser = databaseFilteringAnalyser;
            this.logger = logger;
        }

        public async Task<IDictionary<int, MatchResult>> FindMatchesForLoci(AlleleLevelMatchCriteria criteria, ICollection<Locus> loci)
        {
            if (loci.Any(locus => !LocusSettings.LociPossibleToMatchInMatchingPhaseOne.Contains(locus)))
            {
                // Currently the logic here is not advised for these loci
                // Donors can be untyped at these loci, which counts as a potential match
                // so a simple search of the database would return a huge number of donors. 
                // To avoid serialising that many results, we filter on these loci based on the results at other loci
                throw new NotImplementedException();
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var results = await Task.WhenAll(loci.Select(l =>
                FindMatchesAtLocus(criteria.SearchType, l, criteria.MatchCriteriaForLocus(l)))
            );

            logger.SendTrace($"MATCHING PHASE1: all donors from Db. {results.Sum(x => x.Count)} results in {stopwatch.ElapsedMilliseconds} ms",
                LogLevel.Info);
            stopwatch.Restart();

            var matches = results
                .SelectMany(r => r)
                .GroupBy(m => m.Key)
                // If no mismatches are allowed - donors must be matched at all provided loci. This check performed upfront to improve performance of such searches 
                .Where(g => criteria.DonorMismatchCount != 0 || g.Count() == loci.Count)
                .Select(matchesForDonor =>
                {
                    var donorId = matchesForDonor.Key;
                    var result = new MatchResult
                    {
                        DonorId = donorId,
                    };
                    foreach (var locus in loci)
                    {
                        var (key, donorAndMatchForLocus) = matchesForDonor.FirstOrDefault(m => m.Value.Locus == locus);
                        var locusMatchDetails = donorAndMatchForLocus != null
                            ? donorAndMatchForLocus.Match
                            : new LocusMatchDetails {MatchCount = 0};
                        result.SetMatchDetailsForLocus(locus, locusMatchDetails);
                    }

                    return result;
                })
                .Where(m => loci.All(l => matchFilteringService.FulfilsPerLocusMatchCriteria(m, criteria, l)))
                .Where(m => matchFilteringService.FulfilsTotalMatchCriteria(m, criteria))
                .ToList();

            logger.SendTrace($"MATCHING PHASE1: Manipulate + filter: {stopwatch.ElapsedMilliseconds}", LogLevel.Info);
            stopwatch.Restart();

            return matches.ToDictionary(m => m.DonorId, m => m);
        }

        public async Task<IDictionary<int, MatchResult>> FindMatchesForLociFromDonorSelection(
            AlleleLevelMatchCriteria criteria,
            ICollection<Locus> loci,
            IDictionary<int, MatchResult> phase1MatchResults
        )
        {
            foreach (var locus in loci)
            {
                var results = await FindMatchesAtLocusFromDonorSelection(
                    criteria.SearchType,
                    locus,
                    criteria.MatchCriteriaForLocus(locus),
                    phase1MatchResults.Keys
                );

                foreach (var (donorId, phase2Match) in results)
                {
                    var locusMatchDetails = phase2Match != null
                        ? phase2Match.Match
                        : new LocusMatchDetails {MatchCount = 0};

                    var phase1Match = phase1MatchResults[donorId];
                    phase1Match?.SetMatchDetailsForLocus(locus, locusMatchDetails);
                }

                var mismatchDonorIds = phase1MatchResults.Keys.Except(results.Select(r => r.Key));
                foreach (var mismatchDonorId in mismatchDonorIds)
                {
                    phase1MatchResults[mismatchDonorId].SetMatchDetailsForLocus(locus, new LocusMatchDetails {MatchCount = 0});
                }
            }

            return phase1MatchResults
                .Where(m => matchFilteringService.FulfilsTotalMatchCriteria(m.Value, criteria))
                .Where(m => loci.All(l => matchFilteringService.FulfilsPerLocusMatchCriteria(m.Value, criteria, l)))
                .ToDictionary(m => m.Key, m => m.Value);
        }

        private async Task<IDictionary<int, DonorAndMatchForLocus>> FindMatchesAtLocus(
            DonorType searchType,
            Locus locus,
            AlleleLevelLocusMatchCriteria criteria
        )
        {
            var repoCriteria = new LocusSearchCriteria
            {
                SearchType = searchType,
                PGroupIdsToMatchInPositionOne = await pGroupRepository.GetPGroupIds(criteria.PGroupsToMatchInPositionOne),
                PGroupIdsToMatchInPositionTwo = await pGroupRepository.GetPGroupIds(criteria.PGroupsToMatchInPositionTwo),
                MismatchCount = criteria.MismatchCount,
            };

            var filteringOptions = new MatchingFilteringOptions
            {
                ShouldFilterOnDonorType = databaseFilteringAnalyser.ShouldFilterOnDonorTypeInDatabase(repoCriteria),
            };

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var matchesAtLocus = await donorSearchRepository.GetDonorMatchesAtLocus(locus, repoCriteria, filteringOptions);
            logger.SendTrace($"MATCHING PHASE1: SQL Requests, {stopwatch.ElapsedMilliseconds}", LogLevel.Info);
            stopwatch.Restart();
            
            var matches = matchesAtLocus
                .GroupBy(m => m.DonorId)
                .ToDictionary(g => g.Key, g => DonorAndMatchFromGroup(g, locus));

            logger.SendTrace($"MATCHING PHASE1: Direct/Cross analysis, {stopwatch.ElapsedMilliseconds}", LogLevel.Info);
            
            return matches;
        }

        private async Task<IDictionary<int, DonorAndMatchForLocus>> FindMatchesAtLocusFromDonorSelection(
            DonorType searchType,
            Locus locus,
            AlleleLevelLocusMatchCriteria criteria,
            IEnumerable<int> donorIds
        )
        {
            var repoCriteria = new LocusSearchCriteria
            {
                SearchType = searchType,
                PGroupIdsToMatchInPositionOne = await pGroupRepository.GetPGroupIds(criteria.PGroupsToMatchInPositionOne),
                PGroupIdsToMatchInPositionTwo = await pGroupRepository.GetPGroupIds(criteria.PGroupsToMatchInPositionTwo),
                MismatchCount = criteria.MismatchCount,
            };

            var matches = (await donorSearchRepository.GetDonorMatchesAtLocusFromDonorSelection(locus, repoCriteria, donorIds))
                .GroupBy(m => m.DonorId)
                .ToDictionary(g => g.Key, g => DonorAndMatchFromGroup(g, locus));

            return matches;
        }

        private static DonorAndMatchForLocus DonorAndMatchFromGroup(IGrouping<int, PotentialHlaMatchRelation> group, Locus locus)
        {
            var donorId = group.Key;
            var potentialHlaMatchRelations = group.ToList();
            return new DonorAndMatchForLocus
            {
                DonorId = donorId,
                Match = new LocusMatchDetails
                {
                    MatchCount = DirectMatch(potentialHlaMatchRelations) || CrossMatch(potentialHlaMatchRelations) ? 2 : 1
                },
                Locus = locus
            };
        }

        private static bool DirectMatch(IList<PotentialHlaMatchRelation> matches)
        {
            return matches.Any(m => m.SearchTypePosition == LocusPosition.One && m.MatchingTypePosition == LocusPosition.One)
                   && matches.Any(m => m.SearchTypePosition == LocusPosition.Two && m.MatchingTypePosition == LocusPosition.Two);
        }

        private static bool CrossMatch(IList<PotentialHlaMatchRelation> matches)
        {
            return matches.Any(m => m.SearchTypePosition == LocusPosition.One && m.MatchingTypePosition == LocusPosition.Two)
                   && matches.Any(m => m.SearchTypePosition == LocusPosition.Two && m.MatchingTypePosition == LocusPosition.One);
        }

        private class DonorAndMatchForLocus
        {
            public LocusMatchDetails Match { get; set; }
            public int DonorId { get; set; }
            public Locus Locus { get; set; }
        }
    }
}