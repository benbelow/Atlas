﻿using Nova.SearchAlgorithm.Client.Models.SearchRequests;
using Nova.SearchAlgorithm.Client.Models.SearchResults;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Common.Models.SearchResults;
using Nova.SearchAlgorithm.MatchingDictionary.Services;
using Nova.SearchAlgorithm.MatchingDictionaryConversions;
using Nova.SearchAlgorithm.Services.Matching;
using Nova.SearchAlgorithm.Services.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SearchResult = Nova.SearchAlgorithm.Client.Models.SearchResults.SearchResult;

namespace Nova.SearchAlgorithm.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResult>> Search(SearchRequest searchRequest);
    }

    public class SearchService : ISearchService
    {
        private readonly ILocusHlaMatchingLookupService locusHlaMatchingLookupService;
        private readonly IDonorScoringService donorScoringService;
        private readonly IDonorMatchingService donorMatchingService;

        public SearchService(
            ILocusHlaMatchingLookupService locusHlaMatchingLookupService, 
            IDonorScoringService donorScoringService,
            IDonorMatchingService donorMatchingService
            )
        {
            this.locusHlaMatchingLookupService = locusHlaMatchingLookupService;
            this.donorScoringService = donorScoringService;
            this.donorMatchingService = donorMatchingService;
        }

        public async Task<IEnumerable<SearchResult>> Search(SearchRequest searchRequest)
        {
            var criteria = await GetMatchCriteria(searchRequest);
            var patientHla = GetPatientHla(searchRequest);

            var matches = await donorMatchingService.GetMatches(criteria);
            var scoredMatches = await donorScoringService.ScoreMatchesAgainstHla(matches, patientHla);
            
            return scoredMatches.Select(MapSearchResultToApiSearchResult);
        }

        private async Task<AlleleLevelMatchCriteria> GetMatchCriteria(SearchRequest searchRequest)
        {
            var matchCriteria = searchRequest.MatchCriteria;
            var criteriaMappings = await Task.WhenAll(
                MapLocusInformationToMatchCriteria(Locus.A, matchCriteria.LocusMismatchA, searchRequest.SearchHlaData.LocusSearchHlaA),
                MapLocusInformationToMatchCriteria(Locus.B, matchCriteria.LocusMismatchB, searchRequest.SearchHlaData.LocusSearchHlaB),
                MapLocusInformationToMatchCriteria(Locus.C, matchCriteria.LocusMismatchC, searchRequest.SearchHlaData.LocusSearchHlaC),
                MapLocusInformationToMatchCriteria(Locus.Drb1, matchCriteria.LocusMismatchDrb1, searchRequest.SearchHlaData.LocusSearchHlaDrb1),
                MapLocusInformationToMatchCriteria(Locus.Dqb1, matchCriteria.LocusMismatchDqb1, searchRequest.SearchHlaData.LocusSearchHlaDqb1));

            var criteria = new AlleleLevelMatchCriteria
            {
                SearchType = searchRequest.SearchType,
                RegistriesToSearch = searchRequest.RegistriesToSearch,
                DonorMismatchCount = (int) matchCriteria.DonorMismatchCount,
                LocusMismatchA = criteriaMappings[0],
                LocusMismatchB = criteriaMappings[1],
                LocusMismatchC = criteriaMappings[2],
                LocusMismatchDRB1 = criteriaMappings[3],
                LocusMismatchDQB1 = criteriaMappings[4]
            };
            return criteria;
        }

        private async Task<AlleleLevelLocusMatchCriteria> MapLocusInformationToMatchCriteria(Locus locus, LocusMismatchCriteria mismatchCriteria, LocusSearchHla searchHla)
        {
            if (mismatchCriteria == null)
            {
                return null;
            }

            var lookupResult = await locusHlaMatchingLookupService.GetHlaMatchingLookupResultForLocus(
                locus.ToMatchLocus(),
                new Tuple<string, string>(searchHla.SearchHla1, searchHla.SearchHla2));

            return new AlleleLevelLocusMatchCriteria
            {
                MismatchCount = mismatchCriteria.MismatchCount,
                PGroupsToMatchInPositionOne = lookupResult.Item1.MatchingPGroups,
                PGroupsToMatchInPositionTwo = lookupResult.Item2.MatchingPGroups
            };
        }

        private static PhenotypeInfo<string> GetPatientHla(SearchRequest searchRequest)
        {
            var hlaData = searchRequest.SearchHlaData;
            return new PhenotypeInfo<string>
            {
                A_1 = hlaData.LocusSearchHlaA.SearchHla1,
                A_2 = hlaData.LocusSearchHlaA.SearchHla2,
                B_1 = hlaData.LocusSearchHlaB.SearchHla1,
                B_2 = hlaData.LocusSearchHlaB.SearchHla2,
                C_1 = hlaData.LocusSearchHlaC?.SearchHla1,
                C_2 = hlaData.LocusSearchHlaC?.SearchHla2,
                DQB1_1 = hlaData.LocusSearchHlaDqb1?.SearchHla1,
                DQB1_2 = hlaData.LocusSearchHlaDqb1?.SearchHla2,
                DRB1_1 = hlaData.LocusSearchHlaDrb1.SearchHla1,
                DRB1_2 = hlaData.LocusSearchHlaDrb1.SearchHla2
            };
        }

        private static SearchResult MapSearchResultToApiSearchResult(MatchAndScoreResult result)
        {
            return new SearchResult
            {
                DonorId = result.MatchResult.Donor.DonorId,
                DonorType = result.MatchResult.Donor.DonorType,
                Registry = result.MatchResult.Donor.RegistryCode,
                OverallMatchConfidence = result.ScoreResult.OverallMatchConfidence,
                ConfidenceScore = result.ScoreResult.ConfidenceScore,
                GradeScore = result.ScoreResult.GradeScore,
                TotalMatchCount = result.MatchResult.TotalMatchCount,
                PotentialMatchCount = result.PotentialMatchCount,
                TypedLociCount = result.ScoreResult.TypedLociCount,
                SearchResultAtLocusA = MapSearchResultToApiLocusSearchResult(result, Locus.A),
                SearchResultAtLocusB = MapSearchResultToApiLocusSearchResult(result, Locus.B),
                SearchResultAtLocusC = MapSearchResultToApiLocusSearchResult(result, Locus.C),
                SearchResultAtLocusDqb1 = MapSearchResultToApiLocusSearchResult(result, Locus.Dqb1),
                SearchResultAtLocusDrb1 = MapSearchResultToApiLocusSearchResult(result, Locus.Drb1),
            };
        }

        private static LocusSearchResult MapSearchResultToApiLocusSearchResult(MatchAndScoreResult result, Locus locus)
        {
            var matchDetailsForLocus = result.MatchResult.MatchDetailsForLocus(locus);
            var scoreDetailsForLocus = result.ScoreResult.ScoreDetailsForLocus(locus);

            return new LocusSearchResult
            {
                IsLocusTyped = scoreDetailsForLocus.IsLocusTyped,
                MatchCount = matchDetailsForLocus?.MatchCount,
                MatchGradeScore = scoreDetailsForLocus.MatchGradeScore,
                MatchConfidenceScore = scoreDetailsForLocus.MatchConfidenceScore,
                ScoreDetailsAtPositionOne = new LocusPositionScoreDetails
                {
                    MatchConfidence = scoreDetailsForLocus.ScoreDetailsAtPosition1.MatchConfidence,
                    MatchConfidenceScore = scoreDetailsForLocus.ScoreDetailsAtPosition1.MatchConfidenceScore,
                    MatchGrade = scoreDetailsForLocus.ScoreDetailsAtPosition1.MatchGrade,
                    MatchGradeScore = scoreDetailsForLocus.ScoreDetailsAtPosition1.MatchGradeScore,
                },
                ScoreDetailsAtPositionTwo = new LocusPositionScoreDetails
                {
                    MatchConfidence = scoreDetailsForLocus.ScoreDetailsAtPosition2.MatchConfidence,
                    MatchConfidenceScore = scoreDetailsForLocus.ScoreDetailsAtPosition2.MatchConfidenceScore,
                    MatchGrade = scoreDetailsForLocus.ScoreDetailsAtPosition2.MatchGrade,
                    MatchGradeScore = scoreDetailsForLocus.ScoreDetailsAtPosition2.MatchGradeScore,
                }
            };
        }
    }
}