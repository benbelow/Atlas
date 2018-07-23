﻿using FluentAssertions;
using Nova.HLAService.Client.Models;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups.ScoringLookup;
using Nova.SearchAlgorithm.MatchingDictionary.Repositories;
using Nova.SearchAlgorithm.MatchingDictionary.Repositories.AzureStorage;
using Nova.SearchAlgorithm.MatchingDictionary.Services;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nova.SearchAlgorithm.Test.MatchingDictionary.Services.Lookups
{
    [TestFixture]
    public class HlaScoringLookupServiceTest : 
        HlaSearchingLookupServiceTestBase<IHlaScoringLookupRepository, IHlaScoringLookupService, IHlaScoringLookupResult>
    {
        [SetUp]
        public void HlaScoringLookupServiceTest_SetUpBeforeEachTest()
        {
            LookupService = new HlaScoringLookupService(
                HlaLookupRepository,
                AlleleNamesLookupService,
                HlaServiceClient,
                HlaCategorisationService,
                AlleleStringSplitterService,
                MemoryCache,
                Logger);
        }

        [Test]
        public async Task GetHlaLookupResult_WhenNmdpCode_ScoringInfoForAllAllelesIsReturned()
        {
            const string expectedLookupName = "99:NMDPCODE";
            const string firstAlleleName = "99:01";
            const string secondAlleleName = "99:50";
            const string thirdAlleleName = "99:99";
                        
            HlaCategorisationService
                .GetHlaTypingCategory(expectedLookupName)
                .Returns(HlaTypingCategory.NmdpCode);

            var alleleNames = new List<string> { firstAlleleName, secondAlleleName, thirdAlleleName };
            HlaServiceClient
                .GetAllelesForDefinedNmdpCode(MolecularLocus, expectedLookupName)
                .Returns(alleleNames);

            var firstEntry = BuildTableEntityForSingleAllele(firstAlleleName);
            var secondEntry = BuildTableEntityForSingleAllele(secondAlleleName);
            var thirdEntry = BuildTableEntityForSingleAllele(thirdAlleleName);

            HlaLookupRepository
                .GetHlaLookupTableEntityIfExists(MatchedLocus, Arg.Any<string>(), TypingMethod.Molecular)
                .Returns(firstEntry, secondEntry, thirdEntry);

            var actualResult = await LookupService.GetHlaLookupResult(MatchedLocus, expectedLookupName);
            var expectedResult = BuildExpectedMultipleAlleleLookupResult(expectedLookupName, alleleNames);

            actualResult.Should().Be(expectedResult);
        }

        [TestCase(HlaTypingCategory.AlleleStringOfSubtypes, "Family:Subtype1/Subtype2", "Family:Subtype1", "Family:Subtype2")]
        [TestCase(HlaTypingCategory.AlleleStringOfNames, "Allele1/Allele2", "Allele1", "Allele2")]
        public async Task GetHlaLookupResult_WhenAlleleString_MatchingHlaForAllAllelesIsReturned(
            HlaTypingCategory category, string expectedLookupName, string firstAlleleName, string secondAlleleName)
        {
            HlaCategorisationService
                .GetHlaTypingCategory(expectedLookupName)
                .Returns(category);

            var expectedAlleleNames = new List<string> {firstAlleleName, secondAlleleName};
            AlleleStringSplitterService.GetAlleleNamesFromAlleleString(expectedLookupName)
                .Returns(expectedAlleleNames);

            var firstEntry = BuildTableEntityForSingleAllele(firstAlleleName);
            var secondEntry = BuildTableEntityForSingleAllele(secondAlleleName);

            HlaLookupRepository
                .GetHlaLookupTableEntityIfExists(MatchedLocus, Arg.Any<string>(), TypingMethod.Molecular)
                .Returns(firstEntry, secondEntry);

            var actualResult = await LookupService.GetHlaLookupResult(MatchedLocus, expectedLookupName);
            var expectedResult = BuildExpectedMultipleAlleleLookupResult(expectedLookupName, expectedAlleleNames);

            actualResult.Should().Be(expectedResult);
        }

        protected override HlaLookupTableEntity BuildTableEntityForSingleAllele(string alleleName)
        {
            var scoringInfo = BuildSingleAlleleScoringInfo(alleleName);

            var lookupResult = new HlaScoringLookupResult(
                MatchedLocus,
                alleleName,
                LookupResultCategory.OriginalAllele,
                scoringInfo
                );

            return new HlaLookupTableEntity(lookupResult)
            {
                LookupResultCategoryAsString = LookupResultCategory.OriginalAllele.ToString()
            };
        }

        private static IHlaScoringLookupResult BuildExpectedMultipleAlleleLookupResult(string lookupName, IEnumerable<string> alleleNames)
        {
            var scoringInfo = new MultipleAlleleScoringInfo(
                alleleNames.Select(BuildSingleAlleleScoringInfo));

            return new HlaScoringLookupResult(
                MatchedLocus,
                lookupName,
                LookupResultCategory.MultipleAlleles,
                scoringInfo
                );
        }

        private static SingleAlleleScoringInfo BuildSingleAlleleScoringInfo(string alleleName)
        {
            var scoringInfo = new SingleAlleleScoringInfo(
                alleleName,
                false,
                AlleleTypingStatus.GetDefaultStatus(),
                alleleName,
                alleleName,
                new List<SerologyEntry>());
            return scoringInfo;
        }
    }
}

