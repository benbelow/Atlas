﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nova.SearchAlgorithm.Test.Validation.TestData.Exceptions;
using Nova.SearchAlgorithm.Test.Validation.TestData.Models;
using Nova.SearchAlgorithm.Test.Validation.TestData.Models.Hla;
using Nova.SearchAlgorithm.Test.Validation.TestData.Models.PatientDataSelection;
using Nova.SearchAlgorithm.Test.Validation.TestData.Repositories;
using NSubstitute.Routing.Handlers;

namespace Nova.SearchAlgorithm.Test.Validation.TestData.Services.PatientDataSelection
{
    public interface IMetaDonorSelector
    {
        /// <summary>
        /// Will return a meta-donor matching the criteria from the available test data.
        /// </summary>
        MetaDonor GetMetaDonor(MetaDonorSelectionCriteria criteria);
    }

    public class MetaDonorSelector : IMetaDonorSelector
    {
        private readonly IMetaDonorRepository metaDonorRepository;

        public MetaDonorSelector(IMetaDonorRepository metaDonorRepository)
        {
            this.metaDonorRepository = metaDonorRepository;
        }

        public MetaDonor GetMetaDonor(MetaDonorSelectionCriteria criteria)
        {
            var matchingMetaDonors = metaDonorRepository.AllMetaDonors()
                .Where(md => FulfilsDonorInfoCriteria(criteria, md) && FulfilsDonorHlaCriteria(criteria, md))
                .ToList();

            if (!matchingMetaDonors.Any())
            {
                throw new MetaDonorNotFoundException("No meta-donors found matching specified criteria.");
            }

            var newMetaDonors = matchingMetaDonors.Skip(criteria.MetaDonorsToSkip).ToList();

            if (!newMetaDonors.Any())
            {
                throw new MetaDonorNotFoundException(
                    $"No more meta-donors found matching specified criteria. Ignored {criteria.MetaDonorsToSkip} meta-donors. Is there enough test data?");
            }

            var metaDonor = newMetaDonors.First();
            return metaDonor;
        }

        private static bool FulfilsDonorInfoCriteria(MetaDonorSelectionCriteria criteria, MetaDonor metaDonor)
        {
            return FulfilsDonorTypeCriteria(criteria, metaDonor)
                   && FulfilsRegistryCriteria(criteria, metaDonor);
        }

        private static bool FulfilsDonorHlaCriteria(MetaDonorSelectionCriteria criteria, MetaDonor metaDonor)
        {
            return FulfilsHomozygousCriteria(criteria, metaDonor)
                   && FulfilsDatasetCriteria(criteria, metaDonor)
                   && FulfilsTypingResolutionCriteria(criteria, metaDonor);
        }

        private static bool FulfilsHomozygousCriteria(MetaDonorSelectionCriteria criteria, MetaDonor metaDonor)
        {
            var perLocusFulfilment = criteria.IsHomozygous.Map((locus, shouldBeHomozygous) =>
            {
                if (shouldBeHomozygous)
                {
                    return metaDonor.GenotypeCriteria.IsHomozygous.DataAtLocus(locus);
                }

                // If we don't explicitly need a homozygous donor, we don't mind whether the donor is homozygous or not
                return true;
            });

            return perLocusFulfilment.ToEnumerable().All(x => x);
        }

        private static bool FulfilsTypingResolutionCriteria(MetaDonorSelectionCriteria criteria, MetaDonor metaDonor)
        {
            return criteria.TypingResolutionSets.All(resolutionSet => metaDonor.HlaTypingResolutionSets.Any(resolutionSet.Equals));
        }

        private static bool FulfilsDatasetCriteria(MetaDonorSelectionCriteria criteria, MetaDonor metaDonor)
        {
            // Maps to a list of booleans - each one indicates whether the criteria are met at that locus/position
            return metaDonor.GenotypeCriteria.AlleleSources.Map((l, p, dataset) =>
            {
                var tgsTypingRequired = criteria.MatchingTgsTypingCategories.DataAtPosition(l, p);
                var matchLevelRequired = criteria.MatchLevels.DataAtPosition(l, p);

                switch (dataset)
                {
                    case Dataset.FourFieldTgsAlleles:
                        return matchLevelRequired == MatchLevel.Allele && tgsTypingRequired == TgsHlaTypingCategory.FourFieldAllele;
                    case Dataset.ThreeFieldTgsAlleles:
                        return matchLevelRequired == MatchLevel.Allele && tgsTypingRequired == TgsHlaTypingCategory.ThreeFieldAllele;
                    case Dataset.TwoFieldTgsAlleles:
                        return matchLevelRequired == MatchLevel.Allele && tgsTypingRequired == TgsHlaTypingCategory.TwoFieldAllele;
                    case Dataset.TgsAlleles:
                        return matchLevelRequired == MatchLevel.Allele && tgsTypingRequired == TgsHlaTypingCategory.Arbitrary;
                    case Dataset.PGroupMatchPossible:
                        return matchLevelRequired == MatchLevel.PGroup;
                    case Dataset.GGroupMatchPossible:
                        return matchLevelRequired == MatchLevel.GGroup;
                    case Dataset.FourFieldAllelesWithThreeFieldMatchPossible:
                        return matchLevelRequired == MatchLevel.FirstThreeFieldAllele && tgsTypingRequired == TgsHlaTypingCategory.FourFieldAllele;
                    case Dataset.ThreeFieldAllelesWithTwoFieldMatchPossible:
                        return matchLevelRequired == MatchLevel.FirstTwoFieldAllele && tgsTypingRequired == TgsHlaTypingCategory.ThreeFieldAllele;
                    case Dataset.AlleleStringOfSubtypesPossible:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dataset), dataset, null);
                }
            }).ToEnumerable().All(x => x);
        }

        private static bool FulfilsRegistryCriteria(MetaDonorSelectionCriteria criteria, MetaDonor metaDonor)
        {
            return criteria.MatchingRegistry == metaDonor.Registry;
        }

        private static bool FulfilsDonorTypeCriteria(MetaDonorSelectionCriteria criteria, MetaDonor metaDonor)
        {
            return criteria.MatchingDonorType == metaDonor.DonorType;
        }
    }
}