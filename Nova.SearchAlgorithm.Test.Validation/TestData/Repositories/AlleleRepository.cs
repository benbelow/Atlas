﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents.Spatial;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Test.Validation.TestData.Helpers;
using Nova.SearchAlgorithm.Test.Validation.TestData.Models.Hla;

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
        PhenotypeInfo<List<AlleleTestData>> AllelesWithThreeFieldMatchPossible();
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

        public PhenotypeInfo<List<AlleleTestData>> AllelesWithThreeFieldMatchPossible()
        {
            return FourFieldAlleles().Map((locus, position, alleles) =>
            {
                var groupedAlleles = alleles.GroupBy(a => AlleleSplitter.RemoveLastField(a.AlleleName)).Where(g => g.Count() > 1);
                return alleles.Where(a => groupedAlleles.Any(g => Equals(g.Key, AlleleSplitter.RemoveLastField(a.AlleleName)))).ToList(); 
            });
        }

        public PhenotypeInfo<List<AlleleTestData>> AllTgsAlleles()
        {
            return FourFieldAlleles().Map((l, p, alleles) =>
                alleles.Concat(ThreeFieldAlleles().DataAtPosition(l, p))
                    .Concat(TwoFieldAlleles().DataAtPosition(l, p))
                    .ToList()
            );
        }
    }
}