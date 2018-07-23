using FluentAssertions;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups.ScoringLookup;
using Nova.SearchAlgorithm.MatchingDictionary.Services.HlaDataConversion;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Nova.SearchAlgorithm.Test.MatchingDictionary.Services.HlaDataConversion
{
    [TestFixture]
    public class HlaScoringDataConverterTest :
        MatchedHlaDataConverterTestBase<HlaScoringDataConverter>
    {
        private static readonly List<SerologyEntry> SerologyEntries =
            new List<SerologyEntry> { new SerologyEntry(SerologyName, SeroSubtype) };

        [TestCase("999:999", "999")]
        [TestCase("999:999Q", "999")]
        public override void ConvertToHlaLookupResults_WhenTwoFieldAllele_GeneratesLookupResults_ForOriginalNameAndXxCode(
            string alleleName, string xxCodeLookupName)
        {
            var matchedAllele = BuildMatchedAllele(alleleName);
            var actualLookupResults = LookupResultGenerator.ConvertToHlaLookupResults(new[] { matchedAllele });

            var expectedLookupResults = new List<IHlaLookupResult>
            {
                BuildSingleAlleleLookupResult(alleleName),
                BuildXxCodeLookupResult(new[] {alleleName}, xxCodeLookupName)
            };

            actualLookupResults.Should().BeEquivalentTo(expectedLookupResults);
        }

        [TestCase("999:999:999", "", "999:999", "999")]
        [TestCase("999:999:999:999", "", "999:999", "999")]
        [TestCase("999:999:999L", "L", "999:999", "999")]
        [TestCase("999:999:999:999N", "N", "999:999", "999")]
        public override void ConvertToHlaLookupResults_WhenThreeOrFourFieldAllele_GeneratesLookupResults_ForOriginalNameAndNmdpCodeAndXxCode(
            string alleleName, string expressionSuffix, string nmdpCodeLookupName, string xxCodeLookupName)
        {
            var matchedAllele = BuildMatchedAllele(alleleName);
            var actualLookupResults = LookupResultGenerator.ConvertToHlaLookupResults(new[] { matchedAllele });

            var expectedLookupResults = new List<IHlaLookupResult>
            {
                BuildSingleAlleleLookupResult(alleleName),
                BuildMultipleAlleleLookupResult(nmdpCodeLookupName + expressionSuffix, new []{alleleName}),
                BuildXxCodeLookupResult(new []{alleleName}, xxCodeLookupName)
            };

            actualLookupResults.Should().BeEquivalentTo(expectedLookupResults);
        }

        [Test]
        public override void ConvertToHlaLookupResults_WhenAllelesHaveSameTruncatedNameVariant_GeneratesLookupResult_ForEachUniqueLookupName()
        {
            string[] alleles = { "999:999:998", "999:999:999:01", "999:999:999:02", "999:999:999:03N" };
            const string nmdpCodeLookupName = "999:999";
            const string nmdpCodeLookupNameWithExpressionSuffix = "999:999N";
            const string xxCodeLookupName = "999";

            var matchedAlleles = alleles.Select(BuildMatchedAllele).ToList();
            var actualLookupResults = LookupResultGenerator.ConvertToHlaLookupResults(matchedAlleles);

            var expectedLookupResults = new List<IHlaLookupResult>
            {
                BuildSingleAlleleLookupResult(alleles[0]),
                BuildSingleAlleleLookupResult(alleles[1]),
                BuildSingleAlleleLookupResult(alleles[2]),
                BuildSingleAlleleLookupResult(alleles[3]),
                BuildMultipleAlleleLookupResult(nmdpCodeLookupName, new[]{ alleles[0], alleles[1], alleles[2]}),
                BuildMultipleAlleleLookupResult(nmdpCodeLookupNameWithExpressionSuffix, new []{alleles[3]}),
                BuildXxCodeLookupResult(alleles, xxCodeLookupName)
            };

            actualLookupResults.Should().BeEquivalentTo(expectedLookupResults);
        }

        protected override IHlaLookupResult BuildSerologyHlaLookupResult()
        {
            var scoringInfo = new SerologyScoringInfo(SeroSubtype, SerologyEntries);

            return new HlaScoringLookupResult(
                MatchedLocus,
                SerologyName,
                LookupResultCategory.Serology,
                scoringInfo);
        }

        private static IHlaLookupResult BuildSingleAlleleLookupResult(string alleleName)
        {
            return new HlaScoringLookupResult(
                MatchedLocus,
                alleleName,
                LookupResultCategory.OriginalAllele,
                BuildSingleAlleleScoringInfo(alleleName)
            );
        }

        private static IHlaLookupResult BuildMultipleAlleleLookupResult(string lookupName, IEnumerable<string> alleleNames)
        {
            return new HlaScoringLookupResult(
                MatchedLocus,
                lookupName,
                LookupResultCategory.NmdpCodeAllele,
                new MultipleAlleleScoringInfo(alleleNames.Select(BuildSingleAlleleScoringInfo))
            );
        }

        private static IHlaLookupResult BuildXxCodeLookupResult(IEnumerable<string> alleleNames, string xxCodeLookupName)
        {
            var alleleNamesCollection = alleleNames.ToList();

            return new HlaScoringLookupResult(
                MatchedLocus,
                xxCodeLookupName,
                LookupResultCategory.XxCode,
                new XxCodeScoringInfo(alleleNamesCollection, alleleNamesCollection, SerologyEntries)
            );
        }

        private static SingleAlleleScoringInfo BuildSingleAlleleScoringInfo(string alleleName)
        {
            return new SingleAlleleScoringInfo(
                alleleName,
                false,
                AlleleTypingStatus.GetDefaultStatus(),
                alleleName,
                alleleName,
                SerologyEntries
                );
        }
    }
}
