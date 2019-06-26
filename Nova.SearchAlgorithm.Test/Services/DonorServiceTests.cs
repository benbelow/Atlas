using AutoMapper;
using Nova.SearchAlgorithm.Client.Models.Donors;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Common.Repositories;
using Nova.SearchAlgorithm.Config;
using Nova.SearchAlgorithm.Services;
using Nova.Utils.Http.Exceptions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nova.SearchAlgorithm.Test.Services
{
    [TestFixture]
    public class DonorServiceTests
    {
        private IDonorService donorService;
        private IDonorImportRepository importRepository;
        private IDonorInspectionRepository inspectionRepository;
        private IExpandHlaPhenotypeService expandHlaPhenotypeService;
        private IMapper mapper;

        [SetUp]
        public void SetUp()
        {
            importRepository = Substitute.For<IDonorImportRepository>();
            inspectionRepository = Substitute.For<IDonorInspectionRepository>();
            expandHlaPhenotypeService = Substitute.For<IExpandHlaPhenotypeService>();
            mapper = AutomapperConfig.CreateMapper();
            donorService = new SearchAlgorithm.Services.DonorService(importRepository, expandHlaPhenotypeService, inspectionRepository, mapper);
        }

        [Test]
        public void CreateDonor_WhenDonorExists_ThrowsException()
        {
            inspectionRepository.GetDonors(Arg.Any<IEnumerable<int>>()).Returns(new[] { new DonorResult() });

            Assert.ThrowsAsync<NovaHttpException>(() => donorService.CreateDonor(new InputDonor()));
        }

        [Test]
        public async Task CreateDonor_WhenDonorDoesNotExist_CreatesDonor()
        {
            inspectionRepository.GetDonors(Arg.Any<IEnumerable<int>>()).Returns(new List<DonorResult>(),
                new List<DonorResult> { new DonorResult() });

            var inputDonor = new InputDonor
            {
                HlaNames = new PhenotypeInfo<string>("hla")
            };
            await donorService.CreateDonor(inputDonor);

            await importRepository.Received()
                .InsertBatchOfDonorsWithExpandedHla(Arg.Any<IEnumerable<InputDonorWithExpandedHla>>());
        }

        [Test]
        public void UpdateDonor_WhenDonorDoesNotExist_ThrowsException()
        {
            Assert.ThrowsAsync<NovaNotFoundException>(() => donorService.UpdateDonor(new InputDonor()));
        }

        [Test]
        public async Task UpdateDonor_WhenDonorExists_UpdatesDonor()
        {
            var inputDonor = new InputDonor
            {
                HlaNames = new PhenotypeInfo<string>("hla")
            };
            inspectionRepository.GetDonors(Arg.Any<IEnumerable<int>>()).Returns(new[] { new DonorResult() });

            await donorService.UpdateDonor(inputDonor);

            await importRepository.Received()
                .UpdateBatchOfDonorsWithExpandedHla(Arg.Any<IEnumerable<InputDonorWithExpandedHla>>());
        }

        [Test]
        public void DeleteDonor_WhenDonorDoesNotExist_ThrowsException()
        {
            const int donorId = 123;
            Assert.ThrowsAsync<NovaNotFoundException>(() => donorService.DeleteDonor(donorId));
        }

        [Test]
        public async Task DeleteDonor_WhenDonorExists_DeletesDonor()
        {
            const int donorId = 123;

            inspectionRepository.GetDonors(Arg.Any<IEnumerable<int>>()).Returns(new[] { new DonorResult() });

            await donorService.DeleteDonor(donorId);

            await importRepository.Received().DeleteDonorAndItsExpandedHla(donorId);
        }
    }
}