﻿using FluentAssertions;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingTypings;
using NUnit.Framework;
using System.Linq;

namespace Nova.SearchAlgorithm.Test.MatchingDictionary.Services.HlaMatchPreCalculation.AlleleToSerology
{
    public class AlleleToSerologyMatchingTest : MatchedOnTestBase<MatchedAllele>
    {
        [TestCaseSource(
            typeof(AlleleToSerologyMatchingTestCaseSources),
            nameof(AlleleToSerologyMatchingTestCaseSources.ExpressingAllelesMatchingSerologies))]
        public void AlleleToSerologyMatching_ExpressedAlleles_HaveCorrectMatchingSerology(
            MatchLocus matchLocus,
            string alleleName,
            object[] matchingSerologies)
        {
            var actualMatchingSerologies = GetSingleMatchingTyping(matchLocus, alleleName).MatchingSerologies;
            var expectedMatchingSerologies = matchingSerologies
                .Select(m => (object[])m)
                .Select(BuildMatchingSerology);

            actualMatchingSerologies.ShouldBeEquivalentTo(expectedMatchingSerologies);
        }

        [Test]
        public void AlleleToSerologyMatching_NonExpressedAlleles_HaveNoMatchingSerology()
        {
            var serologyCounts = MatchedHla
                .Where(m => !m.HlaTyping.IsDeleted && m.HlaTyping is AlleleTyping)
                .Select(m => new
                {
                    Allele = m.HlaTyping as AlleleTyping,
                    SerologyCount = m.MatchingSerologies.Count()
                });

            serologyCounts
                .Where(s => s.Allele.IsNullExpresser && s.SerologyCount != 0)
                .Should()
                .BeEmpty();
        }

        [TestCaseSource(
            typeof(AlleleToSerologyMatchingTestCaseSources),
            nameof(AlleleToSerologyMatchingTestCaseSources.DeletedAllelesMatchingSerologies))]
        public void AlleleToSerologyMatching_DeletedAlleles_IdenticalHlaUsedToFindMatchingSerology(
            MatchLocus matchLocus,
            string alleleName,
            object[] matchingSerologies)
        {
            var actualMatchingSerologies = GetSingleMatchingTyping(matchLocus, alleleName).MatchingSerologies;
            var expectedMatchingSerologies = matchingSerologies
                .Select(m => (object[])m)
                .Select(BuildMatchingSerology);

            actualMatchingSerologies.ShouldBeEquivalentTo(expectedMatchingSerologies);
        }

        [Test]
        public void AlleleToSerologyMatching_DeletedAlleleWithNoIdenticalHla_HasNoMatchingSerology()
        {
            var deletedNoIdentical = GetSingleMatchingTyping(MatchLocus.A, "02:100");
            deletedNoIdentical.MatchingSerologies.Should().BeEmpty();
        }

        [TestCaseSource(
            typeof(AlleleToSerologyMatchingTestCaseSources),
            nameof(AlleleToSerologyMatchingTestCaseSources.AllelesMappedToSpecificSubtypeMatchingSerologies))]
        public void AlleleToSerologyMatching_AlleleMappedToSpecificSubtype_HasCorrectMatchingSerologies(MatchLocus matchLocus,
            string alleleName,
            object[] matchingSerologies)
        {
            var actualMatchingSerologies = GetSingleMatchingTyping(matchLocus, alleleName).MatchingSerologies;
            var expectedMatchingSerologies = matchingSerologies
                .Select(m => (object[])m)
                .Select(BuildMatchingSerology);

            actualMatchingSerologies.ShouldBeEquivalentTo(expectedMatchingSerologies);
        }

        [TestCaseSource(
            typeof(AlleleToSerologyMatchingTestCaseSources),
            nameof(AlleleToSerologyMatchingTestCaseSources.B15AllelesMatchingSerologies))]
        public void AlleleToSerologyMatching_B15Alleles_HaveCorrectMatchingSerologies(
            object[] alleleDetails,
            object[] matchingSerologies)
        {
            var actualMatchingSerologies = 
                GetSingleMatchingTyping((MatchLocus)alleleDetails[0], alleleDetails[1].ToString())
                .MatchingSerologies;

            var expectedMatchingSerologies = matchingSerologies
                .Select(m => (object[])m)
                .Select(BuildMatchingSerology);

            actualMatchingSerologies.ShouldBeEquivalentTo(expectedMatchingSerologies);
        }

        [TestCaseSource(
            typeof(AlleleToSerologyMatchingTestCaseSources),
            nameof(AlleleToSerologyMatchingTestCaseSources.AllelesOfUnknownSerology))]
        public void AlleleToSerologyMatching_AllelesOfUnknownSerology_HaveCorrectMatchingSerologies(
            MatchLocus matchLocus,
            string alleleName,
            object[] matchingSerologies)
        {
            var actualMatchingSerologies = GetSingleMatchingTyping(matchLocus, alleleName).MatchingSerologies;
            var expectedMatchingSerologies = matchingSerologies
                .Select(m => (object[])m)
                .Select(BuildMatchingSerology);

            actualMatchingSerologies.ShouldBeEquivalentTo(expectedMatchingSerologies);
        }

        private static MatchingSerology BuildMatchingSerology(object[] dataSource)
        {
            var serology = new SerologyTyping(
                dataSource[0].ToString(),
                dataSource[1].ToString(),
                (SerologySubtype)dataSource[2]);

            var isDeletedMapping = (bool)dataSource[3];

            return new MatchingSerology(serology, isDeletedMapping);
        }
    }
}
