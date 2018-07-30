﻿using System.Collections.Generic;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;
using Nova.SearchAlgorithm.MatchingDictionary.Models.MatchingTypings;
using NUnit.Framework;

namespace Nova.SearchAlgorithm.Test.MatchingDictionary.Services.HlaMatchPreCalculation.SerologyToSerology
{
    [UseReporter(typeof(NUnitReporter))]
    [ApprovalTests.Namers.UseApprovalSubdirectory("../../../../Resources/MDPreCalc")]
    public class SerologyToSerologyMatchingTest : MatchedOnTestBase<ISerologyInfoForMatching>
    {
        [TestCaseSource(
            typeof(SerologyToSerologyMatchingTestCaseSources),
            nameof(SerologyToSerologyMatchingTestCaseSources.ExpectedSerologyInfos)
            )]
        public void MatchedSerologies_WhenValidSerology_SerologyInfoCorrectlyAssigned(
            string locus,
            MatchLocus matchLocus,
            string serologyName,
            SerologySubtype serologySubtype,
            object[][] matchingSerologies)
        {
            var actualSerologyInfo = GetSingleMatchingTyping(matchLocus, serologyName);

            var expectedSerologyTyping = new SerologyTyping(locus, serologyName, serologySubtype);

            var expectedMatchingSerologies = matchingSerologies
                .Select(m =>
                    new MatchingSerology(
                        new SerologyTyping(locus, m[0].ToString(), (SerologySubtype)m[1]),
                            (bool)m[2]));

            var expectedSerologyInfo = new SerologyInfoForMatching
            (
                expectedSerologyTyping,
                expectedSerologyTyping,
                expectedMatchingSerologies
            );

            actualSerologyInfo.ShouldBeEquivalentTo(expectedSerologyInfo);
        }

        [Test]
        public void MatchedSerologies_WhenDeletedSerology_SerologyInfoCorrectlyAssigned()
        {
            const MatchLocus matchLocus = MatchLocus.C;
            const string deletedSerologyName = "11";
            const string serologyUsedInMatchingName = "1";

            var actualSerologyInfo = GetSingleMatchingTyping(matchLocus, deletedSerologyName);

            const string locus = "Cw";
            var expectedDeletedSerology =
                new SerologyTyping(locus, deletedSerologyName, SerologySubtype.NotSplit, true);
            var expectedTypingUsedInMatching =
                new SerologyTyping(locus, serologyUsedInMatchingName, SerologySubtype.NotSplit);
            var expectedMatchingSerologies = new List<MatchingSerology>
            {
                new MatchingSerology(expectedDeletedSerology, true),
                new MatchingSerology(expectedTypingUsedInMatching, true)
            };

            var expectedSerologyInfo = new SerologyInfoForMatching
            (
                expectedDeletedSerology,
                expectedTypingUsedInMatching,
                expectedMatchingSerologies
            );

            actualSerologyInfo.ShouldBeEquivalentTo(expectedSerologyInfo);
        }

        [Test]
        public void MatchedSerologies_CollectionContainsAllExpectedSerology()
        {
            var str = string.Join("\r\n", SharedTestDataCache
                .GetMatchedHla()
                .OfType<MatchedSerology>()
                .OrderBy(s => s.HlaTyping.MatchLocus)
                .ThenBy(s => int.Parse(s.HlaTyping.Name))
                .Select(s => $"{s.HlaTyping.MatchLocus.ToString().ToUpper()}\t{s.HlaTyping.Name}")
                .ToList());
            Approvals.Verify(str);
        }

        [Test]
        public void MatchedSerologies_WhereSerologyIsValid_CollectionOnlyContainsValidRelationships()
        {
            var groupBySubtype = SharedTestDataCache
                .GetMatchedHla()
                .OfType<MatchedSerology>()
                .Where(m => !m.HlaTyping.IsDeleted)
                .Select(m => new
                {
                    MatchedType = (SerologyTyping)m.HlaTyping,
                    SubtypeCounts = m.MatchingSerologies
                        .Select(s => s.SerologyTyping)
                        .Where(s => !s.Equals(m.HlaTyping))
                        .GroupBy(s => s.SerologySubtype)
                        .Select(s => new { Subtype = s.Key, Count = s.Count() })
                }).ToList();

            var broads = groupBySubtype.Where(s => s.MatchedType.SerologySubtype == SerologySubtype.Broad).ToList();
            var splits = groupBySubtype.Where(s => s.MatchedType.SerologySubtype == SerologySubtype.Split).ToList();
            var associated = groupBySubtype.Where(s => s.MatchedType.SerologySubtype == SerologySubtype.Associated).ToList();
            var notSplits = groupBySubtype.Where(s => s.MatchedType.SerologySubtype == SerologySubtype.NotSplit).ToList();

            // Matching list should not contain the subtype of the Matched type
            Assert.IsEmpty(
                groupBySubtype.Where(s => s.SubtypeCounts.Any(sc => sc.Subtype == s.MatchedType.SerologySubtype)));

            // Broads cannot be matched to NotSplit, and must have at least two Splits
            Assert.IsEmpty(
                broads.Where(b => b.SubtypeCounts.Any(sc => sc.Subtype == SerologySubtype.NotSplit)));
            Assert.IsEmpty(
                broads.Where(b => b.SubtypeCounts.Single(sc => sc.Subtype == SerologySubtype.Split).Count < 2));

            // Splits cannot be matched to NotSplit, and must have one Broad
            Assert.IsEmpty(
                splits.Where(s => s.SubtypeCounts.Any(sc => sc.Subtype == SerologySubtype.NotSplit)));
            Assert.IsEmpty(
                splits.Where(s => s.SubtypeCounts.Single(sc => sc.Subtype == SerologySubtype.Broad).Count != 1));

            // Associated can only have:
            //      * 1 x NotSplit, or;
            //      * 1 x Broad, or;
            //      * 1 x Split and 1 x Broad
            Assert.IsEmpty(
                associated.Where(a => a.SubtypeCounts.Any(sc => sc.Count != 1)));
            Assert.IsEmpty(
                associated.Where(a =>
                    a.SubtypeCounts.Any(sc => sc.Subtype == SerologySubtype.NotSplit)
                    && a.SubtypeCounts.Any(sc => sc.Subtype != SerologySubtype.NotSplit)));
            Assert.IsEmpty(
                associated
                .Where(a => a.SubtypeCounts.Any(sc => sc.Subtype == SerologySubtype.Split))
                .Where(a => a.SubtypeCounts.All(sc => sc.Subtype != SerologySubtype.Broad)));

            // NotSplits can only be matched to Associated
            Assert.IsEmpty(
                notSplits.Where(n => n.SubtypeCounts.Any(sc => sc.Subtype != SerologySubtype.Associated)));
        }
    }
}
