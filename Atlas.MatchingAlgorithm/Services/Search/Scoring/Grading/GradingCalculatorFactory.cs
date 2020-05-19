﻿using System;
using Atlas.HlaMetadataDictionary.Models.Lookups.ScoringLookup;
using Atlas.HlaMetadataDictionary.Services;
using Atlas.MatchingAlgorithm.Services.Search.Scoring.Grading.GradingCalculators;

namespace Atlas.MatchingAlgorithm.Services.Search.Scoring.Grading
{
    public static class GradingCalculatorFactory
    {
        public static IGradingCalculator GetGradingCalculator(
            IPermissiveMismatchCalculator permissiveMismatchCalculator,
            IHlaScoringInfo patientInfo,
            IHlaScoringInfo donorInfo
        )
        {
            // order of checks is critical to which calculator is returned

            if (patientInfo is SerologyScoringInfo || donorInfo is SerologyScoringInfo)
            {
                return new SerologyGradingCalculator();
            }
            // If comparing a single null allele to a non-single allele,
            // as we are ignoring null alleles within allele strings, we can consider this a null vs. expressing comparison
            if (IsSingleNullAllele(patientInfo) ^ IsSingleNullAllele(donorInfo))
            {
                return new ExpressingVsNullAlleleGradingCalculator();
            }
            if (patientInfo is ConsolidatedMolecularScoringInfo || donorInfo is ConsolidatedMolecularScoringInfo)
            {
                return new ConsolidatedMolecularGradingCalculator(permissiveMismatchCalculator);
            }
            if (patientInfo is MultipleAlleleScoringInfo || donorInfo is MultipleAlleleScoringInfo)
            {
                return new MultipleAlleleGradingCalculator(permissiveMismatchCalculator);
            }
            if (patientInfo is SingleAlleleScoringInfo pInfo && donorInfo is SingleAlleleScoringInfo dInfo)
            {
                return GetSingleAlleleGradingCalculator(permissiveMismatchCalculator, pInfo, dInfo);
            }

            throw new ArgumentException("No calculator available for provided patient and donor scoring infos.");
        }

        private static IGradingCalculator GetSingleAlleleGradingCalculator(
            IPermissiveMismatchCalculator permissiveMismatchCalculator,
            SingleAlleleScoringInfo patientInfo,
            SingleAlleleScoringInfo donorInfo)
        {
            var patientAlleleIsNull = ExpressionSuffixParser.IsAlleleNull(patientInfo.AlleleName);
            var donorAlleleIsNull = ExpressionSuffixParser.IsAlleleNull(donorInfo.AlleleName);

            if (!patientAlleleIsNull && !donorAlleleIsNull)
            {
                return new ExpressingAlleleGradingCalculator(permissiveMismatchCalculator);
            }

            if (patientAlleleIsNull && donorAlleleIsNull)
            {
                return new NullAlleleGradingCalculator();
            }

            return new ExpressingVsNullAlleleGradingCalculator();
        }

        private static bool IsSingleNullAllele(IHlaScoringInfo scoringInfo)
        {
            return scoringInfo is SingleAlleleScoringInfo info && ExpressionSuffixParser.IsAlleleNull(info.AlleleName);
        }
    }
}