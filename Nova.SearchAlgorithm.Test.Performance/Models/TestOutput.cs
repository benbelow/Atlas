﻿using Nova.SearchAlgorithm.Client.Models;

namespace Nova.SearchAlgorithm.Test.Performance.Models
{
    public class TestOutput
    {
        public string DonorId { get; set; }

        public long ElapsedMilliseconds { get; set; }
        public int MatchedDonors { get; set; }
        
        public int? SolarSearchElapsedMilliseconds { get; set; }
        public int? SolarSearchMatchedDonors { get; set; }
        
        public Environment Environment { get; set; }
        public string DatabaseSize { get; set; }

        public SearchType SearchType { get; set; }
        public DonorType DonorType { get; set; }
        public bool IsAlignedRegistriesSearch { get; set; }

        public string HlaA1 { get; set; }
        public string HlaA2 { get; set; }
        public string HlaB1 { get; set; }
        public string HlaB2 { get; set; }
        public string HlaC1 { get; set; }
        public string HlaC2 { get; set; }
        public string HlaDqb11 { get; set; }
        public string HlaDqb12 { get; set; }
        public string HlaDrb11 { get; set; }
        public string HlaDrb12 { get; set; }

        public TestOutput(TestInput input, SearchMetrics metrics)
        {
            DonorId = input.DonorId;
            SearchType = input.SearchType;
            DonorType = input.DonorType;
            IsAlignedRegistriesSearch = input.IsAlignedRegistriesSearch;

            HlaA1 = input.Hla.A_1;
            HlaA2 = input.Hla.A_2;
            HlaB1 = input.Hla.B_1;
            HlaB2 = input.Hla.B_2;
            HlaC1 = input.Hla.C_1;
            HlaC2 = input.Hla.C_2;
            HlaDqb11 = input.Hla.Dqb1_1;
            HlaDqb12 = input.Hla.Dqb1_2;
            HlaDrb11 = input.Hla.Drb1_1;
            HlaDrb12 = input.Hla.Drb1_2;

            ElapsedMilliseconds = metrics.ElapsedMilliseconds;
            MatchedDonors = metrics.DonorsReturned;

            Environment = input.AlgorithmInstanceInfo.Environment;
            DatabaseSize = input.AlgorithmInstanceInfo.DatabaseSize;

            SolarSearchElapsedMilliseconds = input.SolarSearchElapsedMilliseconds;
            SolarSearchMatchedDonors = input.SolarSearchMatchedDonors;
        }
    }
}