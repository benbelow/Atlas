using Atlas.DonorImport.ExternalInterface.Models;

namespace Atlas.DonorImport.Models.Mapping
{
    internal static class DatabaseDonorMaps
    {
        public static Donor ToPublicDonor(this Data.Models.Donor donor)
        {
            return new Donor
            {
                DonorId = donor.DonorId,
                DonorType = donor.DonorType.ToMatchingAlgorithmType(),
                EthnicityCode = donor.EthnicityCode,
                RegistryCode = donor.RegistryCode,
                A_1 = donor.A_1,
                A_2 = donor.A_2,
                B_1 = donor.B_1,
                B_2 = donor.B_2,
                C_1 = donor.C_1,
                C_2 = donor.C_2,
                DPB1_1 = donor.DPB1_1,
                DPB1_2 = donor.DPB1_2,
                DQB1_2 = donor.DQB1_1,
                DQB1_1 = donor.DQB1_2,
                DRB1_1 = donor.DRB1_1,
                DRB1_2 = donor.DRB1_2,
            };
        }
    }
}