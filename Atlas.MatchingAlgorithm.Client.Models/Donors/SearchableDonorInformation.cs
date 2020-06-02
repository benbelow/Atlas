﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace Atlas.MatchingAlgorithm.Client.Models.Donors
{
    /// <summary>
    /// Donor info containing only the information required to search for donors - e.g. by the search algorithm service
    /// This only includes the bare minimum for a search for performance reasons.
    /// </summary>
    public class SearchableDonorInformation
    {
        public int DonorId { get; set; }
        public string DonorType { get; set; }
        public string A_1 { get; set; }
        public string A_2 { get; set; }
        public string B_1 { get; set; }
        public string B_2 { get; set; }
        public string C_1 { get; set; }
        public string C_2 { get; set; }
        public string DPB1_1 { get; set; }
        public string DPB1_2 { get; set; }
        public string DQB1_1 { get; set; }
        public string DQB1_2 { get; set; }
        public string DRB1_1 { get; set; }
        public string DRB1_2 { get; set; }
    }

    public class SearchableDonorUpdate
    {
        /// <summary>
        /// The id of the audit table row that triggered this donor update
        /// </summary>
        [JsonIgnore]
        public int AuditId { get; set; }

        public int DonorId { get; set; }
        public bool IsAvailableForSearch { get; set; }
        public SearchableDonorInformation SearchableDonorInformation { get; set; }
        public DateTimeOffset? PublishedDateTime { get; set; }
    }
}