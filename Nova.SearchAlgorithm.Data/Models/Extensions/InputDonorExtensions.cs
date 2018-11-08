﻿using Nova.SearchAlgorithm.Client.Models.Donors;
using Nova.SearchAlgorithm.Common.Models;
using Nova.SearchAlgorithm.Data.Entity;

namespace Nova.SearchAlgorithm.Data.Models.Extensions
{
    static class InputDonorExtensions
    {
        public static Donor ToDonorEntity(this InputDonorWithExpandedHla donor)
        {
            return new Donor
            {
                DonorId = donor.DonorId,
                DonorType = donor.DonorType,
                RegistryCode = donor.RegistryCode,
                A_1 = donor.MatchingHla?.A_1?.OriginalName,
                A_2 = donor.MatchingHla?.A_2?.OriginalName,
                B_1 = donor.MatchingHla?.B_1?.OriginalName,
                B_2 = donor.MatchingHla?.B_2?.OriginalName,
                C_1 = donor.MatchingHla?.C_1?.OriginalName,
                C_2 = donor.MatchingHla?.C_2?.OriginalName,
                DPB1_1 = donor.MatchingHla?.Dpb1_1?.OriginalName,
                DPB1_2 = donor.MatchingHla?.Dpb1_2?.OriginalName,
                DQB1_1 = donor.MatchingHla?.Dqb1_1?.OriginalName,
                DQB1_2 = donor.MatchingHla?.Dqb1_2?.OriginalName,
                DRB1_1 = donor.MatchingHla?.Drb1_1?.OriginalName,
                DRB1_2 = donor.MatchingHla?.Drb1_2?.OriginalName,
            };
        }

        public static Donor ToDonorEntity(this InputDonor donor)
        {
            return new Donor
            {
                DonorId = donor.DonorId,
                DonorType = donor.DonorType,
                RegistryCode = donor.RegistryCode,
                A_1 = donor.HlaNames?.A_1,
                A_2 = donor.HlaNames?.A_2,
                B_1 = donor.HlaNames?.B_1,
                B_2 = donor.HlaNames?.B_2,
                C_1 = donor.HlaNames?.C_1,
                C_2 = donor.HlaNames?.C_2,
                DPB1_1 = donor.HlaNames?.Dpb1_1,
                DPB1_2 = donor.HlaNames?.Dpb1_2,
                DQB1_1 = donor.HlaNames?.Dqb1_1,
                DQB1_2 = donor.HlaNames?.Dqb1_2,
                DRB1_1 = donor.HlaNames?.Drb1_1,
                DRB1_2 = donor.HlaNames?.Drb1_2,
            };
        }
    }
}
