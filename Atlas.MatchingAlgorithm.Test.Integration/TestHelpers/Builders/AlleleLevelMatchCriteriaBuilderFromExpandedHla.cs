﻿using Atlas.HlaMetadataDictionary.Models.Lookups.MatchingLookup;
using Atlas.MatchingAlgorithm.Client.Models;
using Atlas.MatchingAlgorithm.Common.Models;

namespace Atlas.MatchingAlgorithm.Test.Integration.TestHelpers.Builders
{
    public class AlleleLevelMatchCriteriaFromExpandedHla
    {
        private readonly Locus locusUnderTest;
        private readonly DonorType donorType;

        public AlleleLevelMatchCriteriaFromExpandedHla(
            Locus locusUnderTest,
            DonorType donorType)
        {
            this.locusUnderTest = locusUnderTest;
            this.donorType = donorType;
        }

        /// <summary>
        /// Uses AlleleLevelMatchCriteriaBuilder to build criteria with 
        /// the specified number of mismatches at the locus under test.
        /// </summary>
        public AlleleLevelMatchCriteria GetAlleleLevelMatchCriteria(
            PhenotypeInfo<IHlaMatchingLookupResult> phenotype,
            int mismatchCount = 0)
        {
            var builder = GetDefaultBuilder(phenotype);

            if (mismatchCount > 0)
            {
                var locusUnderTestCriteria = GetLocusMatchCriteria(locusUnderTest, mismatchCount, phenotype);

                builder
                    .WithDonorMismatchCount(mismatchCount)
                    .WithLocusMatchCriteria(locusUnderTest, locusUnderTestCriteria);
            }
                              
            return builder.Build();
        }

        private AlleleLevelMatchCriteriaBuilder GetDefaultBuilder(PhenotypeInfo<IHlaMatchingLookupResult> phenotype)
        {
            const int zeroMismatchCount = 0;
            return new AlleleLevelMatchCriteriaBuilder()
                .WithSearchType(donorType)
                .WithLocusMatchCriteria(Locus.A, GetLocusMatchCriteria(Locus.A, zeroMismatchCount, phenotype))
                .WithLocusMatchCriteria(Locus.B, GetLocusMatchCriteria(Locus.B, zeroMismatchCount, phenotype))
                .WithLocusMatchCriteria(Locus.Drb1, GetLocusMatchCriteria(Locus.Drb1, zeroMismatchCount, phenotype))
                .WithDonorMismatchCount(zeroMismatchCount);
        }

        private static AlleleLevelLocusMatchCriteria GetLocusMatchCriteria(
            Locus locus,
            int mismatchCount,
            PhenotypeInfo<IHlaMatchingLookupResult> phenotype)
        {
            return new AlleleLevelLocusMatchCriteria
            {
                MismatchCount = mismatchCount,
                PGroupsToMatchInPositionOne = phenotype.DataAtPosition(locus, TypePosition.One).MatchingPGroups,
                PGroupsToMatchInPositionTwo = phenotype.DataAtPosition(locus, TypePosition.Two).MatchingPGroups
            };
        }
    }
}
