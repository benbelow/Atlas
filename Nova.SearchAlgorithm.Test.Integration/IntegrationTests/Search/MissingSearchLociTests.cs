﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Nova.SearchAlgorithm.ApplicationInsights.SearchRequests;
using Nova.SearchAlgorithm.Clients.AzureStorage;
using Nova.SearchAlgorithm.Clients.ServiceBus;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Services.ConfigurationProviders;
using Nova.SearchAlgorithm.Services.Search;
using Nova.SearchAlgorithm.Test.Integration.TestData;
using Nova.SearchAlgorithm.Test.Integration.TestHelpers.Builders;
using Nova.Utils.ApplicationInsights;
using NUnit.Framework;

namespace Nova.SearchAlgorithm.Test.Integration.IntegrationTests.Search
{
    [TestFixture]
    public class MissingSearchLociTests
    {
        private ISearchDispatcher searchDispatcher;
        private PhenotypeInfo<string> searchHla;

        [SetUp]
        public void SetUp()
        {
            var searchServiceBusClient = DependencyInjection.DependencyInjection.Provider.GetService<ISearchServiceBusClient>();
            var searchService = DependencyInjection.DependencyInjection.Provider.GetService<ISearchService>();
            var resultsBlobStorageClient = DependencyInjection.DependencyInjection.Provider.GetService<IResultsBlobStorageClient>();
            var logger = DependencyInjection.DependencyInjection.Provider.GetService<ILogger>();
            var searchRequestContext = new SearchRequestContext();
            var wmdaHlaVersionProvider = DependencyInjection.DependencyInjection.Provider.GetService<IWmdaHlaVersionProvider>();

            searchDispatcher = new SearchDispatcher(
                searchServiceBusClient,
                searchService,
                resultsBlobStorageClient,
                logger,
                searchRequestContext,
                wmdaHlaVersionProvider);

            searchHla = new TestHla.HeterozygousSet1().SixLocus_SingleExpressingAlleles;
        }

        #region TenOutOfTen

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithEmptyLocusA_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithEmptyLocusSearchHlaAt(Locus.A)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithNullHlaAtLocusA_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithNullLocusSearchHlasAt(Locus.A)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithEmptyLocusB_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithEmptyLocusSearchHlaAt(Locus.B)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithNullHlaAtLocusB_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithNullLocusSearchHlasAt(Locus.B)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithEmptyLocusDrb1_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithEmptyLocusSearchHlaAt(Locus.Drb1)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithNullHlaAtLocusDrb1_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithNullLocusSearchHlasAt(Locus.Drb1)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithEmptyLocusC_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithEmptyLocusSearchHlaAt(Locus.C)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithNullHlaAtLocusC_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithNullLocusSearchHlasAt(Locus.C)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithEmptyLocusDqb1_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithEmptyLocusSearchHlaAt(Locus.Dqb1)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithNullHlaAtLocusDqb1_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithNullLocusSearchHlasAt(Locus.Dqb1)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithEmptyLocusDpb1_DoesNotThrowValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithEmptyLocusSearchHlaAt(Locus.Dpb1)
                .Build();

            // DPB1 is an optional locus for scoring so can be empty
            Assert.DoesNotThrowAsync(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientWithNullHlaAtLocusDpb1_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .WithNullLocusSearchHlasAt(Locus.Dpb1)
                .Build();

            // Although DPB1 is an optional locus, its individual HLA strings cannot be null
            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_TenOutOfTen_PatientTypedAtAllSearchLoci_DoesNotThrowValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .TenOutOfTen()
                .Build();

            Assert.DoesNotThrowAsync(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        #endregion

        #region SixOutOfSix

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithEmptyLocusA_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithEmptyLocusSearchHlaAt(Locus.A)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithNullHlaAtLocusA_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithNullLocusSearchHlasAt(Locus.A)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithEmptyLocusB_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithEmptyLocusSearchHlaAt(Locus.B)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithNullHlaAtLocusB_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithNullLocusSearchHlasAt(Locus.B)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithEmptyLocusDrb1_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithEmptyLocusSearchHlaAt(Locus.Drb1)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithNullHlaAtLocusDrb1_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithNullLocusSearchHlasAt(Locus.Drb1)
                .Build();

            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithEmptyLocusC_DoesNotThrowValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithEmptyLocusSearchHlaAt(Locus.C)
                .Build();

            // C is an optional locus for scoring so can be empty
            Assert.DoesNotThrowAsync(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithNullHlaAtLocusC_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithNullLocusSearchHlasAt(Locus.C)
                .Build();

            // Although C is an optional locus, its individual HLA strings cannot be null
            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithEmptyLocusDqb1_DoesNotThrowValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithEmptyLocusSearchHlaAt(Locus.Dqb1)
                .Build();

            // DQB1 is an optional locus for scoring so can be empty
            Assert.DoesNotThrowAsync(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithNullHlaAtLocusDqb1_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithNullLocusSearchHlasAt(Locus.Dqb1)
                .Build();

            // Although DQB1 is an optional locus, its individual HLA strings cannot be null
            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithEmptyLocusDpb1_DoesNotThrowValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithEmptyLocusSearchHlaAt(Locus.Dpb1)
                .Build();

            // DPB1 is an optional locus for scoring so can be empty
            Assert.DoesNotThrowAsync(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientWithNullHlaAtLocusDpb1_ThrowsValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .WithNullLocusSearchHlasAt(Locus.Dpb1)
                .Build();

            // Although DPB1 is an optional locus, its individual HLA strings cannot be null
            Assert.ThrowsAsync<ValidationException>(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        [Test]
        public void DispatchSearch_SixOutOfSix_PatientTypedAtAllSearchLoci_DoesNotThrowValidationError()
        {
            var searchRequest = new SearchRequestFromHlasBuilder(searchHla)
                .SixOutOfSix()
                .Build();

            Assert.DoesNotThrowAsync(
                async () => await searchDispatcher.DispatchSearch(searchRequest));
        }

        #endregion
    }
}
