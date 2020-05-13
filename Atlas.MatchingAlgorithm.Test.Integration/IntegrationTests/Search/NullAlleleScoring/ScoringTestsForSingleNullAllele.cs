﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Atlas.MatchingAlgorithm.Client.Models.SearchResults;
using Atlas.MatchingAlgorithm.Common.Models;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders.TransientSqlDatabase.RepositoryFactories;
using Atlas.MatchingAlgorithm.Services.MatchingDictionary;
using Atlas.MatchingAlgorithm.Services.Search;
using Atlas.MatchingAlgorithm.Test.Integration.TestData;
using Atlas.MatchingAlgorithm.Test.Integration.TestHelpers;
using Atlas.MatchingAlgorithm.Test.Integration.TestHelpers.Builders;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlas.MatchingAlgorithm.Data.Models.DonorInfo;
using Atlas.MatchingAlgorithm.Services.Donors;

// ReSharper disable InconsistentNaming

namespace Atlas.MatchingAlgorithm.Test.Integration.IntegrationTests.Search.NullAlleleScoring
{
    /// <summary>
    /// Confirm that scoring on single null alleles is as expected 
    /// when run as part of the larger search algorithm service.
    /// This fixture focuses on one locus with a single null allele typing at one position;
    /// there are other integration tests that cover expressing vs. expressing scoring.
    /// </summary>
    public class ScoringTestsForSingleNullAllele
    {
        private const Locus LocusUnderTest = Locus.A;
        private const TypePosition PositionUnderTest = TypePosition.One;
        private const TypePosition OtherPosition = TypePosition.Two;
        private const string OriginalNullAllele = "02:43N";
        private const string DifferentNullAllele = "11:69N";

        private List<MatchGrade> matchGradesForMatchingNullAlleles;
        private PhenotypeInfo<string> originalNullAlleleAtOnePositionPhenotype;
        private PhenotypeInfo<string> differentNullAlleleAtOnePositionPhenotype;
        private PhenotypeInfo<string> homozygousByTypingAtOneLocusPhenotype;
        private ISearchService searchService;
        private int originalNullAlleleDonorId;
        private int homozygousLocusDonorId;

        [OneTimeSetUp]
        public void ImportTestDonor()
        {
            matchGradesForMatchingNullAlleles = new List<MatchGrade>
            {
                MatchGrade.NullGDna,
                MatchGrade.NullCDna,
                MatchGrade.NullPartial
            };

            SetPhenotypes();
            SetUpTestDonors();
        }

        [SetUp]
        public void ResolveSearchService()
        {
            searchService = DependencyInjection.DependencyInjection.Provider.GetService<ISearchService>();
        }

        [Test]
        public async Task Search_SixOutOfSix_WhenPatientAndDonorHaveSameSingleNullAllele_ThenMatchingNullGradeAndDefiniteConfidenceAssigned()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(originalNullAlleleAtOnePositionPhenotype)
                .SixOutOfSix()
                .Build();

            var results = await searchService.Search(searchRequest);
            var result = results.SingleOrDefault(d => d.DonorId == originalNullAlleleDonorId);

            // Position under test
            matchGradesForMatchingNullAlleles.Should().Contain(result.SearchResultAtLocusA.ScoreDetailsAtPositionOne.MatchGrade);
            result.SearchResultAtLocusA.ScoreDetailsAtPositionOne.MatchConfidence.Should().Be(MatchConfidence.Definite);

            // Other position at same locus
            result.SearchResultAtLocusA.ScoreDetailsAtPositionTwo.MatchGrade.Should().NotBe(MatchGrade.Mismatch);
            result.SearchResultAtLocusA.ScoreDetailsAtPositionTwo.MatchConfidence.Should().Be(MatchConfidence.Definite);
        }

        [Test]
        public async Task Search_SixOutOfSix_WhenPatientAndDonorHaveDifferentSingleNullAllele_ThenNullMismatchGradeAndDefiniteConfidenceAssigned()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(differentNullAlleleAtOnePositionPhenotype)
                .SixOutOfSix()
                .Build();

            var results = await searchService.Search(searchRequest);
            var result = results.SingleOrDefault(d => d.DonorId == originalNullAlleleDonorId);

            // Position under test
            result.SearchResultAtLocusA.ScoreDetailsAtPositionOne.MatchGrade.Should().Be(MatchGrade.NullMismatch);
            result.SearchResultAtLocusA.ScoreDetailsAtPositionOne.MatchConfidence.Should().Be(MatchConfidence.Definite);

            // Other position at same locus
            result.SearchResultAtLocusA.ScoreDetailsAtPositionTwo.MatchGrade.Should().NotBe(MatchGrade.Mismatch);
            result.SearchResultAtLocusA.ScoreDetailsAtPositionTwo.MatchConfidence.Should().Be(MatchConfidence.Definite);
        }

        [Test]
        public async Task Search_SixOutOfSix_WhenPatientIsHomozygousAndDonorHasSingleNullAllele_ThenMismatchGradeAndConfidenceAssigned()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(homozygousByTypingAtOneLocusPhenotype)
                .SixOutOfSix()
                .Build();

            var results = await searchService.Search(searchRequest);
            var result = results.SingleOrDefault(d => d.DonorId == originalNullAlleleDonorId);

            // Position under test
            result.SearchResultAtLocusA.ScoreDetailsAtPositionOne.MatchGrade.Should().Be(MatchGrade.Mismatch);
            result.SearchResultAtLocusA.ScoreDetailsAtPositionOne.MatchConfidence.Should().Be(MatchConfidence.Mismatch);

            // Other position at same locus
            result.SearchResultAtLocusA.ScoreDetailsAtPositionTwo.MatchGrade.Should().NotBe(MatchGrade.Mismatch);
            result.SearchResultAtLocusA.ScoreDetailsAtPositionTwo.MatchConfidence.Should().Be(MatchConfidence.Definite);
        }

        [Test]
        public async Task Search_SixOutOfSix_WhenPatientHasSingleNullAlleleAndDonorIsHomozygous_ThenMismatchGradeAndConfidenceAssigned()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(originalNullAlleleAtOnePositionPhenotype)
                .SixOutOfSix()
                .Build();

            var results = await searchService.Search(searchRequest);
            var result = results.SingleOrDefault(d => d.DonorId == homozygousLocusDonorId);

            // Position under test
            result.SearchResultAtLocusA.ScoreDetailsAtPositionOne.MatchGrade.Should().Be(MatchGrade.Mismatch);
            result.SearchResultAtLocusA.ScoreDetailsAtPositionOne.MatchConfidence.Should().Be(MatchConfidence.Mismatch);

            // Other position at same locus
            result.SearchResultAtLocusA.ScoreDetailsAtPositionTwo.MatchGrade.Should().NotBe(MatchGrade.Mismatch);
            result.SearchResultAtLocusA.ScoreDetailsAtPositionTwo.MatchConfidence.Should().Be(MatchConfidence.Definite);
        }

        private void SetPhenotypes()
        {
            var originalHlaPhenotype = new SampleTestHlas.HeterozygousSet1().SixLocus_SingleExpressingAlleles;

            originalNullAlleleAtOnePositionPhenotype = originalHlaPhenotype.Map((l, p, hla) => hla);
            originalNullAlleleAtOnePositionPhenotype
                .SetAtPosition(LocusUnderTest, PositionUnderTest, OriginalNullAllele);

            differentNullAlleleAtOnePositionPhenotype = originalHlaPhenotype.Map((l, p, hla) => hla);
            differentNullAlleleAtOnePositionPhenotype
                .SetAtPosition(LocusUnderTest, PositionUnderTest, DifferentNullAllele);

            homozygousByTypingAtOneLocusPhenotype = originalHlaPhenotype.Map((l, p, hla) => hla);
            homozygousByTypingAtOneLocusPhenotype
                .SetAtLocus(LocusUnderTest, originalHlaPhenotype.DataAtPosition(LocusUnderTest, OtherPosition));
        }

        private void SetUpTestDonors()
        {
            originalNullAlleleDonorId = SetUpTestDonor(originalNullAlleleAtOnePositionPhenotype);
            homozygousLocusDonorId = SetUpTestDonor(homozygousByTypingAtOneLocusPhenotype);
        }

        private static int SetUpTestDonor(PhenotypeInfo<string> donorPhenotype)
        {
            var donorHlaExpander = DependencyInjection.DependencyInjection.Provider.GetService<IDonorHlaExpanderFactory>().BuildForActiveHlaNomenclatureVersion();
            var matchingHlaPhenotype = donorHlaExpander.ExpandDonorHlaAsync(new DonorInfo { HlaNames = donorPhenotype }).Result.MatchingHla;

            var testDonor = new DonorInfoWithTestHlaBuilder(DonorIdGenerator.NextId())
                .WithHla(matchingHlaPhenotype)
                .Build();

            var repositoryFactory =
                DependencyInjection.DependencyInjection.Provider.GetService<IActiveRepositoryFactory>();
            var donorRepository = repositoryFactory.GetDonorUpdateRepository();
            donorRepository.InsertBatchOfDonorsWithExpandedHla(new[] { testDonor }).Wait();

            return testDonor.DonorId;
        }
    }
}
