﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlas.Client.Models.Search.Requests;
using Atlas.Client.Models.Search.Results.Matching.PerLocus;
using Atlas.Common.ApplicationInsights;
using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.Common.GeneticData.PhenotypeInfo.TransferModels;
using Atlas.Common.Utils.Extensions;
using Atlas.HlaMetadataDictionary.ExternalInterface;
using Atlas.HlaMetadataDictionary.ExternalInterface.Models.Metadata.ScoringMetadata;
using Atlas.MatchingAlgorithm.Client.Models.Scoring;
using Atlas.MatchingAlgorithm.Common.Models.Scoring;
using Atlas.MatchingAlgorithm.Common.Models.SearchResults;
using Atlas.MatchingAlgorithm.Data.Models.SearchResults;
using Atlas.MatchingAlgorithm.Models;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders;
using Atlas.MatchingAlgorithm.Services.Search.Scoring.Aggregation;
using Atlas.MatchingAlgorithm.Services.Search.Scoring.Confidence;
using Atlas.MatchingAlgorithm.Services.Search.Scoring.Grading;
using Atlas.MatchingAlgorithm.Services.Search.Scoring.Ranking;

namespace Atlas.MatchingAlgorithm.Services.Search.Scoring
{
    public interface IDonorScoringService
    {
        Task<ScoreResult> ScoreDonorHlaAgainstPatientHla(DonorHlaScoringRequest request);

        Task<Dictionary<PhenotypeInfo<string>, ScoreResult>> ScoreDonorsHlaAgainstPatientHla(
            List<PhenotypeInfo<string>> distinctDonorPhenotypes,
            PhenotypeInfo<string> patientPhenotypeInfo,
            ScoringCriteria scoringCriteria);
    }

    public class DonorScoringService : IDonorScoringService
    {
        private readonly IHlaMetadataDictionary hlaMetadataDictionary;
        private readonly IGradingService gradingService;
        private readonly IConfidenceService confidenceService;
        private readonly IMatchScoreCalculator matchScoreCalculator;
        private readonly IScoreResultAggregator scoreResultAggregator;
        private readonly IDpb1TceGroupMatchCalculator dpb1TceGroupMatchCalculator;
        private readonly ILogger logger;

        public DonorScoringService(
            IHlaMetadataDictionaryFactory factory,
            IActiveHlaNomenclatureVersionAccessor hlaNomenclatureVersionAccessor,
            IGradingService gradingService,
            IConfidenceService confidenceService,
            IMatchScoreCalculator matchScoreCalculator,
            IScoreResultAggregator scoreResultAggregator,
            IDpb1TceGroupMatchCalculator dpb1TceGroupMatchCalculator,
            ILogger logger)
        {
            hlaMetadataDictionary = factory.BuildDictionary(hlaNomenclatureVersionAccessor.GetActiveHlaNomenclatureVersion());
            this.gradingService = gradingService;
            this.confidenceService = confidenceService;
            this.matchScoreCalculator = matchScoreCalculator;
            this.scoreResultAggregator = scoreResultAggregator;
            this.dpb1TceGroupMatchCalculator = dpb1TceGroupMatchCalculator;
            this.logger = logger;
        }

        public async Task<ScoreResult> ScoreDonorHlaAgainstPatientHla(DonorHlaScoringRequest request)
        {
            if (request.ScoringCriteria.LociToScore.IsNullOrEmpty())
            {
                return default;
            }

            var patientScoringMetadata = await GetHlaScoringMetadata(request.PatientHla.ToPhenotypeInfo(), request.ScoringCriteria.LociToScore);
            return await ScoreDonorHlaAgainstPatientMetadata(request.DonorHla.ToPhenotypeInfo(), request.ScoringCriteria, patientScoringMetadata);
        }

        public async Task<Dictionary<PhenotypeInfo<string>, ScoreResult>> ScoreDonorsHlaAgainstPatientHla(
            List<PhenotypeInfo<string>> donorsHla,
            PhenotypeInfo<string> patientHla,
            ScoringCriteria scoringCriteria)
        {
            var patientMetadata = await GetHlaScoringMetadata(patientHla, scoringCriteria.LociToScore);
            logger.SendTrace($"Received patient scoring HLA result", LogLevel.Info);

            var scoringResultsPerDonorsHla = new Dictionary<PhenotypeInfo<string>, ScoreResult>();

            // we are deliberately avoiding running scoring for multiple donors in parallel for now to minimize load on scoring system.
            // in case we find performance issues with this approach, it'll be changed later.
            for (var i = 0; i < donorsHla.Count; i++)
            {
                var donorHla = donorsHla[i];
                ScoreResult scoringResult = null;
                try
                {
                    scoringResult = await ScoreDonorHlaAgainstPatientMetadata(donorHla, scoringCriteria, patientMetadata);
                    logger.SendTrace($"Received scoring result for donor {i + 1} / {donorsHla.Count}", LogLevel.Verbose);
                }
                catch (Exception ex)
                {
                    logger.SendTrace($"Could not get score result for one of the donors. Exception: {ex}", LogLevel.Error);
                }

                scoringResultsPerDonorsHla[donorHla] = scoringResult;
            }

            logger.SendTrace($"Received scoring results for {donorsHla.Count} donors", LogLevel.Info);

            return scoringResultsPerDonorsHla;
        }

        protected async Task<ScoreResult> ScoreDonorHlaAgainstPatientMetadata(
            PhenotypeInfo<string> donorHla,
            ScoringCriteria scoringCriteria,
            PhenotypeInfo<IHlaScoringMetadata> patientScoringMetadata)
        {
            var donorScoringMetadata = await GetHlaScoringMetadata(donorHla, scoringCriteria.LociToScore);

            var grades = gradingService.CalculateGrades(patientScoringMetadata, donorScoringMetadata);
            var confidences = confidenceService.CalculateMatchConfidences(patientScoringMetadata, donorScoringMetadata, grades);

            var dpb1TceGroupMatchType = await dpb1TceGroupMatchCalculator.CalculateDpb1TceGroupMatchType(
                patientScoringMetadata.Dpb1.Map(x => x?.LookupName),
                donorHla.Dpb1
            );

            var donorScoringInfo = new DonorScoringInfo
            {
                Grades = grades,
                Confidences = confidences,
                DonorHla = donorHla
            };

            return BuildScoreResult(scoringCriteria, donorScoringInfo, dpb1TceGroupMatchType);
        }

        protected async Task<PhenotypeInfo<IHlaScoringMetadata>> GetHlaScoringMetadata(PhenotypeInfo<string> hlaNames, IEnumerable<Locus> lociToScore)
        {
            return await hlaNames.MapAsync(
                async (locus, _, hla) =>
                {
                    // do not perform lookup for an untyped locus or a locus that is not to be scored
                    if (hla.IsNullOrEmpty() || !lociToScore.Contains(locus))
                    {
                        return default;
                    }

                    return await hlaMetadataDictionary.GetHlaScoringMetadata(locus, hla);
                });
        }

        private ScoreResult BuildScoreResult(ScoringCriteria criteria, DonorScoringInfo donorScoringInfo, Dpb1TceGroupMatchType dpb1TceGroupMatchType)
        {
            var scoreResult = new ScoreResult();

            foreach (var locus in criteria.LociToScore)
            {
                var gradeResults = donorScoringInfo.Grades.GetLocus(locus).Map(x => x.GradeResult);
                var confidences = donorScoringInfo.Confidences.GetLocus(locus);

                var scoreDetailsPerPosition = new LocusInfo<LocusPositionScoreDetails>(p =>
                    BuildScoreDetailsForPosition(gradeResults.GetAtPosition(p), confidences.GetAtPosition(p))
                );

                var matchCategory = locus == Locus.Dpb1
                    ? LocusMatchCategoryAggregator.Dpb1MatchCategoryFromPositionScores(scoreDetailsPerPosition, dpb1TceGroupMatchType)
                    : LocusMatchCategoryAggregator.LocusMatchCategoryFromPositionScores(scoreDetailsPerPosition);

                var scoreDetails = new LocusScoreDetails
                {
                    IsLocusTyped = donorScoringInfo.DonorHla.GetLocus(locus).Position1And2NotNull(),
                    ScoreDetailsAtPosition1 = scoreDetailsPerPosition.Position1,
                    ScoreDetailsAtPosition2 = scoreDetailsPerPosition.Position2,
                    MatchCategory = matchCategory,
                    MismatchDirection = GetMismatchDirection(matchCategory, locus, dpb1TceGroupMatchType)
                };
                scoreResult.SetScoreDetailsForLocus(locus, scoreDetails);
            }

            scoreResult.AggregateScoreDetails = scoreResultAggregator.AggregateScoreDetails(
                new ScoreResultAggregatorParameters
                {
                    ScoreResult = scoreResult,
                    ScoredLoci = criteria.LociToScore.ToList(),
                    LociToExclude = criteria.LociToExcludeFromAggregateScore.ToList()
                });

            return scoreResult;
        }

        private LocusPositionScoreDetails BuildScoreDetailsForPosition(MatchGrade matchGrade, MatchConfidence matchConfidence)
        {
            return new LocusPositionScoreDetails
            {
                MatchGrade = matchGrade,
                MatchGradeScore = matchScoreCalculator.CalculateScoreForMatchGrade(matchGrade),
                MatchConfidence = matchConfidence,
                MatchConfidenceScore = matchScoreCalculator.CalculateScoreForMatchConfidence(matchConfidence),
            };
        }

        private static MismatchDirection? GetMismatchDirection(
            LocusMatchCategory locusMatchCategory,
            Locus locus,
            Dpb1TceGroupMatchType dpb1TceGroupMatchType)
        {
            if (locus != Locus.Dpb1)
            {
                return null;
            }

            if (locusMatchCategory != LocusMatchCategory.Mismatch)
            {
                return MismatchDirection.NotApplicable;
            }

            return LocusMatchCategoryAggregator.GetMismatchDirection(dpb1TceGroupMatchType);
        }
        
        private class DonorScoringInfo
        {
            public PhenotypeInfo<MatchGradeResult> Grades { get; set; }
            public PhenotypeInfo<MatchConfidence> Confidences { get; set; }
            public PhenotypeInfo<string> DonorHla { get; set; }
        }
    }
}