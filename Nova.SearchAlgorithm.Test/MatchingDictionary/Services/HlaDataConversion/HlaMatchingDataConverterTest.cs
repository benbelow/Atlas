using FluentAssertions;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Lookups.MatchingLookup;
using Nova.SearchAlgorithm.MatchingDictionary.Services.HlaDataConversion;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Nova.SearchAlgorithm.Test.MatchingDictionary.Services.HlaDataConversion
{
    [TestFixture]
    public class HlaMatchingDataConverterTest :
        MatchedHlaDataConverterTestBase<HlaMatchingDataConverter>
    {
        [TestCase("999:999", "999")]
        [TestCase("999:999Q", "999")]
        public override void ConvertToHlaLookupResults_WhenTwoFieldAllele_GeneratesLookupResults_ForOriginalNameAndXxCode(
            string alleleName, string xxCodeLookupName)
        {
            var matchedAllele = BuildMatchedAllele(alleleName);
            var actualLookupResults = LookupResultGenerator.ConvertToHlaLookupResults(new[] { matchedAllele });

            var expectedLookupResults = new List<IHlaLookupResult>
            {
                BuildMolecularHlaLookupResult(alleleName),
                BuildMolecularHlaLookupResult(xxCodeLookupName, new []{alleleName})
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
                BuildMolecularHlaLookupResult(alleleName),
                BuildMolecularHlaLookupResult(nmdpCodeLookupName + expressionSuffix, new []{alleleName}),
                BuildMolecularHlaLookupResult(xxCodeLookupName, new[] {alleleName})
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
                BuildMolecularHlaLookupResult(alleles[0]),
                BuildMolecularHlaLookupResult(alleles[1]),
                BuildMolecularHlaLookupResult(alleles[2]),
                BuildMolecularHlaLookupResult(alleles[3]),
                BuildMolecularHlaLookupResult(nmdpCodeLookupName, new[] {alleles[0], alleles[1], alleles[2]}),
                BuildMolecularHlaLookupResult(nmdpCodeLookupNameWithExpressionSuffix, new[] {alleles[3]}),
                BuildMolecularHlaLookupResult(xxCodeLookupName, alleles)
            };

            actualLookupResults.Should().BeEquivalentTo(expectedLookupResults);
        }

        /// <summary>
        /// Builds a serology lookup result based on constant values.
        /// </summary>
        protected override IHlaLookupResult BuildSerologyHlaLookupResult()
        {
            return new HlaMatchingLookupResult(
                MatchedLocus,
                SerologyName,
                TypingMethod.Serology,
                new List<string>()
            );
        }

        /// <summary>
        /// Build a molecular lookup result with the allele name used as the Matching P Group by default,
        /// unless a list of 1 or more P Groups is supplied.
        /// </summary>
        private static IHlaLookupResult BuildMolecularHlaLookupResult(string alleleName, IEnumerable<string> pGroups = null)
        {
            return new HlaMatchingLookupResult(
                MatchedLocus,
                alleleName,
                TypingMethod.Molecular,
                pGroups ?? new[] { alleleName }
            );
        }
    }
}
