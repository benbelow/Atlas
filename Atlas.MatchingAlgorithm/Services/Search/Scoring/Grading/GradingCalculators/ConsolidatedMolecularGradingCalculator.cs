﻿using Atlas.MatchingAlgorithm.Client.Models.SearchResults;
using Atlas.HlaMetadataDictionary.Models.Lookups;
using Atlas.HlaMetadataDictionary.Models.Lookups.ScoringLookup;
using System.Linq;

namespace Atlas.MatchingAlgorithm.Services.Scoring.Grading
{
    /// <summary>
    /// To be used when both typings are molecular, and at least
    /// one has consolidated molecular scoring info.
    /// </summary>
    public interface IConsolidatedMolecularGradingCalculator : IGradingCalculator
    {
    }

    public class ConsolidatedMolecularGradingCalculator :
        GradingCalculatorBase,
        IConsolidatedMolecularGradingCalculator
    {
        private readonly IPermissiveMismatchCalculator permissiveMismatchCalculator;

        public ConsolidatedMolecularGradingCalculator(IPermissiveMismatchCalculator permissiveMismatchCalculator)
        {
            this.permissiveMismatchCalculator = permissiveMismatchCalculator;
        }

        protected override bool ScoringInfosAreOfPermittedTypes(
            IHlaScoringInfo patientInfo,
            IHlaScoringInfo donorInfo)
        {
            return (patientInfo is ConsolidatedMolecularScoringInfo ||
                    donorInfo is ConsolidatedMolecularScoringInfo) &&
                   !(patientInfo is SerologyScoringInfo) &&
                   !(donorInfo is SerologyScoringInfo);
        }

        protected override MatchGrade GetMatchGrade(
            IHlaScoringLookupResult patientLookupResult, 
            IHlaScoringLookupResult donorLookupResult)
        {
            var patientInfo = patientLookupResult.HlaScoringInfo;
            var donorInfo = donorLookupResult.HlaScoringInfo;

            // Order of the following checks is critical to the grade outcome

            if (patientInfo.Equals(donorInfo))
            {
                return MatchGrade.GGroup;
            }
            else if (IsGGroupMatch(patientInfo, donorInfo))
            {
                return MatchGrade.GGroup;
            }
            else if (IsPGroupMatch(patientInfo, donorInfo))
            {
                return MatchGrade.PGroup;
            }
            else if (IsPermissiveMismatch(patientLookupResult, donorLookupResult))
            {
                return MatchGrade.PermissiveMismatch;
            }

            return MatchGrade.Mismatch;
        }

        /// <summary>
        /// Do both typings have intersecting G Groups?
        /// </summary>
        private static bool IsGGroupMatch(
            IHlaScoringInfo patientInfo,
            IHlaScoringInfo donorInfo)
        {
            return patientInfo.MatchingGGroups
                .Intersect(donorInfo.MatchingGGroups)
                .Any();
        }

        /// <summary>
        /// Do both typings have intersecting P Groups?
        /// </summary>
        private static bool IsPGroupMatch(
            IHlaScoringInfo patientInfo,
            IHlaScoringInfo donorInfo)
        {
            return patientInfo.MatchingPGroups
                .Intersect(donorInfo.MatchingPGroups)
                .Any();
        }

        private bool IsPermissiveMismatch(
            IHlaLookupResult patientLookupResult,
            IHlaLookupResult donorLookupResult)
        {
            return permissiveMismatchCalculator.IsPermissiveMismatch(
                patientLookupResult.Locus,
                patientLookupResult.LookupName,
                donorLookupResult.LookupName);
        }
    }
}