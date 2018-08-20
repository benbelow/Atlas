﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Test.Validation.TestData.Models;
using Nova.SearchAlgorithm.Test.Validation.TestData.Models.Hla;
using Nova.SearchAlgorithm.Test.Validation.TestData.Models.PatientDataSelection;
using Nova.SearchAlgorithm.Test.Validation.TestData.Repositories;
using Nova.SearchAlgorithm.Test.Validation.TestData.Services.PatientDataSelection;
using NSubstitute;
using NUnit.Framework;

namespace Nova.SearchAlgorithm.Test.Validation.ValidationFrameworkUnitTests.PatientDataSelection
{
    [TestFixture]
    public class PatientHlaSelectorTests
    {
        private IPatientHlaSelector patientHlaSelector;
        private IAlleleRepository alleleRepository;
        private List<AlleleTestData> alleles;
        private List<AlleleTestData> pGroupAlleles;
        private List<AlleleTestData> gGroupAlleles;

        [SetUp]
        public void SetUp()
        {
            alleles = new List<AlleleTestData>
            {
                new AlleleTestData {AlleleName = "01:01:01:a-1", PGroup = "p-1", GGroup = "g-1", NmdpCode = "nmdp-1", Serology = "s-1"},
                new AlleleTestData {AlleleName = "01:01:01:a-2", PGroup = "p-1", GGroup = "g-1", NmdpCode = "nmdp-1", Serology = "s-1"},
            };

            pGroupAlleles = new List<AlleleTestData>
            {
                new AlleleTestData
                {
                    AlleleName = "01:01:01:a-1 (p-groups-matching)",
                    PGroup = "p-1",
                    GGroup = "g-1",
                    NmdpCode = "nmdp-1",
                    Serology = "s-1"
                },
                new AlleleTestData
                {
                    AlleleName = "01:01:01:a-2 (p-groups-matching)",
                    PGroup = "p-1",
                    GGroup = "g-1",
                    NmdpCode = "nmdp-1",
                    Serology = "s-1"
                },
            };

            gGroupAlleles = new List<AlleleTestData>
            {
                new AlleleTestData
                {
                    AlleleName = "01:01:01:a-1 (g-groups-matching)",
                    PGroup = "p-1",
                    GGroup = "g-1",
                    NmdpCode = "nmdp-1",
                    Serology = "s-1"
                },
                new AlleleTestData
                {
                    AlleleName = "01:01:01:a-2 (g-groups-matching)",
                    PGroup = "p-1",
                    GGroup = "g-1",
                    NmdpCode = "nmdp-1",
                    Serology = "s-1"
                },
            };

            alleleRepository = Substitute.For<IAlleleRepository>();

            alleleRepository.AllTgsAlleles().Returns(new PhenotypeInfo<bool>().Map((l, p, noop) => alleles));
            alleleRepository.PatientAllelesForPGroupMatching().Returns(new LocusInfo<bool>().Map((l, noop) => pGroupAlleles.First()));
            alleleRepository.DonorAllelesForPGroupMatching().Returns(new LocusInfo<bool>().Map((l, noop) => pGroupAlleles));
            alleleRepository.AllelesForGGroupMatching().Returns(new PhenotypeInfo<bool>().Map((l, p, noop) => gGroupAlleles));

            patientHlaSelector = new PatientHlaSelector(alleleRepository);
        }

        [Test]
        public void GetPatientHla_ReturnsMatchingAlleleFromMetaDonorGenotype()
        {
            var criteria = new PatientHlaSelectionCriteria
            {
                HlaMatches = new PhenotypeInfo<bool>().Map((l, p, noop) => true),
                MatchLevels = new PhenotypeInfo<bool>().Map((l, p, noop) => MatchLevel.Allele),
            };

            var metaDonor = new MetaDonor
            {
                Genotype =
                {
                    Hla = new PhenotypeInfo<bool>().Map((locus, p, noop) => TgsAllele.FromTestDataAllele(alleles.First(), locus))
                }
            };

            var patientHla = patientHlaSelector.GetPatientHla(metaDonor, criteria);

            patientHla.A_1.Should().Be(metaDonor.Genotype.Hla.A_1.TgsTypedAllele);
        }

        [Test]
        public void GetPatientHla_ForPGroupMatchLevel_DoesNotSelectExactAlleleMatch()
        {
            var criteria = new PatientHlaSelectionCriteria
            {
                HlaMatches = new PhenotypeInfo<bool>().Map((l, p, noop) => true),
                MatchLevels = new PhenotypeInfo<int>().Map((l, p, noop) => MatchLevel.PGroup)
            };

            var metaDonor = new MetaDonor
            {
                Genotype =
                {
                    Hla = new PhenotypeInfo<bool>().Map((locus, p, noop) => TgsAllele.FromTestDataAllele(alleles.First(), locus))
                }
            };

            var patientHla = patientHlaSelector.GetPatientHla(metaDonor, criteria);

            patientHla.A_1.Should().NotBe(metaDonor.Genotype.Hla.A_1.TgsTypedAllele);
        }

        [Test]
        public void GetPatientHla_ForGGroupMatchLevel_DoesNotSelectExactAlleleMatch()
        {
            var criteria = new PatientHlaSelectionCriteria
            {
                HlaMatches = new PhenotypeInfo<bool>().Map((l, p, noop) => true),
                MatchLevels = new PhenotypeInfo<int>().Map((l, p, noop) => MatchLevel.GGroup)
            };

            var metaDonor = new MetaDonor
            {
                Genotype =
                {
                    Hla = new PhenotypeInfo<bool>().Map((locus, p, noop) => TgsAllele.FromTestDataAllele(alleles.First(), locus))
                }
            };

            var patientHla = patientHlaSelector.GetPatientHla(metaDonor, criteria);

            patientHla.A_1.Should().NotBe(metaDonor.Genotype.Hla.A_1.TgsTypedAllele);
        }

        [Test]
        public void GetPatientHla_ForUntypedLocus_ReturnsNull()
        {
            var criteria = new PatientHlaSelectionCriteria
            {
                PatientTypingResolutions = new PhenotypeInfo<bool>().Map((l, p, noop) => HlaTypingResolution.Untyped)
            };

            var metaDonor = new MetaDonor
            {
                Genotype =
                {
                    Hla = new PhenotypeInfo<bool>().Map((locus, p, noop) => TgsAllele.FromTestDataAllele(alleles.First(), locus))
                }
            };

            var patientHla = patientHlaSelector.GetPatientHla(metaDonor, criteria);

            patientHla.ToEnumerable().All(x => x == null).Should().BeTrue();
        }
    }
}