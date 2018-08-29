﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Test.Validation.TestData.Exceptions;
using Nova.SearchAlgorithm.Test.Validation.TestData.Helpers;
using Nova.SearchAlgorithm.Test.Validation.TestData.Models.Hla;
using Nova.SearchAlgorithm.Test.Validation.TestData.Resources;

namespace Nova.SearchAlgorithm.Test.Validation.TestData.Repositories
{
    public interface IAlleleRepository
    {
        PhenotypeInfo<List<AlleleTestData>> FourFieldAlleles();
        PhenotypeInfo<List<AlleleTestData>> ThreeFieldAlleles();
        PhenotypeInfo<List<AlleleTestData>> TwoFieldAlleles();
        
        /// <returns>
        /// All 2, 3, and 4 field alleles from the datasets generated from TGS-typed donors in Solar.
        /// Does not include manually curated test data used for e.g. p-group/g-group matching
        /// </returns>
        PhenotypeInfo<List<AlleleTestData>> AllTgsAlleles();
        
        PhenotypeInfo<List<AlleleTestData>> AllelesForGGroupMatching();
        LocusInfo<List<AlleleTestData>> DonorAllelesForPGroupMatching();
        LocusInfo<AlleleTestData> PatientAllelesForPGroupMatching();
        PhenotypeInfo<List<AlleleTestData>> DonorAllelesWithThreeFieldMatchPossible();
        PhenotypeInfo<List<AlleleTestData>> PatientAllelesWithThreeFieldMatchPossible();
        PhenotypeInfo<List<AlleleTestData>> AllelesWithTwoFieldMatchPossible();
    }

    /// <summary>
    /// Repository layer for accessing test allele data stored in Resources directory.
    /// </summary>
    public class AlleleRepository : IAlleleRepository
    {
        public PhenotypeInfo<List<AlleleTestData>> FourFieldAlleles()
        {
            return Resources.FourFieldAlleles.Alleles;
        }

        public PhenotypeInfo<List<AlleleTestData>> ThreeFieldAlleles()
        {
            return Resources.ThreeFieldAlleles.Alleles;
        }

        public PhenotypeInfo<List<AlleleTestData>> TwoFieldAlleles()
        {
            return Resources.TwoFieldAlleles.Alleles;
        }

        public PhenotypeInfo<List<AlleleTestData>> AllelesForGGroupMatching()
        {
            return Resources.GGroupMatchingAlleles.Alleles;
        }

        public LocusInfo<List<AlleleTestData>> DonorAllelesForPGroupMatching()
        {
            return Resources.PGroupMatchingAlleles.DonorAlleles;
        }

        public LocusInfo<AlleleTestData> PatientAllelesForPGroupMatching()
        {
            return Resources.PGroupMatchingAlleles.PatientAlleles;
        }

        public PhenotypeInfo<List<AlleleTestData>> DonorAllelesWithThreeFieldMatchPossible()
        {
            return FourFieldAlleles().Map((locus, position, alleles) =>
            {
                var groupedAlleles = AlleleGroupsWithSharedFirstThreeFields(alleles);

                return alleles.Where(a =>
                {
                    var group = groupedAlleles.SingleOrDefault(g => Equals(g.Key, AlleleSplitter.FirstThreeFieldsAsString(a.AlleleName)));
                    if (group == null)
                    {
                        return false;
                    }

                    // If only two alleles exist in a group with the same first three fields, only allow the first one to be used by donors.
                    // This ensures that patients will always be able to pick the other one, without causing an exact allele level match
                    if (group.Count() == 2)
                    {
                        return group.First().AlleleName == a.AlleleName;
                    }

                    return true;
                }).ToList();
            });
        }

        public PhenotypeInfo<List<AlleleTestData>> PatientAllelesWithThreeFieldMatchPossible()
        {
            return FourFieldAlleles().Map((locus, position, alleles) =>
            {
                var groupedAlleles = AlleleGroupsWithSharedFirstThreeFields(alleles);
                return groupedAlleles.SelectMany(g => g).ToList();
            });
        }

        public PhenotypeInfo<List<AlleleTestData>> AllelesWithTwoFieldMatchPossible()
        {
            return Resources.AllelesWithDifferentThirdFields.Alleles.ToPhenotypeInfo((l, alleles) => 
                new Tuple<List<AlleleTestData>, List<AlleleTestData>>(alleles, alleles));
        }

        public PhenotypeInfo<List<AlleleTestData>> AllTgsAlleles()
        {
            return FourFieldAlleles().Map((l, p, alleles) =>
                alleles.Concat(ThreeFieldAlleles().DataAtPosition(l, p))
                    .Concat(TwoFieldAlleles().DataAtPosition(l, p))
                    .ToList()
            );
        }

        private static IEnumerable<IGrouping<string, AlleleTestData>> AlleleGroupsWithSharedFirstThreeFields(List<AlleleTestData> alleles)
        {
            var groupedAlleles = alleles
                .GroupBy(a => AlleleSplitter.FirstThreeFieldsAsString(a.AlleleName))
                .Where(g => g.Count() > 1)
                .ToList();
            return groupedAlleles;
        }
    }
}