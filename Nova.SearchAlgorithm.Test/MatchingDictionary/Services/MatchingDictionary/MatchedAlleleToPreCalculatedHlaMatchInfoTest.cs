using FluentAssertions;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingDictionary;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingTypings;
using Nova.SearchAlgorithm.MatchingDictionary.Services.MatchingDictionary;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;

namespace Nova.SearchAlgorithm.Test.MatchingDictionary.Services.MatchingDictionary
{
    [TestFixture]
    public class MatchedAlleleToPreCalculatedHlaMatchInfoTest
    {
        [TestCase("01:01:02", "")]
        [TestCase("01:01:01:01", "")]
        [TestCase("01:01:38L", "L")]
        [TestCase("01:01:01:02N", "N")]
        public void MatchedAlleleToPreCalculatedHlaMatchInfo_ThreeOrFourFieldAllele_ThreeEntriesGenerated(string alleleName, string expressionSuffix)
        {
            var matchedAllele = BuildTestObjectFromAlleleName(alleleName);

            var expected = new List<PreCalculatedHlaMatchInfo>
            {
                new PreCalculatedHlaMatchInfo(matchedAllele, alleleName, MolecularSubtype.CompleteAllele),
                new PreCalculatedHlaMatchInfo(matchedAllele, "01:01" + expressionSuffix, MolecularSubtype.TwoFieldAllele),
                new PreCalculatedHlaMatchInfo(matchedAllele, "01", MolecularSubtype.FirstFieldAllele)
            };

            TestGenerationOfPreCalculatedHlaMatchInfoFromMatchedAlleles(new[] { matchedAllele }, expected);
        }

        [TestCase("01:32", "")]
        [TestCase("01:248Q", "Q")]
        public void MatchedAlleleToPreCalculatedHlaMatchInfo_TwoFieldAllele_TwoEntriesGenerated(string alleleName, string expressionSuffix)
        {
            var matchedAllele = BuildTestObjectFromAlleleName(alleleName);

            var expected = new List<PreCalculatedHlaMatchInfo>
            {
                new PreCalculatedHlaMatchInfo(matchedAllele, alleleName, MolecularSubtype.CompleteAllele),
                new PreCalculatedHlaMatchInfo(matchedAllele, "01", MolecularSubtype.FirstFieldAllele)
            };

            TestGenerationOfPreCalculatedHlaMatchInfoFromMatchedAlleles(new[] { matchedAllele }, expected);
        }

        [Test]
        public void MatchedAlleleToPreCalculatedHlaMatchInfo_AllelesWithSameTruncatedNames_OneEntryPerNameAndSubtypeCombination()
        {
            string[] alleles = { "01:01:01:01", "01:01:01:03", "01:01:51" };

            var matchedAlleles = alleles.Select(BuildTestObjectFromAlleleName).ToList();

            var expected = new List<PreCalculatedHlaMatchInfo>
            {
                new PreCalculatedHlaMatchInfo(matchedAlleles[0], alleles[0], MolecularSubtype.CompleteAllele),
                new PreCalculatedHlaMatchInfo(matchedAlleles[1], alleles[1], MolecularSubtype.CompleteAllele),
                new PreCalculatedHlaMatchInfo(matchedAlleles[2], alleles[2], MolecularSubtype.CompleteAllele),
                new PreCalculatedHlaMatchInfo(matchedAlleles[0], "01:01", MolecularSubtype.TwoFieldAllele),
                new PreCalculatedHlaMatchInfo(matchedAlleles[0], "01", MolecularSubtype.FirstFieldAllele)
            };

            TestGenerationOfPreCalculatedHlaMatchInfoFromMatchedAlleles(matchedAlleles, expected);
        }

        private static MatchedAllele BuildTestObjectFromAlleleName(string alleleName)
        {
            const MatchLocus matchLocus = MatchLocus.A;

            var infoForMatching = Substitute.For<IAlleleInfoForMatching>();
            infoForMatching.HlaTyping.Returns(new AlleleTyping(matchLocus, alleleName));

            return new MatchedAllele(infoForMatching, new List<SerologyMappingForAllele>());
        }

        private static void TestGenerationOfPreCalculatedHlaMatchInfoFromMatchedAlleles(
            IEnumerable<MatchedAllele> matchedAlleles, IEnumerable<PreCalculatedHlaMatchInfo> expected)
        {
            var actual = matchedAlleles.ToPreCalculatedHlaMatchInfo();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
