﻿using LazyCache;
using Microsoft.Extensions.DependencyInjection;
using Atlas.HLAService.Client;
using Atlas.HlaMetadataDictionary.Services;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using Atlas.HlaMetadataDictionary.Exceptions;
using Atlas.Utils.Core.Models;
using Locus = Atlas.MatchingAlgorithm.Common.Models.Locus;

namespace Atlas.MatchingAlgorithm.Test.Integration.IntegrationTests.MatchingDictionary
{
    /// <summary>
    /// Fixture testing the base functionality of HlaSearchingLookupService via an arbitrarily chosen base class.
    /// Relies on a file-backed matching dictionary - tests may break if underlying data is changed.
    /// </summary>
    [TestFixture]
    public class HlaSearchingLookupLookupTests
    {
        private const Locus DefaultLocus = Locus.A;
        private const LocusType DefaultLocusType = LocusType.A;
        private const string CacheKey = "NmdpCodeLookup_A";

        private IHlaMatchingLookupService lookupService;
        private IHlaServiceClient hlaServiceClient;
        private IAppCache appCache;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            lookupService = DependencyInjection.DependencyInjection.Provider.GetService<IHlaMatchingLookupService>();
            hlaServiceClient = DependencyInjection.DependencyInjection.Provider.GetService<IHlaServiceClient>();
            appCache = DependencyInjection.DependencyInjection.Provider.GetService<IAppCache>();
        }

        [SetUp]
        public void SetUp()
        {
            hlaServiceClient
                .GetAllelesForDefinedNmdpCode(DefaultLocusType, Arg.Any<string>())
                .Returns(new List<string>());

            // clear NMDP code allele mappings between tests
            appCache.Remove(CacheKey);
        }

        [Test]
        public void GetHlaLookupResult_WhenInvalidHlaTyping_ThrowsException()
        {
            const string hlaName = "XYZ:123:INVALID";

            Assert.ThrowsAsync<MatchingDictionaryException>(
                async () => await lookupService.GetHlaLookupResult(DefaultLocus, hlaName, null));
        }

        [Test]
        public void GetHlaLookupResult_WhenNmdpCodeContainsAlleleNotInMatchingDictionary_ThrowsException()
        {
            const string missingAllele = "9999:9999";

            // NMDP code value does not matter, but does need to conform to the expected pattern
            const string nmdpCode = "99:CODE";
            hlaServiceClient
                .GetAllelesForDefinedNmdpCode(DefaultLocusType, nmdpCode)
                .Returns(new List<string> { missingAllele });

            Assert.ThrowsAsync<MatchingDictionaryException>(async () => 
                await lookupService.GetHlaLookupResult(DefaultLocus, nmdpCode, null));
        }

        [Test]
        public void GetHlaLookupResult_WhenAlleleStringOfNamesContainsAlleleNotInMatchingDictionary_ThrowsException()
        {
            const string existingAllele = "01:133";
            const string missingAllele = "9999:9999";
            const string alleleString = existingAllele + "/" + missingAllele;

            Assert.ThrowsAsync<MatchingDictionaryException>(async () =>
                await lookupService.GetHlaLookupResult(DefaultLocus, alleleString, null));
        }

        [Test]
        public void GetHlaLookupResult_WhenAlleleStringOfSubtypesContainsAlleleNotInMatchingDictionary_ThrowsException()
        {
            const string alleleString = "01:133/9999";

            Assert.ThrowsAsync<MatchingDictionaryException>(async () =>
                await lookupService.GetHlaLookupResult(DefaultLocus, alleleString, null));
        }
    }
}
