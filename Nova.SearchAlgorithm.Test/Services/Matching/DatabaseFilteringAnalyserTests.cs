﻿using FluentAssertions;
using Nova.SearchAlgorithm.Client.Models;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Services.Matching;
using NUnit.Framework;

namespace Nova.SearchAlgorithm.Test.Services.Matching
{
    [TestFixture]
    public class DatabaseFilteringAnalyserTests
    {
        private IDatabaseFilteringAnalyser databaseFilteringAnalyser;

        [SetUp]
        public void SetUp()
        {
            databaseFilteringAnalyser = new DatabaseFilteringAnalyser();
        }

        [Test]
        public void ShouldFilterOnDonorTypeInDatabase_ForAdult_ReturnsFalse()
        {
            var criteria = new LocusSearchCriteria
            {
                SearchType = DonorType.Adult
            };

            var result = databaseFilteringAnalyser.ShouldFilterOnDonorTypeInDatabase(criteria);

            result.Should().BeFalse();
        }

        [Test]
        public void ShouldFilterOnDonorTypeInDatabase_ForCord_ReturnsTrue()
        {
            var criteria = new LocusSearchCriteria
            {
                SearchType = DonorType.Cord
            };

            var result = databaseFilteringAnalyser.ShouldFilterOnDonorTypeInDatabase(criteria);

            result.Should().BeTrue();
        }

        [Test]
        public void ShouldFilterOnRegistriesInDatabase_ForAnthonyNolanOnly_ReturnsTrue()
        {
            var criteria = new LocusSearchCriteria
            {
                Registries = new[] {RegistryCode.AN}
            };

            var result = databaseFilteringAnalyser.ShouldFilterOnRegistriesInDatabase(criteria);

            result.Should().BeTrue();
        }

        [Test]
        public void ShouldFilterOnRegistriesInDatabase_ForAlignedRegistries_ReturnsFalse()
        {
            var criteria = new LocusSearchCriteria
            {
                Registries = new[] {RegistryCode.AN, RegistryCode.WBS, RegistryCode.NHSBT, RegistryCode.DKMS}
            };

            var result = databaseFilteringAnalyser.ShouldFilterOnRegistriesInDatabase(criteria);

            result.Should().BeFalse();
        }
    }
}