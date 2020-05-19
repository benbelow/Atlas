﻿using Atlas.Common.GeneticData;
using FluentAssertions;
using Atlas.Common.GeneticData.PhenotypeInfo;
using NUnit.Framework;

namespace Atlas.Common.Test.Core.PhenotypeInfo
{
    [TestFixture]
    public class LocusInfoTests
    {
        [Test]
        public void GetAtPosition_GetsValueAtSpecifiedPosition()
        {
            const string position1 = "[TEST] FIRST POSITION";
            const string position2 = "[TEST] SECOND POSITION";
            var locusInfo = new LocusInfo<string>()
            {
                Position1 = position1,
                Position2 = position2
            };

            locusInfo.GetAtPosition(LocusPosition.One).Should().Be(position1);
            locusInfo.GetAtPosition(LocusPosition.Two).Should().Be(position2);
        }

        [Test]
        public void SetAtPosition_SetsValueAtSpecifiedPosition()
        {
            const string position1 = "[TEST] FIRST POSITION";
            const string position2 = "[TEST] SECOND POSITION";
            var locusInfo = new LocusInfo<string>();

            locusInfo.SetAtPosition(LocusPosition.One, position1);
            locusInfo.SetAtPosition(LocusPosition.Two, position2);

            locusInfo.Position1.Should().Be(position1);
            locusInfo.Position2.Should().Be(position2);
        }
    }
}