﻿using Microsoft.Azure.Documents.Spatial;
using Nova.SearchAlgorithm.Client.Models;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Data.Entity;

namespace Nova.SearchAlgorithm.Test.Validation.TestData
{
    /// <summary>
    /// Test data is generated by taking a full set of TGS typed allele data (genotype), and artificially lowering the typing resolution
    /// </summary>
    public class DonorGenotype
    {
        public PhenotypeInfo<TgsAllele> Hla;

        public Donor BuildTgsTypedDonor(int donorId, DonorType donorType, RegistryCode registryCode)
        {
            var tgsHla = Hla.Map<string>((locus, position, tgsAllele) => tgsAllele.TgsTypedAllele);
            return BuildDonor(donorId, donorType, registryCode, tgsHla);
        }

        private Donor BuildDonor(int donorId, DonorType donorType, RegistryCode registryCode, PhenotypeInfo<string> hla)
        {
            return new Donor
            {
                DonorId = donorId,
                DonorType = donorType,
                RegistryCode = registryCode,
                A_1 = hla.A_1,
                A_2 = hla.A_2,
                B_1 = hla.B_1,
                B_2 = hla.B_2,
                DRB1_1 = hla.DRB1_1,
                DRB1_2 = hla.DRB1_2,
                C_1 = hla.C_1,
                C_2 = hla.C_2,
                DQB1_1 = hla.DQB1_1,
                DQB1_2 = hla.DQB1_2,
            };
        }
    }
}