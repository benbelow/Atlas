﻿using Autofac;
using FluentAssertions;
using Nova.SearchAlgorithm.Client.Models.SearchResults;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Common.Repositories;
using Nova.SearchAlgorithm.Services;
using Nova.SearchAlgorithm.Test.Integration.TestData;
using Nova.SearchAlgorithm.Test.Integration.TestHelpers.Builders;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

namespace Nova.SearchAlgorithm.Test.Integration.IntegrationTests.Search
{
    /// <summary>
    /// Tests to cover the DPB1 permissive mismatch feature.
    /// </summary>
    public class Dpb1ScoringTests : IntegrationTestBase
    {
        private const string DefaultDpb1Hla = "01:01:01:01";
        private const string MismatchedDpb1HlaWithSameTceGroup = "02:01:02:01";
        private const string MismatchedDpb1HlaWithDifferentTceGroup = "03:01:01:01";
        private const string MismatchedDpb1HlaWithNoTceGroup = "679:01";

        private readonly MatchGrade[] dpb1MismatchGrades =
        {
            MatchGrade.Mismatch,
            MatchGrade.PermissiveMismatch
        };

        private ISearchService searchService;
        private PhenotypeInfo<string> defaultPhenotype;
        private int testDonorId;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            defaultPhenotype = SetDefaultPhenotype();
            testDonorId = SetupTestDonor(defaultPhenotype);
        }

        [SetUp]
        public void ResolveSearchService()
        {
            searchService = Container.Resolve<ISearchService>();
        }

        [Test]
        public async Task Search_SixOutOfSix_PatientAndDonorHaveTwoMatchingDpb1Typings_NoMismatchGradesAtDpb1()
        {
            var result = await RunSixOutOfSixSearchWithPatientPhenotypeOf(defaultPhenotype);

            dpb1MismatchGrades.Should().NotContain(result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionOne.MatchGrade);
            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionOne.MatchConfidence.Should().NotBe(MatchConfidence.Mismatch);

            dpb1MismatchGrades.Should().NotContain(result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionTwo.MatchGrade);
            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionTwo.MatchConfidence.Should().NotBe(MatchConfidence.Mismatch);
        }

        [Test]
        public async Task Search_SixOutOfSix_PatientAndDonorDpb1TypingsAreMismatched_ButHaveSameTceGroup_TwoPermissiveMismatchGradesAtDpb1()
        {
            var patientPhenotype = GetPatientPhenotype(MismatchedDpb1HlaWithSameTceGroup);
            var result = await RunSixOutOfSixSearchWithPatientPhenotypeOf(patientPhenotype);

            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionOne.MatchGrade.Should().Be(MatchGrade.PermissiveMismatch);
            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionOne.MatchConfidence.Should().Be(MatchConfidence.Mismatch);

            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionTwo.MatchGrade.Should().Be(MatchGrade.PermissiveMismatch);
            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionTwo.MatchConfidence.Should().Be(MatchConfidence.Mismatch);
        }

        [Test]
        public async Task Search_SixOutOfSix_PatientAndDonorHaveTwoMismatchedDpb1Typings_ButHaveDifferentTceGroups_TwoMismatchGradesAtDpb1()
        {
            var patientPhenotype = GetPatientPhenotype(MismatchedDpb1HlaWithDifferentTceGroup);
            var result = await RunSixOutOfSixSearchWithPatientPhenotypeOf(patientPhenotype);

            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionOne.MatchGrade.Should().Be(MatchGrade.Mismatch);
            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionOne.MatchConfidence.Should().Be(MatchConfidence.Mismatch);

            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionTwo.MatchGrade.Should().Be(MatchGrade.Mismatch);
            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionTwo.MatchConfidence.Should().Be(MatchConfidence.Mismatch);
        }

        [Test]
        public async Task Search_SixOutOfSix_PatientAndDonorHaveTwoMismatchedDpb1Typings_ButPatientHasNoTceGroupAssignments_TwoMismatchGradesAtDpb1()
        {
            var patientPhenotype = GetPatientPhenotype(MismatchedDpb1HlaWithNoTceGroup);
            var result = await RunSixOutOfSixSearchWithPatientPhenotypeOf(patientPhenotype);

            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionOne.MatchGrade.Should().Be(MatchGrade.Mismatch);
            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionOne.MatchConfidence.Should().Be(MatchConfidence.Mismatch);

            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionTwo.MatchGrade.Should().Be(MatchGrade.Mismatch);
            result.SearchResultAtLocusDpb1.ScoreDetailsAtPositionTwo.MatchConfidence.Should().Be(MatchConfidence.Mismatch);
        }

        private static PhenotypeInfo<string> SetDefaultPhenotype()
        {
            var defaultHlaSet = new TestHla.HeterozygousSet1();
            var phenotype = defaultHlaSet.SixLocus_SingleExpressingAlleles;
            phenotype.SetAtLocus(Locus.Dpb1, DefaultDpb1Hla);
            return phenotype;
        }

        private int SetupTestDonor(PhenotypeInfo<string> testDonorPhenotype)
        {
            var testDonor = BuildTestDonor(testDonorPhenotype);
            var donorRepository = Container.Resolve<IDonorImportRepository>();
            donorRepository.InsertDonorWithExpandedHla(testDonor).Wait();
            return testDonor.DonorId;
        }

        private InputDonorWithExpandedHla BuildTestDonor(PhenotypeInfo<string> testDonorPhenotype)
        {
            var expandHlaPhenotypeService = Container.Resolve<IExpandHlaPhenotypeService>();

            var matchingHlaPhenotype = expandHlaPhenotypeService
                .GetPhenotypeOfExpandedHla(testDonorPhenotype)
                .Result;

            return new InputDonorWithExpandedHlaBuilder(DonorIdGenerator.NextId())
                .WithMatchingHla(matchingHlaPhenotype)
                .Build();
        }

        private PhenotypeInfo<string> GetPatientPhenotype(string dpb1Hla)
        {
            var modifiedPhenotype = new PhenotypeInfo<string>(defaultPhenotype);
            modifiedPhenotype.SetAtLocus(Locus.Dpb1, dpb1Hla);

            return modifiedPhenotype;
        }

        private async Task<SearchResult> RunSixOutOfSixSearchWithPatientPhenotypeOf(PhenotypeInfo<string> patientPhenotype)
        {
            var searchRequest = new SearchRequestFromHlasBuilder(patientPhenotype)
                .SixOutOfSix()
                .Build();

            var searchResults = await searchService.Search(searchRequest);
            return searchResults.SingleOrDefault(d => d.DonorId == testDonorId);
        }
    }
}
