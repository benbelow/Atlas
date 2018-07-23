﻿using Nova.SearchAlgorithm.Client.Models;

namespace Nova.SearchAlgorithm.Common.Models
{
    public class InputDonor
    {
        public int DonorId { get; set; }
        
        public DonorType DonorType { get; set; }
        public RegistryCode RegistryCode { get; set; }
        public PhenotypeInfo<ExpandedHla> MatchingHla { get; set; }

        public RawInputDonor ToRawInputDonor()
        {
            return new RawInputDonor
            {
                DonorId = DonorId,
                RegistryCode = RegistryCode,
                DonorType = DonorType,
                HlaNames = MatchingHla.Map((l, p, expandedHla) => expandedHla?.OriginalName)
            };
        }
    }
}