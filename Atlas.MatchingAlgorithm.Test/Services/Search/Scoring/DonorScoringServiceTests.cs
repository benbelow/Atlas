﻿using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.Common.Test.SharedTestHelpers.Builders;
using Atlas.HlaMetadataDictionary.Services.DataRetrieval;
using Atlas.HlaMetadataDictionary.Test.TestHelpers.Builders;
using Atlas.MatchingAlgorithm.Client.Models.SearchResults.PerLocus;
using Atlas.MatchingAlgorithm.Common.Models.Scoring;
using Atlas.MatchingAlgorithm.Common.Models.SearchResults;
using Atlas.MatchingAlgorithm.Data.Models.SearchResults;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders;
using Atlas.MatchingAlgorithm.Services.Search.Scoring;
using Atlas.MatchingAlgorithm.Services.Search.Scoring.Aggregation;
using Atlas.MatchingAlgorithm.Services.Search.Scoring.Confidence;
using Atlas.MatchingAlgorithm.Services.Search.Scoring.Grading;
using Atlas.MatchingAlgorithm.Services.Search.Scoring.Ranking;
using Atlas.MatchingAlgorithm.Test.TestHelpers.Builders;
using Atlas.MatchingAlgorithm.Test.TestHelpers.Builders.SearchResults;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlas.MatchingAlgorithm.Test.Services.Search.Scoring
{
    [TestFixture]
    public class DonorScoringServiceTests
    {
        private IHlaScoringMetadataService scoringMetadataService;
        private IGradingService gradingService;
        private IConfidenceService confidenceService;
        private IRankingService rankingService;
        private IMatchScoreCalculator matchScoreCalculator;
        private IScoreResultAggregator scoreResultAggregator;

        private IDonorScoringService donorScoringService;

        [SetUp]
        public void SetUp()
        {
            scoringMetadataService = Substitute.For<IHlaScoringMetadataService>();
            gradingService = Substitute.For<IGradingService>();
            confidenceService = Substitute.For<IConfidenceService>();
            rankingService = Substitute.For<IRankingService>();
            matchScoreCalculator = Substitute.For<IMatchScoreCalculator>();
            scoreResultAggregator = Substitute.For<IScoreResultAggregator>();
            var hlaVersionAccessor = Substitute.For<IActiveHlaNomenclatureVersionAccessor>();

            rankingService.RankSearchResults(Arg.Any<IEnumerable<MatchAndScoreResult>>())
                .Returns(callInfo => (IEnumerable<MatchAndScoreResult>)callInfo.Args().First());
            gradingService.CalculateGrades(null, null).ReturnsForAnyArgs(new PhenotypeInfo<MatchGradeResult>(new MatchGradeResult()));
            confidenceService.CalculateMatchConfidences(null, null, null).ReturnsForAnyArgs(new PhenotypeInfo<MatchConfidence>());

            var hlaMetadataDictionaryBuilder = new HlaMetadataDictionaryBuilder().Using(scoringMetadataService);

            donorScoringService = new DonorScoringService(
                hlaMetadataDictionaryBuilder,
                hlaVersionAccessor,
                gradingService,
                confidenceService,
                rankingService,
                matchScoreCalculator,
                scoreResultAggregator
            );
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_NoLociToScore_ReturnsMatchResultsWithNoScoreResults()
        {
            var matchResults = new List<MatchResult>
            {
                new MatchResultBuilder().WithHlaAtLocus(Locus.A, "hla-a").Build(),
                new MatchResultBuilder().WithHlaAtLocus(Locus.B, "hla-b").Build()
            };

            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.MatchResults, matchResults)
                .With(x => x.LociToScore, new List<Locus>())
                .Build();

            var results = await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            results.Select(r => r.MatchResult).Should().BeEquivalentTo(matchResults);
            results.Select(r => r.ScoreResult).Should().AllBeEquivalentTo((ScoreResult)null);
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_FetchesScoringMetadataForRequestedLociForAllResults()
        {
            // 3 loci x 2 x 2 results
            const int expectedNumberOfFetches = 12;
            var lociToScore = new List<Locus> { Locus.A, Locus.B, Locus.C };

            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.MatchResults, new[] { new MatchResultBuilder().Build(), new MatchResultBuilder().Build() })
                .With(x => x.LociToScore, lociToScore)
                .Build();

            await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            await scoringMetadataService.Received(expectedNumberOfFetches).GetHlaMetadata(Arg.Any<Locus>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_DoesNotFetchScoringDataForUntypedLociForResults()
        {
            const Locus locus = Locus.B;
            const string patientHlaAtLocus = "patient-hla-locus-B";

            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.PatientHla, new PhenotypeInfoBuilder<string>().WithDataAt(locus, patientHlaAtLocus).Build())
                .With(x => x.MatchResults, new[] { new MatchResultBuilder().WithHlaAtLocus(locus, null).Build() })
                .Build();

            await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            await scoringMetadataService.DidNotReceive().GetHlaMetadata(locus, Arg.Is<string>(s => s != patientHlaAtLocus), Arg.Any<string>());
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_FetchesScoringMetadataForRequestedLociForPatientHla()
        {
            // 2 loci x 2 positions
            const int expectedNumberOfFetches = 4;
            var lociToScore = new List<Locus> { Locus.Dqb1, Locus.Drb1 };

            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.PatientHla, new PhenotypeInfo<string>("hla"))
                .With(x => x.MatchResults, new List<MatchResult>())
                .With(x => x.LociToScore, lociToScore)
                .Build();

            await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            await scoringMetadataService.Received(expectedNumberOfFetches).GetHlaMetadata(Arg.Any<Locus>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_DoesNotFetchScoringDataForUntypedLociForPatient()
        {
            const Locus untypedLocus = Locus.B;
            var patientHla = new PhenotypeInfoBuilder<string>("hla").WithDataAt(untypedLocus, (string)default).Build();

            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.PatientHla, patientHla)
                .With(x => x.MatchResults, new List<MatchResult>())
                .Build();

            await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            await scoringMetadataService.DidNotReceive().GetHlaMetadata(untypedLocus, Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_DoesNotModifyMatchDetailsForResults()
        {
            var matchResult = new MatchResultBuilder().Build();

            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.MatchResults, new[] { matchResult })
                .Build();

            var results = await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            results.First().MatchResult.Should().BeEquivalentTo(matchResult);
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_ReturnsMatchGradeForMatchResults()
        {
            const MatchGrade expectedMatchGradeAtA1 = MatchGrade.GGroup;
            const MatchGrade expectedMatchGradeAtB2 = MatchGrade.Protein;

            var matchGrades = new PhenotypeInfoBuilder<MatchGradeResult>(new MatchGradeResult())
                .WithDataAt(Locus.A, LocusPosition.One, new MatchGradeResult { GradeResult = expectedMatchGradeAtA1 })
                .WithDataAt(Locus.B, LocusPosition.Two, new MatchGradeResult { GradeResult = expectedMatchGradeAtB2 })
                .Build();

            gradingService.CalculateGrades(null, null).ReturnsForAnyArgs(matchGrades);

            var results = await donorScoringService.ScoreMatchesAgainstPatientHla(MatchResultsScoringRequestBuilder.New.Build());

            // Check across multiple loci and positions
            results.First().ScoreResult.ScoreDetailsAtLocusA.ScoreDetailsAtPosition1.MatchGrade.Should().Be(expectedMatchGradeAtA1);
            results.First().ScoreResult.ScoreDetailsAtLocusB.ScoreDetailsAtPosition2.MatchGrade.Should().Be(expectedMatchGradeAtB2);
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_CalculatesMatchGradeForEachMatchResult()
        {
            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.MatchResults, new[] { new MatchResultBuilder().Build(), new MatchResultBuilder().Build() })
                .Build();

            await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            gradingService.ReceivedWithAnyArgs(2).CalculateGrades(null, null);
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_ReturnsMatchGradeScoreForMatchResults()
        {
            const MatchGrade matchGradeAtA1 = MatchGrade.GGroup;
            const MatchGrade matchGradeAtB2 = MatchGrade.Protein;
            var matchGrades = new PhenotypeInfoBuilder<MatchGradeResult>(new MatchGradeResult())
                .WithDataAt(Locus.A, LocusPosition.One, new MatchGradeResult { GradeResult = matchGradeAtA1 })
                .WithDataAt(Locus.B, LocusPosition.Two, new MatchGradeResult { GradeResult = matchGradeAtB2 })
                .Build();
            gradingService.CalculateGrades(null, null).ReturnsForAnyArgs(matchGrades);

            const int expectedMatchGradeScoreAtA1 = 190;
            const int expectedMatchGradeScoreAtB2 = 87;
            matchScoreCalculator
                .CalculateScoreForMatchGrade(Arg.Is<MatchGrade>(a => a == matchGradeAtA1))
                .Returns(expectedMatchGradeScoreAtA1);
            matchScoreCalculator
                .CalculateScoreForMatchGrade(Arg.Is<MatchGrade>(a => a == matchGradeAtB2))
                .Returns(expectedMatchGradeScoreAtB2);

            var results = (await donorScoringService.ScoreMatchesAgainstPatientHla(MatchResultsScoringRequestBuilder.New.Build())).ToList();

            // Check across multiple loci and positions
            results.First().ScoreResult.ScoreDetailsAtLocusA.ScoreDetailsAtPosition1.MatchGradeScore.Should().Be(expectedMatchGradeScoreAtA1);
            results.First().ScoreResult.ScoreDetailsAtLocusB.ScoreDetailsAtPosition2.MatchGradeScore.Should().Be(expectedMatchGradeScoreAtB2);
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_ReturnsMatchConfidenceForMatchResults()
        {
            const MatchConfidence expectedMatchConfidenceAtA1 = MatchConfidence.Mismatch;
            const MatchConfidence expectedMatchConfidenceAtB2 = MatchConfidence.Potential;

            confidenceService.CalculateMatchConfidences(null, null, null)
                .ReturnsForAnyArgs(new PhenotypeInfo<MatchConfidence>
                {
                    A = { Position1 = expectedMatchConfidenceAtA1 },
                    B = { Position2 = expectedMatchConfidenceAtB2 },
                });

            var results = (await donorScoringService.ScoreMatchesAgainstPatientHla(MatchResultsScoringRequestBuilder.New.Build())).ToList();

            // Check across multiple loci and positions
            results.First().ScoreResult.ScoreDetailsAtLocusA.ScoreDetailsAtPosition1.MatchConfidence.Should().Be(expectedMatchConfidenceAtA1);
            results.First().ScoreResult.ScoreDetailsAtLocusB.ScoreDetailsAtPosition2.MatchConfidence.Should().Be(expectedMatchConfidenceAtB2);
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_ReturnsMatchConfidenceScoreForMatchResults()
        {
            const MatchConfidence matchConfidenceAtA1 = MatchConfidence.Mismatch;
            const MatchConfidence matchConfidenceAtB2 = MatchConfidence.Potential;

            confidenceService.CalculateMatchConfidences(null, null, null)
                .ReturnsForAnyArgs(new PhenotypeInfo<MatchConfidence>
                {
                    A = { Position1 = matchConfidenceAtA1 },
                    B = { Position2 = matchConfidenceAtB2 },
                });

            const int expectedMatchConfidenceScoreAtA1 = 7;
            const int expectedMatchConfidenceScoreAtB2 = 340;
            matchScoreCalculator
                .CalculateScoreForMatchConfidence(Arg.Is<MatchConfidence>(a => a == matchConfidenceAtA1))
                .Returns(expectedMatchConfidenceScoreAtA1);
            matchScoreCalculator
                .CalculateScoreForMatchConfidence(Arg.Is<MatchConfidence>(a => a == matchConfidenceAtB2))
                .Returns(expectedMatchConfidenceScoreAtB2);

            var results = (await donorScoringService.ScoreMatchesAgainstPatientHla(MatchResultsScoringRequestBuilder.New.Build())).ToList();

            // Check across multiple loci and positions
            results.First().ScoreResult.ScoreDetailsAtLocusA.ScoreDetailsAtPosition1.MatchConfidenceScore.Should()
                .Be(expectedMatchConfidenceScoreAtA1);
            results.First().ScoreResult.ScoreDetailsAtLocusB.ScoreDetailsAtPosition2.MatchConfidenceScore.Should()
                .Be(expectedMatchConfidenceScoreAtB2);
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_CalculatesMatchConfidenceForEachMatchResult()
        {
            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.MatchResults, new[] { new MatchResultBuilder().Build(), new MatchResultBuilder().Build() })
                .Build();

            await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            confidenceService.ReceivedWithAnyArgs(2).CalculateMatchConfidences(null, null, null);
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_OnlyReturnsDetailsForScoredLoci()
        {
            const Locus scoredLocus = Locus.A;
            const Locus notScoredLocus = Locus.B;

            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.LociToScore, new[] { scoredLocus })
                .Build();

            var results = await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            var result = results.Single();
            result.ScoreResult.ScoreDetailsForLocus(scoredLocus).Should().NotBeNull();
            result.ScoreResult.ScoreDetailsForLocus(notScoredLocus).Should().BeNull();
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_SetsTypingStatusAtScoredLoci()
        {
            var lociToScore = new List<Locus> { Locus.A, Locus.C };

            var donorWithUntypedLoci = new MatchResultBuilder()
                .WithHlaAtLocus(Locus.A, "hla-a")
                .WithHlaAtLocus(Locus.C, null)
                .Build();

            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.LociToScore, lociToScore)
                .With(x => x.MatchResults, new[] { donorWithUntypedLoci })
                .Build();

            var results = await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            var result = results.Single();
            result.ScoreResult.ScoreDetailsAtLocusA.IsLocusTyped.Should().BeTrue();
            result.ScoreResult.ScoreDetailsAtLocusC.IsLocusTyped.Should().BeFalse();
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_RanksResults()
        {
            var expectedSortedResults = new List<MatchAndScoreResult> { new MatchAndScoreResultBuilder().Build() };
            rankingService.RankSearchResults(null).ReturnsForAnyArgs(expectedSortedResults);

            var results = await donorScoringService.ScoreMatchesAgainstPatientHla(MatchResultsScoringRequestBuilder.New.Build());

            results.Should().BeEquivalentTo(expectedSortedResults);
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_AggregatesScoringData()
        {
            await donorScoringService.ScoreMatchesAgainstPatientHla(MatchResultsScoringRequestBuilder.New.Build());

            scoreResultAggregator.Received().AggregateScoreDetails(Arg.Any<ScoreResultAggregatorParameters>());
        }

        [Test]
        public async Task ScoreMatchesAgainstHla_AssignsAggregateScoringData()
        {
            scoreResultAggregator.AggregateScoreDetails(Arg.Any<ScoreResultAggregatorParameters>()).Returns(new AggregateScoreDetails());

            var request = MatchResultsScoringRequestBuilder.New
                .With(x => x.MatchResults, new[] { new MatchResultBuilder().Build(), new MatchResultBuilder().Build() })
                .Build();

            var results = await donorScoringService.ScoreMatchesAgainstPatientHla(request);

            foreach (var result in results.Select(r => r.ScoreResult))
            {
                result.AggregateScoreDetails.Should().NotBeNull();
            }
        }
    }
}