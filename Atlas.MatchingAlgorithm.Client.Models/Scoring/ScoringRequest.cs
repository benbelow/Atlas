﻿using Atlas.MatchingAlgorithm.Client.Models.SearchRequests;

namespace Atlas.MatchingAlgorithm.Client.Models.Scoring
{
    public class ScoringRequest
    {
        public SearchHlaData DonorHla { get; set; }
        public SearchHlaData PatientHla { get; set; }
    }
}