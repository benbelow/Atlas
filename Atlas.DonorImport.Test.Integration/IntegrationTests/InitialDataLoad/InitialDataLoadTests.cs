using System.Reflection;
using System.Threading.Tasks;
using Atlas.Common.Test.SharedTestHelpers;
using Atlas.DonorImport.ExternalInterface.Models;
using Atlas.DonorImport.Services;
using Atlas.DonorImport.Test.Integration.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Atlas.DonorImport.Test.Integration.IntegrationTests.InitialDataLoad
{
    [TestFixture]
    public class InitialDataLoadTests
    {
        private IDonorInspectionRepository donorRepository;

        private IDonorFileImporter donorFileImporter;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await TestStackTraceHelper.CatchAndRethrowWithStackTraceInExceptionMessage_Async(async () =>
            {
                donorRepository = DependencyInjection.DependencyInjection.Provider.GetService<IDonorInspectionRepository>();
                donorFileImporter = DependencyInjection.DependencyInjection.Provider.GetService<IDonorFileImporter>();
                // Run operation under test once for this fixture, to (a) improve performance (b) remove the need to clean up duplicate ids between runs
                await ImportFile();
            });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestStackTraceHelper.CatchAndRethrowWithStackTraceInExceptionMessage(DatabaseManager.ClearDatabases);
        }

        [Test]
        public async Task ImportDonors_ForAllAdditions_AddsAllDonorsToDatabase()
        {
            var importedDonors = await donorRepository.DonorCount();

            importedDonors.Should().Be(1000);
        }

        /// <summary>
        /// Snapshot test of an arbitrary donor, to test mapping and plumbing working as expected
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ImportDonors_NewDonor_IsAddedCorrectly()
        {
            const string selectedDonorId = "1";
            const string expectedDonorHash = "MHH/OTtSeI96PClybhTF0g==";

            var actualDonor = await donorRepository.GetDonor(selectedDonorId);

            actualDonor.Hash.Should().Be(expectedDonorHash);
            actualDonor.CalculateHash().Should().Be(expectedDonorHash);
        }

        private async Task ImportFile()
        {
            const string fileName = "1000-initial-donors.json";
            var donorTestFilePath = $"Atlas.DonorImport.Test.Integration.IntegrationTests.InitialDataLoad.{fileName}";
            await using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(donorTestFilePath))
            {
                await donorFileImporter.ImportDonorFile(new DonorImportFile{ Contents = stream, FileName = fileName});
            }
        }
    }
}