﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Atlas.Common.ApplicationInsights;
using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.HlaMetadataDictionary.Exceptions;
using Atlas.HlaMetadataDictionary.ExternalInterface;
using Atlas.HlaMetadataDictionary.Models.Lookups.MatchingLookup;
using Atlas.MatchingAlgorithm.Data.Models.DonorInfo;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders;
using Atlas.MatchingAlgorithm.Services.Donors;
using Atlas.MatchingAlgorithm.Test.TestHelpers.Builders;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Atlas.MatchingAlgorithm.Test.Services.Donors
{
    [TestFixture]
    public class DonorHlaExpanderTests
    {
        private IDonorHlaExpander donorHlaExpander;
        private IHlaMetadataDictionary hlaMetadataDictionary;
        private ILogger logger;

        [SetUp]
        public void SetUp()
        {
            hlaMetadataDictionary = Substitute.For<IHlaMetadataDictionary>();
            hlaMetadataDictionary.GetLocusHlaMatchingLookupResults(default, default).ReturnsForAnyArgs(call =>
            {
                var input = call.Arg<LocusInfo<string>>();
                return Task.FromResult(new LocusInfo<IHlaMatchingLookupResult>(default, default));
            });

            logger = Substitute.For<ILogger>();
            donorHlaExpander = new DonorHlaExpander(hlaMetadataDictionary, logger);
        }

        [Test]
        public async Task ExpandDonorHlaBatchAsync_LooksUpHlaData()
        {
            await donorHlaExpander.ExpandDonorHlaBatchAsync(new List<DonorInfo>
                {
                    new DonorInfo
                    {
                        HlaNames = new PhenotypeInfo<string>("hla")
                    }
                },
                "event-name");


            await hlaMetadataDictionary.Received().GetLocusHlaMatchingLookupResults(Arg.Any<Locus>(), Arg.Any<LocusInfo<string>>());
        }

        [Test]
        public async Task ExpandDonorHlaBatchAsync_HlaNomenclatureVersionProvidedToFactory_ExpandsDonorHlaWithAppropriateDictionary()
        {
            var dictionaryBuilder = new HlaMetadataDictionaryBuilder().Returning(hlaMetadataDictionary);

            var constructedDonorExpander = new DonorHlaExpanderFactory(dictionaryBuilder, Substitute.For<IActiveHlaNomenclatureVersionAccessor>(), Substitute.For<ILogger>()).BuildForSpecifiedHlaNomenclatureVersion(null);
            
            await constructedDonorExpander.ExpandDonorHlaBatchAsync(new List<DonorInfo>
                {
                    new DonorInfo
                    {
                        HlaNames = new PhenotypeInfo<string>("hla")
                    }
                },
                "event-name");

            await hlaMetadataDictionary.Received().GetLocusHlaMatchingLookupResults(
                Arg.Any<Locus>(),
                Arg.Any<LocusInfo<string>>());
        }

        [Test]
        public async Task ExpandDonorHlaBatchAsync_ExpansionSucceeded_ReturnsExpectedDonor()
        {
            const int donorId = 123;

            hlaMetadataDictionary
                .GetLocusHlaMatchingLookupResults(default, default)
                .ReturnsForAnyArgs(new LocusInfo<IHlaMatchingLookupResult>(null));
                
            var result = await donorHlaExpander.ExpandDonorHlaBatchAsync(new List<DonorInfo>
            {
                new DonorInfo
                {
                    DonorId = donorId,
                    HlaNames = new PhenotypeInfo<string>("hla")
                }
            },
                "event-name");

            result.ProcessingResults.Should().OnlyContain(d => d.DonorId == donorId);
        }

        [Test]
        public async Task ExpandDonorHlaBatchAsync_AnticipatedExpansionFailure_ReturnsFailedDonor()
        {
            const int donorId = 123;

            hlaMetadataDictionary
                .GetLocusHlaMatchingLookupResults(default, default)
                .ThrowsForAnyArgs(new HlaMetadataDictionaryException(Locus.A, "hla", "error"));
                
            var result = await donorHlaExpander.ExpandDonorHlaBatchAsync(new List<DonorInfo>
            {
                new DonorInfo
                {
                    DonorId = donorId,
                    HlaNames = new PhenotypeInfo<string>("hla")
                }
            },
                "event-name");

            result.FailedDonors.Should().OnlyContain(d => d.DonorId == donorId.ToString());
        }

        [Test]
        public void ExpandDonorHlaBatchAsync_AnticipatedExpansionFailure_DoesNotThrowException()
        {
            hlaMetadataDictionary
                .GetLocusHlaMatchingLookupResults(default, default)
                .ThrowsForAnyArgs(new HlaMetadataDictionaryException(Locus.A, "hla", "error"));

            Assert.DoesNotThrowAsync(async () =>
            {
                await donorHlaExpander.ExpandDonorHlaBatchAsync(new List<DonorInfo>
                {
                    new DonorInfo
                    {
                        HlaNames = new PhenotypeInfo<string>("hla")
                    }
                },
                    "event-name");
            });
        }

        [Test]
        public void ExpandDonorHlaBatchAsync_UnanticipatedExpansionFailure_ThrowsException()
        {
            hlaMetadataDictionary
                .GetLocusHlaMatchingLookupResults(Locus.A, Arg.Any<LocusInfo<string>>())
                .Throws(new Exception("error"));

            Assert.ThrowsAsync<Exception>(async () =>
                {
                    await donorHlaExpander.ExpandDonorHlaBatchAsync(new List<DonorInfo>
                    {
                        new DonorInfo
                        {
                            HlaNames = new PhenotypeInfo<string>("hla")
                        }
                    },
                        "event-name");
                }
            );
        }
    }
}
