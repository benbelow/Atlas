﻿using FluentAssertions;
using Nova.SearchAlgorithm.Client.Models.SearchResults;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Common.Models.Scoring;
using Nova.SearchAlgorithm.Common.Models.SearchResults;
using Nova.SearchAlgorithm.MatchingDictionary.Services;
using Nova.SearchAlgorithm.Services.Scoring;
using Nova.SearchAlgorithm.Services.Scoring.Confidence;
using Nova.SearchAlgorithm.Services.Scoring.Grading;
using Nova.SearchAlgorithm.Services.Scoring.Ranking;
using Nova.SearchAlgorithm.Test.Builders.SearchResults;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nova.SearchAlgorithm.Services;
using Nova.SearchAlgorithm.Services.ConfigurationProviders;

namespace Nova.SearchAlgorithm.Test.Services.Scoring
{
    [TestFixture]
    public class DonorScoringServiceTests
    {
        private IHlaScoringLookupService scoringLookupService;
        private IGradingService gradingService;
        private IConfidenceService confidenceService;
        private IRankingService rankingService;
        private IMatchScoreCalculator matchScoreCalculator;

        private DonorScoringService donorScoringService;

        private readonly PhenotypeInfo<MatchGradeResult> defaultMatchGradeResults = new PhenotypeInfo<MatchGradeResult>
        {
            A = {Position1 = new MatchGradeResult(), Position2 = new MatchGradeResult()},
            B = {Position1 = new MatchGradeResult(), Position2 = new MatchGradeResult()},
            C = {Position1 = new MatchGradeResult(), Position2 = new MatchGradeResult()},
            Dpb1 = {Position1 = new MatchGradeResult(), Position2 = new MatchGradeResult()},
            Dqb1 = {Position1 = new MatchGradeResult(), Position2 = new MatchGradeResult()},
            Drb1 = {Position1 = new MatchGradeResult(), Position2 = new MatchGradeResult()},
        };

        [SetUp]
        public void SetUp()
        {
            scoringLookupService = Substitute.For<IHlaScoringLookupService>();
            gradingService = Substitute.For<IGradingService>();
            confidenceService = Substitute.For<IConfidenceService>();
            rankingService = Substitute.For<IRankingService>();
            matchScoreCalculator = Substitute.For<IMatchScoreCalculator>();
            var wmdaHlaVersionProvider = Substitute.For<IWmdaHlaVersionProvider>();

            rankingService.RankSearchResults(Arg.Any<IEnumerable<MatchAndScoreResult>>())
                .Returns(callInfo => (IEnumerable<MatchAndScoreResult>) callInfo.Args().First());
            gradingService.CalculateGrades(null, null).ReturnsForAnyArgs(defaultMatchGradeResults);
            confidenceService.CalculateMatchConfidences(null, null, null).ReturnsForAnyArgs(new PhenotypeInfo<MatchConfidence>());

            donorScoringService = new DonorScoringService(
                scoringLookupService,
                gradingService,
                confidenceService,
                rankingService,
                matchScoreCalculator,
                wmdaHlaVersionProvider
            );
        }

        [Test]
        public async Task Score_FetchesScoringDataForAllLociForAllResults()
        {
            // 6 loci x 2 x 2 results
            const int expectedNumberOfFetches = 24;

            var patientHla = new PhenotypeInfo<string>();
            var result1 = new MatchResultBuilder().Build();
            var result2 = new MatchResultBuilder().Build();

            await donorScoringService.ScoreMatchesAgainstHla(new[] {result1, result2}, patientHla);

            await scoringLookupService.Received(expectedNumberOfFetches).GetHlaLookupResult(Arg.Any<Locus>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task Score_DoesNotFetchScoringDataForUntypedLociForResults()
        {
            const Locus locus = Locus.B;
            const string patientHlaAtLocus = "patient-hla-locus-B";
            var patientHla = new PhenotypeInfo<string>();
            patientHla.SetAtLocus(locus, patientHlaAtLocus);
            var result1 = new MatchResultBuilder()
                .WithHlaAtLocus(locus, null)
                .Build();

            await donorScoringService.ScoreMatchesAgainstHla(new[] {result1}, patientHla);

            await scoringLookupService.DidNotReceive().GetHlaLookupResult(locus, Arg.Is<string>(s => s != patientHlaAtLocus), Arg.Any<string>());
        }

        [Test]
        public async Task Score_FetchesScoringDataForAllLociForPatientHla()
        {
            // 6 loci x 2 positions
            const int expectedNumberOfFetches = 12;

            var patientHla = new PhenotypeInfo<string>
            {
                A = {Position1 = "hla-a-1", Position2 = "hla-a-2"},
                B = {Position1 = "hla-b-1", Position2 = "hla-b-2"},
                C = {Position1 = "hla-c-1", Position2 = "hla-c-2"},
                Dpb1 = {Position1 = "hla-dpb1-1", Position2 = "hla-dpb1-2"},
                Dqb1 = {Position1 = "hla-dqb1-1", Position2 = "hla-dqb1-2"},
                Drb1 = {Position1 = "hla-drb1-1", Position2 = "hla-drb1-2"},
            };

            await donorScoringService.ScoreMatchesAgainstHla(new List<MatchResult>(), patientHla);

            await scoringLookupService.Received(expectedNumberOfFetches).GetHlaLookupResult(Arg.Any<Locus>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task Score_DoesNotFetchScoringDataForUntypedLociForPatient()
        {
            var patientHla = new PhenotypeInfo<string>
            {
                A = {Position1 = "hla-1", Position2 = "hla-2"}
            };

            await donorScoringService.ScoreMatchesAgainstHla(new List<MatchResult>(), patientHla);

            await scoringLookupService.DidNotReceive().GetHlaLookupResult(Locus.B, Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task Score_DoesNotModifyMatchDetailsForResults()
        {
            var matchResult = new MatchResultBuilder().Build();

            var results = await donorScoringService.ScoreMatchesAgainstHla(new[] {matchResult}, new PhenotypeInfo<string>());

            results.First().MatchResult.ShouldBeEquivalentTo(matchResult);
        }

        [Test]
        public async Task Score_ReturnsMatchGradeForMatchResults()
        {
            const MatchGrade expectedMatchGradeAtA1 = MatchGrade.GGroup;
            const MatchGrade expectedMatchGradeAtB2 = MatchGrade.Protein;

            var matchResult = new MatchResultBuilder().Build();

            var matchGrades = defaultMatchGradeResults;
            matchGrades.A.Position1 = new MatchGradeResult {GradeResult = expectedMatchGradeAtA1};
            matchGrades.B.Position2 = new MatchGradeResult {GradeResult = expectedMatchGradeAtB2};

            gradingService.CalculateGrades(null, null).ReturnsForAnyArgs(matchGrades);

            var results = (await donorScoringService.ScoreMatchesAgainstHla(new[] {matchResult}, new PhenotypeInfo<string>())).ToList();

            // Check across multiple loci and positions
            results.First().ScoreResult.ScoreDetailsAtLocusA.ScoreDetailsAtPosition1.MatchGrade.Should().Be(expectedMatchGradeAtA1);
            results.First().ScoreResult.ScoreDetailsAtLocusB.ScoreDetailsAtPosition2.MatchGrade.Should().Be(expectedMatchGradeAtB2);
        }

        [Test]
        public async Task Score_CalculatesMatchGradeForEachMatchResult()
        {
            var matchResult1 = new MatchResultBuilder().Build();
            var matchResult2 = new MatchResultBuilder().Build();

            await donorScoringService.ScoreMatchesAgainstHla(new[] {matchResult1, matchResult2}, new PhenotypeInfo<string>());

            gradingService.ReceivedWithAnyArgs(2).CalculateGrades(null, null);
        }

        [Test]
        public async Task Score_ReturnsMatchGradeScoreForMatchResults()
        {
            const MatchGrade matchGradeAtA1 = MatchGrade.GGroup;
            const MatchGrade matchGradeAtB2 = MatchGrade.Protein;

            const int expectedMatchGradeScoreAtA1 = 190;
            const int expectedMatchGradeScoreAtB2 = 87;

            var matchResult = new MatchResultBuilder().Build();

            var matchGrades = defaultMatchGradeResults;
            matchGrades.A.Position1 = new MatchGradeResult {GradeResult = matchGradeAtA1};
            matchGrades.B.Position2 = new MatchGradeResult {GradeResult = matchGradeAtB2};

            gradingService.CalculateGrades(null, null).ReturnsForAnyArgs(matchGrades);
            matchScoreCalculator
                .CalculateScoreForMatchGrade(Arg.Is<MatchGrade>(a => a == matchGradeAtA1))
                .Returns(expectedMatchGradeScoreAtA1);
            matchScoreCalculator
                .CalculateScoreForMatchGrade(Arg.Is<MatchGrade>(a => a == matchGradeAtB2))
                .Returns(expectedMatchGradeScoreAtB2);

            var results = (await donorScoringService.ScoreMatchesAgainstHla(new[] {matchResult}, new PhenotypeInfo<string>())).ToList();

            // Check across multiple loci and positions
            results.First().ScoreResult.ScoreDetailsAtLocusA.ScoreDetailsAtPosition1.MatchGradeScore.Should().Be(expectedMatchGradeScoreAtA1);
            results.First().ScoreResult.ScoreDetailsAtLocusB.ScoreDetailsAtPosition2.MatchGradeScore.Should().Be(expectedMatchGradeScoreAtB2);
        }

        [Test]
        public async Task Score_ReturnsMatchConfidenceForMatchResults()
        {
            const MatchConfidence expectedMatchConfidenceAtA1 = MatchConfidence.Mismatch;
            const MatchConfidence expectedMatchConfidenceAtB2 = MatchConfidence.Potential;

            var matchResult = new MatchResultBuilder().Build();

            confidenceService.CalculateMatchConfidences(null, null, null)
                .ReturnsForAnyArgs(new PhenotypeInfo<MatchConfidence>
                {
                    A = {Position1 = expectedMatchConfidenceAtA1},
                    B = {Position2 = expectedMatchConfidenceAtB2},
                });

            var results = (await donorScoringService.ScoreMatchesAgainstHla(new[] {matchResult}, new PhenotypeInfo<string>())).ToList();

            // Check across multiple loci and positions
            results.First().ScoreResult.ScoreDetailsAtLocusA.ScoreDetailsAtPosition1.MatchConfidence.Should().Be(expectedMatchConfidenceAtA1);
            results.First().ScoreResult.ScoreDetailsAtLocusB.ScoreDetailsAtPosition2.MatchConfidence.Should().Be(expectedMatchConfidenceAtB2);
        }

        [Test]
        public async Task Score_ReturnsMatchConfidenceScoreForMatchResults()
        {
            const MatchConfidence matchConfidenceAtA1 = MatchConfidence.Mismatch;
            const MatchConfidence matchConfidenceAtB2 = MatchConfidence.Potential;

            const int expectedMatchConfidenceScoreAtA1 = 7;
            const int expectedMatchConfidenceScoreAtB2 = 340;

            confidenceService.CalculateMatchConfidences(null, null, null)
                .ReturnsForAnyArgs(new PhenotypeInfo<MatchConfidence>
                {
                    A = {Position1 = matchConfidenceAtA1},
                    B = {Position2 = matchConfidenceAtB2},
                });

            var matchResult = new MatchResultBuilder().Build();
            matchScoreCalculator
                .CalculateScoreForMatchConfidence(Arg.Is<MatchConfidence>(a => a == matchConfidenceAtA1))
                .Returns(expectedMatchConfidenceScoreAtA1);
            matchScoreCalculator
                .CalculateScoreForMatchConfidence(Arg.Is<MatchConfidence>(a => a == matchConfidenceAtB2))
                .Returns(expectedMatchConfidenceScoreAtB2);

            var results = (await donorScoringService.ScoreMatchesAgainstHla(new[] {matchResult}, new PhenotypeInfo<string>())).ToList();

            // Check across multiple loci and positions
            results.First().ScoreResult.ScoreDetailsAtLocusA.ScoreDetailsAtPosition1.MatchConfidenceScore.Should()
                .Be(expectedMatchConfidenceScoreAtA1);
            results.First().ScoreResult.ScoreDetailsAtLocusB.ScoreDetailsAtPosition2.MatchConfidenceScore.Should()
                .Be(expectedMatchConfidenceScoreAtB2);
        }

        [Test]
        public async Task Score_CalculatesMatchConfidenceForEachMatchResult()
        {
            var matchResult1 = new MatchResultBuilder().Build();
            var matchResult2 = new MatchResultBuilder().Build();

            await donorScoringService.ScoreMatchesAgainstHla(new[] {matchResult1, matchResult2}, new PhenotypeInfo<string>());

            confidenceService.ReceivedWithAnyArgs(2).CalculateMatchConfidences(null, null, null);
        }

        [Test]
        public async Task Score_ForUntypedDonorLoci_ReturnsIsDonorTypedAsFalse()
        {
            const Locus locus = Locus.A;
            scoringLookupService.GetHlaLookupResult(locus, Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();

            var matchResult1 = new MatchResultBuilder().Build();

            var results = await donorScoringService.ScoreMatchesAgainstHla(new[] {matchResult1}, new PhenotypeInfo<string>());

            results.Single().ScoreResult.ScoreDetailsForLocus(locus).IsLocusTyped.Should().BeFalse();
        }

        [Test]
        public async Task Score_ForTypedDonorLoci_ReturnsIsDonorTypedAsTrue()
        {
            const Locus locus = Locus.A;

            var matchResult1 = new MatchResultBuilder().Build();

            var results = await donorScoringService.ScoreMatchesAgainstHla(new[] {matchResult1}, new PhenotypeInfo<string>());

            results.Single().ScoreResult.ScoreDetailsForLocus(locus).IsLocusTyped.Should().BeTrue();
        }

        [Test]
        public async Task Score_ForTypedDonorLoci_ReturnsTypedLociCountEqualToNumberOfTypedLoci()
        {
            const Locus locus = Locus.A;
            scoringLookupService.GetHlaLookupResult(locus, Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();

            var matchResult1 = new MatchResultBuilder().Build();

            var results = await donorScoringService.ScoreMatchesAgainstHla(new[] {matchResult1}, new PhenotypeInfo<string>());

            // Results for 6 loci, one of which is untyped
            results.Single().ScoreResult.TypedLociCount.Should().Be(5);
        }

        [Test]
        public async Task Score_RanksResults()
        {
            var expectedSortedResults = new List<MatchAndScoreResult> {new MatchAndScoreResultBuilder().Build()};
            rankingService.RankSearchResults(null).ReturnsForAnyArgs(expectedSortedResults);

            var results = await donorScoringService.ScoreMatchesAgainstHla(new List<MatchResult>(), new PhenotypeInfo<string>());

            results.Should().BeEquivalentTo(expectedSortedResults);
        }
    }
}