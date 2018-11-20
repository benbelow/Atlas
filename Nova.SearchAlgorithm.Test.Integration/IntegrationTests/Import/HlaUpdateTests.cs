﻿using Autofac;
using FluentAssertions;
using Nova.SearchAlgorithm.Client.Models;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Common.Repositories;
using Nova.SearchAlgorithm.Services.DonorImport;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nova.SearchAlgorithm.Client.Models.Donors;

namespace Nova.SearchAlgorithm.Test.Integration.IntegrationTests.Import
{
    public class HlaUpdateTests : IntegrationTestBase
    {
        private IDonorImportRepository importRepo;
        private IDonorInspectionRepository inspectionRepo;
        private IHlaUpdateService updateService;

        // We know the number of p-groups for a given hla string, from the in memory matching dictionary. If the underlying data changes, this may become incorrect.
        private readonly Tuple<string, int> hlaWithKnownPGroups1 = new Tuple<string, int>("01:XX", 213);

        [SetUp]
        public void ResolveSearchRepo()
        {
            importRepo = Container.Resolve<IDonorImportRepository>();
            inspectionRepo = Container.Resolve<IDonorInspectionRepository>();
            updateService = Container.Resolve<IHlaUpdateService>();
        }

        [Test]
        public async Task UpdateDonorHla_DoesNotUpdateStoredDonorInformation()
        {
            var inputDonor = DonorWithId(DonorIdGenerator.NextId());
            await importRepo.InsertBatchOfDonors(new List<InputDonor> {inputDonor});

            await updateService.UpdateDonorHla();

            var storedDonor = await inspectionRepo.GetDonor(inputDonor.DonorId);
            AssertStoredDonorInfoMatchesOriginalDonorInfo(storedDonor, inputDonor);
        }

        [Test]
        public async Task UpdateDonorHla_ForPatientHlaMatchingMultiplePGroups_InsertsMatchRowForEachPGroup()
        {
            var inputDonor = DonorWithId(DonorIdGenerator.NextId());
            await importRepo.InsertBatchOfDonors(new List<InputDonor> {inputDonor});

            await updateService.UpdateDonorHla();

            var pGroups = await inspectionRepo.GetPGroupsForDonors(new[] {inputDonor.DonorId});
            pGroups.First().PGroupNames.A.Position1.Count().Should().Be(hlaWithKnownPGroups1.Item2);
        }

        [Test]
        public async Task UpdateDonorHla_WhenUpdateHasBeenRunForADonor_DoesNotAddMorePGroups()
        {
            var inputDonor = DonorWithId(DonorIdGenerator.NextId());
            await importRepo.InsertBatchOfDonors(new List<InputDonor> {inputDonor});

            await updateService.UpdateDonorHla();
            var initialPGroupCountAtA1 =
                (await inspectionRepo.GetPGroupsForDonors(new[] {inputDonor.DonorId})).First().PGroupNames.A.Position1.Count();

            await updateService.UpdateDonorHla();
            var pGroups = await inspectionRepo.GetPGroupsForDonors(new[] {inputDonor.DonorId});
            pGroups.First().PGroupNames.A.Position1.Count().Should().Be(initialPGroupCountAtA1);
        }

        [Test]
        public async Task UpdateDonorHla_UpdatesHlaForNewDonorsSinceLastRun()
        {
            var inputDonor = DonorWithId(DonorIdGenerator.NextId());
            await importRepo.InsertBatchOfDonors(new List<InputDonor> {inputDonor});
            await updateService.UpdateDonorHla();

            var newDonor = DonorWithId(DonorIdGenerator.NextId());
            await importRepo.InsertBatchOfDonors(new List<InputDonor> {inputDonor});
            await updateService.UpdateDonorHla();

            var pGroups = await inspectionRepo.GetPGroupsForDonors(new[] {newDonor.DonorId});
            pGroups.Should().NotBeEmpty();
        }

        private static void AssertStoredDonorInfoMatchesOriginalDonorInfo(DonorResult donorActual, InputDonor donorExpected)
        {
            donorActual.DonorId.Should().Be(donorExpected.DonorId);
            donorActual.DonorType.Should().Be(donorExpected.DonorType);
            donorActual.RegistryCode.Should().Be(donorExpected.RegistryCode);
            donorActual.HlaNames.ShouldBeEquivalentTo(donorExpected.HlaNames);
        }

        private InputDonor DonorWithId(int id)
        {
            return new InputDonor
            {
                RegistryCode = RegistryCode.DKMS,
                DonorType = DonorType.Cord,
                DonorId = id,
                HlaNames = new PhenotypeInfo<string>
                {
                    A =
                    {
                        Position1 = hlaWithKnownPGroups1.Item1,
                        Position2 = "30:02:01:01",
                    },
                    B =
                    {
                        Position1 = "07:02",
                        Position2 = "08:01",
                    },
                    Drb1 =
                    {
                        Position1 = "01:11",
                        Position2 = "03:41",
                    }
                }
            };
        }
    }
}