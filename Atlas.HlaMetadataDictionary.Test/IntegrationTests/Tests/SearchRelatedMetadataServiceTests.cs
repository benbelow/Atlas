﻿using System.Collections.Generic;
using Atlas.Common.Caching;
using Atlas.Common.GeneticData;
using Atlas.Common.Test.SharedTestHelpers;
using Atlas.HlaMetadataDictionary.ExternalInterface.Exceptions;
using Atlas.HlaMetadataDictionary.ExternalInterface.Models.Metadata;
using Atlas.HlaMetadataDictionary.Services.DataRetrieval;
using Atlas.MultipleAlleleCodeDictionary.ExternalInterface;
using LazyCache;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace Atlas.HlaMetadataDictionary.Test.IntegrationTests.Tests
{
    /// <summary>
    /// Fixture testing the base functionality of HlaSearchingMetadataService via an arbitrarily chosen base class.
    /// Fixture relies on a file-backed HlaMetadataDictionary - tests may break if underlying data is changed.
    /// </summary>
    [TestFixture]
    public class SearchRelatedMetadataServiceTests
    {
        private const Locus DefaultLocus = Locus.A;
        private const string CacheKey = "NmdpCodeLookup_A";

        private ISearchRelatedMetadataService<IHlaMatchingMetadata> metadataService;
        private IMacDictionary macDictionary;
        private IAppCache appCache;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestStackTraceHelper.CatchAndRethrowWithStackTraceInExceptionMessage(() =>
            {
                // The metadataService is the object under test here.
                // We kinda want an abstract "IHlaSearchMetadataService<T>". But that's not an option, so we pick one
                // of the concrete implementations (the Matching implementation, as it happens), and apply our tests to that.
                // We could have used *any* of the concrete implementations of HlaSearchingMetadataServiceBase<THlaMetadata>.
                metadataService = DependencyInjection.DependencyInjection.Provider.GetService<IHlaMatchingMetadataService>();
                macDictionary = DependencyInjection.DependencyInjection.Provider.GetService<IMacDictionary>();
                appCache = DependencyInjection.DependencyInjection.Provider.GetService<IPersistentCacheProvider>().Cache;
            });
        }

        [SetUp]
        public void SetUp()
        {
            macDictionary
                .GetHlaFromMac(Arg.Any<string>())
                .Returns(new List<string>());
        }

        [TearDown]
        public void TearDown()
        {
            // clear MAC allele mappings between tests
            appCache.Remove(CacheKey);
        }

        [Test]
        public void GetHlaMetadata_WhenInvalidHlaTyping_ThrowsException()
        {
            const string hlaName = "XYZ:123:INVALID";

            Assert.ThrowsAsync<HlaMetadataDictionaryException>(
                async () => await metadataService.GetHlaMetadata(DefaultLocus, hlaName, null));
        }

        [Test]
        public void GetHlaMetadata_WhenNmdpCodeContainsAlleleNotInHlaMetadataDictionary_ThrowsException()
        {
            const string missingAllele = "9999:9999";

            // MAC value does not matter, but does need to conform to the expected pattern
            const string macWithFirstField = "99:CODE";
            macDictionary
                .GetHlaFromMac(macWithFirstField)
                .Returns(new List<string> { missingAllele });

            Assert.ThrowsAsync<HlaMetadataDictionaryException>(async () => 
                await metadataService.GetHlaMetadata(DefaultLocus, macWithFirstField, null));
        }

        [Test]
        public void GetHlaMetadata_WhenAlleleStringOfNamesContainsAlleleNotInHlaMetadataDictionary_ThrowsException()
        {
            const string existingAllele = "01:133";
            const string missingAllele = "9999:9999";
            const string alleleString = existingAllele + "/" + missingAllele;

            Assert.ThrowsAsync<HlaMetadataDictionaryException>(async () =>
                await metadataService.GetHlaMetadata(DefaultLocus, alleleString, null));
        }

        [Test]
        public void GetHlaMetadata_WhenAlleleStringOfSubtypesContainsAlleleNotInHlaMetadataDictionary_ThrowsException()
        {
            const string alleleString = "01:133/9999";

            Assert.ThrowsAsync<HlaMetadataDictionaryException>(async () =>
                await metadataService.GetHlaMetadata(DefaultLocus, alleleString, null));
        }
    }
}
