﻿using System.Linq;
using System.Threading.Tasks;
using Atlas.Common.Test.SharedTestHelpers;
using Atlas.DonorImport.Services;
using Atlas.DonorImport.Test.Integration.TestHelpers;
using Atlas.DonorImport.Test.TestHelpers.Builders;
using Atlas.DonorImport.Test.TestHelpers.Builders.ExternalModels;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Atlas.DonorImport.Test.Integration.IntegrationTests.Import
{
    public class TransientConnectionFailureTests
    {
        /// <summary>
        /// These tests exist to test when there has been a transient connection failure.
        /// When this happens we want to try and re-import the file successfully.
        /// </summary>
        [TestFixture]
        public class TransactionScopeErrorTests
        {
            private IDonorFileImporter donorFileImporter;
            private IDonorImportFileHistoryService donorImportFileHistoryService;
            private IDonorInspectionRepository donorRepository;

            [OneTimeSetUp]
            public void OneTimeSetUp()
            {
                TestStackTraceHelper.CatchAndRethrowWithStackTraceInExceptionMessage(() =>
                {
                    donorFileImporter = DependencyInjection.DependencyInjection.Provider.GetService<IDonorFileImporter>();
                    donorImportFileHistoryService = DependencyInjection.DependencyInjection.Provider.GetService<IDonorImportFileHistoryService>();
                    donorRepository = DependencyInjection.DependencyInjection.Provider.GetService<IDonorInspectionRepository>();
                });
            }

            [OneTimeTearDown]
            public void OneTimeTearDown()
            {
                TestStackTraceHelper.CatchAndRethrowWithStackTraceInExceptionMessage(() =>
                {
                    // Ensure any mocks set up for this test do not stick around.
                    DependencyInjection.DependencyInjection.BackingProvider = DependencyInjection.ServiceConfiguration.CreateProvider();
                    DatabaseManager.ClearDatabases();
                });
            }

            [Test]
            public async Task ImportDonors_WhenRetryingImportOfFile_DoesNotThrow()
            {
                // Set up initial failed import with single successful batch 
                var donorFile = DonorImportFileBuilder.NewWithoutContents.Build();
                donorFile.Contents = DonorImportFileContentsBuilder.New.WithDonorCount(10_000).Build().ToStream();
                await donorFileImporter.ImportDonorFile(donorFile);
                await donorImportFileHistoryService.RegisterUnexpectedDonorImportError(donorFile);

                donorFile.Contents = DonorImportFileContentsBuilder.New.WithDonorCount(10_001).Build().ToStream();
                await donorFileImporter.ImportDonorFile(donorFile);

                donorRepository.StreamAllDonors().Count().Should().Be(10_001);
            }
        }
    }
}
