﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Nova.SearchAlgorithm.Client.Models;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Test.Validation.TestData.Exceptions;
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
    public class MetaDonorSelectorTests
    {
        private IMetaDonorRepository metaDonorRepository;
        private IMetaDonorSelector metaDonorSelector;

        private readonly PhenotypeInfo<TgsHlaTypingCategory> defaultTgsTypingCategories = new PhenotypeInfo<TgsHlaTypingCategory>();

        [SetUp]
        public void SetUp()
        {
            metaDonorRepository = Substitute.For<IMetaDonorRepository>();

            metaDonorSelector = new MetaDonorSelector(metaDonorRepository);
        }

        [Test]
        public void GetMetaDonor_WhenNoMetaDonorsExistOfSpecifiedType_ThrowsException()
        {
            const DonorType donorType = DonorType.Adult;
            const DonorType anotherDonorType = DonorType.Cord;

            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria {TgsHlaCategories = defaultTgsTypingCategories},
                    DonorType = anotherDonorType
                }
            };
            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingDonorType = donorType,
                MatchingTgsTypingCategories = defaultTgsTypingCategories,
            };

            Assert.Throws<MetaDonorNotFoundException>(() => metaDonorSelector.GetMetaDonor(criteria));
        }

        [Test]
        public void GetMetaDonor_WhenNoMetaDonorsExistAtSpecifiedRegistry_ThrowsException()
        {
            const RegistryCode registry = RegistryCode.AN;
            const RegistryCode anotherRegistry = RegistryCode.DKMS;

            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria {TgsHlaCategories = defaultTgsTypingCategories},
                    Registry = anotherRegistry
                }
            };
            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingRegistry = registry,
                MatchingTgsTypingCategories = defaultTgsTypingCategories,
            };

            Assert.Throws<MetaDonorNotFoundException>(() => metaDonorSelector.GetMetaDonor(criteria));
        }
        
        [Test]
        public void GetMetaDonor_WhenNoMetaDonorsExistAtSpecifiedTgsResolution_ThrowsException()
        {
            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria {TgsHlaCategories = defaultTgsTypingCategories},
                }
            };
            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingTgsTypingCategories = new PhenotypeInfo<TgsHlaTypingCategory>(TgsHlaTypingCategory.ThreeFieldAllele)
            };

            Assert.Throws<MetaDonorNotFoundException>(() => metaDonorSelector.GetMetaDonor(criteria));
        }
        
        /// <summary>
        /// We want to guarantee that 'arbitrary' typing resolution does not match other, more specific resolutions
        ///
        /// i.e. one interpretation of a patient selector specifying 'arbitrary' data would be that it could sucessfully match any other resolution
        /// This test makes it explicit to maintainers that 'arbitrary' resolution in the patient selection should only match
        /// meta-donors that also specifiy 'arbitrary' resolution
        /// </summary>
        [Test]
        public void GetMetaDonor_WhenNoMetaDonorsExistAtSpecifiedTgsResolution_AndCriteriaAllowsArbitraryResolution_ThrowsException()
        {
            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria {TgsHlaCategories = defaultTgsTypingCategories},
                }
            };
            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingTgsTypingCategories = new PhenotypeInfo<TgsHlaTypingCategory>(TgsHlaTypingCategory.Arbitrary),
                MatchLevels = new PhenotypeInfo<MatchLevel>(),
            };

            Assert.Throws<MetaDonorNotFoundException>(() => metaDonorSelector.GetMetaDonor(criteria));
        }

        [Test]
        public void GetMetaDonor_WhenPGroupLevelMatchRequiredAndNoMetaDonorHasPGroupMatchPossible_ThrowsException()
        {
            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria
                    {
                        TgsHlaCategories = defaultTgsTypingCategories,
                        PGroupMatchPossible = new PhenotypeInfo<bool>(false),
                    },
                }
            };
            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingTgsTypingCategories = defaultTgsTypingCategories,
                MatchLevels = new PhenotypeInfo<MatchLevel>(MatchLevel.PGroup),
            };

            Assert.Throws<MetaDonorNotFoundException>(() => metaDonorSelector.GetMetaDonor(criteria));
        }
        
        [Test]
        public void GetMetaDonor_WhenGGroupLevelMatchRequiredAndNoMetaDonorHasGGroupMatchPossible_ThrowsException()
        {
            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria
                    {
                        TgsHlaCategories = defaultTgsTypingCategories,
                        GGroupMatchPossible = new PhenotypeInfo<bool>(false)
                    },
                }
            };
            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingTgsTypingCategories = defaultTgsTypingCategories,
                MatchLevels = new PhenotypeInfo<bool>().Map((l, p, noop) => MatchLevel.GGroup),
            };

            Assert.Throws<MetaDonorNotFoundException>(() => metaDonorSelector.GetMetaDonor(criteria));
        }
        
        [Test]
        public void GetMetaDonor_WhenNoMetaDonorsContainDonorAtExpectedResolution_ThrowsException()
        {
            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria { TgsHlaCategories = defaultTgsTypingCategories },
                }
            };
            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingTgsTypingCategories = defaultTgsTypingCategories,
                TypingResolutions = new PhenotypeInfo<HlaTypingResolution>(HlaTypingResolution.NmdpCode),
                MatchLevels = new PhenotypeInfo<MatchLevel>(),
            };

            Assert.Throws<MetaDonorNotFoundException>(() => metaDonorSelector.GetMetaDonor(criteria));
        }

        [Test]
        public void GetMetaDonor_ReturnsMetaDonorAtMatchingRegistry()
        {
            const RegistryCode registryCode = RegistryCode.AN;
            const RegistryCode anotherRegistryCode = RegistryCode.DKMS;

            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria {TgsHlaCategories = defaultTgsTypingCategories},
                    Registry = anotherRegistryCode,
                },
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria {TgsHlaCategories = defaultTgsTypingCategories},
                    Registry = registryCode,
                }
            };

            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingRegistry = registryCode,
                MatchingTgsTypingCategories = defaultTgsTypingCategories,
                MatchLevels = new PhenotypeInfo<MatchLevel>(),
            };

            var metaDonor = metaDonorSelector.GetMetaDonor(criteria);

            metaDonor.Registry.Should().Be(registryCode);
        }

        [Test]
        public void GetMetaDonor_ReturnsMetaDonorOfMatchingType()
        {
            const DonorType donorType = DonorType.Adult;
            const DonorType anotherDonorType = DonorType.Cord;

            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria {TgsHlaCategories = defaultTgsTypingCategories},
                    DonorType = anotherDonorType,
                },
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria {TgsHlaCategories = defaultTgsTypingCategories},
                    DonorType = donorType,
                }
            };

            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingDonorType = donorType,
                MatchingTgsTypingCategories = defaultTgsTypingCategories,
                MatchLevels = new PhenotypeInfo<MatchLevel>(),
            };

            var metaDonor = metaDonorSelector.GetMetaDonor(criteria);

            metaDonor.DonorType.Should().Be(donorType);
        }
        
        [Test]
        public void GetMetaDonor_WhenNoLocusShouldBeHomozygous_AndMetaDonorIsHomozygous_ReturnsMetaDonor()
        {
            const DonorType donorType = DonorType.Adult;

            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria
                    {
                        TgsHlaCategories = defaultTgsTypingCategories,
                        IsHomozygous = new LocusInfo<bool>().Map((l, noop) => true)
                    },
                    DonorType = donorType,
                }
            };

            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingDonorType = donorType,
                MatchingTgsTypingCategories = defaultTgsTypingCategories,
                MatchLevels = new PhenotypeInfo<MatchLevel>(),
                IsHomozygous = new LocusInfo<bool>().Map((l, noop) => false)
            };

            var metaDonor = metaDonorSelector.GetMetaDonor(criteria);

            metaDonor.Should().NotBeNull();
        }      
        
        [Test]
        public void GetMetaDonor_WhenLocusShouldBeHomozygous_AndMetaDonorIsHomozygous_ReturnsMetaDonor()
        {
            const DonorType donorType = DonorType.Adult;

            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria
                    {
                        TgsHlaCategories = defaultTgsTypingCategories,
                        IsHomozygous = new LocusInfo<bool>().Map((l, noop) => true)
                    },
                    DonorType = donorType,
                }
            };

            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingDonorType = donorType,
                MatchingTgsTypingCategories = defaultTgsTypingCategories,
                MatchLevels = new PhenotypeInfo<MatchLevel>(),
                IsHomozygous = new LocusInfo<bool>().Map((l, noop) => true)
            };

            var metaDonor = metaDonorSelector.GetMetaDonor(criteria);

            metaDonor.Should().NotBeNull();
        }
        
        [Test]
        public void GetMetaDonor_WhenLocusShouldBeHomozygous_AndMetaDonorIsNotHomozygous_ThrowsException()
        {
            const DonorType donorType = DonorType.Adult;

            var metaDonors = new List<MetaDonor>
            {
                new MetaDonor
                {
                    GenotypeCriteria = new GenotypeCriteria
                    {
                        TgsHlaCategories = defaultTgsTypingCategories,
                        IsHomozygous = new LocusInfo<bool>().Map((l, noop) => false)
                    },
                    DonorType = donorType,
                }
            };

            metaDonorRepository.AllMetaDonors().Returns(metaDonors);

            var criteria = new MetaDonorSelectionCriteria
            {
                MatchingDonorType = donorType,
                MatchingTgsTypingCategories = defaultTgsTypingCategories,
                MatchLevels = new PhenotypeInfo<MatchLevel>(),
                IsHomozygous = new LocusInfo<bool>().Map((l, noop) => true)
            };

            Assert.Throws<MetaDonorNotFoundException>(() => metaDonorSelector.GetMetaDonor(criteria));
        }
    }
}