﻿using Nova.SearchAlgorithm.Client.Models;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Test.Validation.TestData.Models.Hla;

namespace Nova.SearchAlgorithm.Test.Validation.TestData.Models.PatientDataSelection
{
    public class MetaDonorSelectionCriteria
    {
        public DonorType MatchingDonorType { get; set; }
        public RegistryCode MatchingRegistry { get; set; }

        /// <summary>
        /// Determines how many fields the matching meta-donor's genotype should have at each position
        /// </summary>
        public PhenotypeInfo<TgsHlaTypingCategory> MatchingTgsTypingCategories { get; set; } = new PhenotypeInfo<bool>()
            .Map((l, p, noop) => l == Locus.Dpb1 ? TgsHlaTypingCategory.FourFieldAllele : TgsHlaTypingCategory.Arbitrary);

        /// <summary>
        /// The match level of the expected matching donor (if a match is expected)
        /// Necessary for meta-donor selection as we must ensure the genotype is valid for the specified match type
        /// e.g. for a p-group match, ensure that other alleles in the same p-group exist in our dataset
        /// </summary>
        public PhenotypeInfo<MatchLevel> MatchLevels { get; set; }

        /// <summary>
        /// Determines to what resolution the expected matched donor is typed
        /// Necessary for meta-donor selection to ensure the selected meta-donor contains donors at the expected resolution
        /// </summary>
        public PhenotypeInfo<HlaTypingResolution> TypingResolutions = new PhenotypeInfo<bool>()
            .Map((l, p, noop) => HlaTypingResolution.Arbitrary);
        
        /// <summary>
        /// Determines whether the expected meta-donor should be homozygous at each locus
        /// </summary>
        public LocusInfo<bool> IsHomozygous = new LocusInfo<bool>().Map((l, noop) => false);
    }
}