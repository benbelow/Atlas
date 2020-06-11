using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.MatchingAlgorithm.Data.Persistent.Models
{
    [Table("DataRefreshHistory")]
    public class DataRefreshRecord
    {
        public int Id { get; set; }
        public DateTime RefreshBeginUtc { get; set; }
        public DateTime? RefreshEndUtc { get; set; }

        /// <summary>
        /// The string representation of a "TransientDatabase" enum value. 
        /// </summary>
        public string Database { get; set; }

        public string HlaNomenclatureVersion { get; set; }
        public bool? WasSuccessful { get; set; }

        public DateTime? DataDeletionCompleted { get; set; }
        public DateTime? DatabaseScalingSetupCompleted { get; set; }
        public DateTime? MetadataDictionaryRefreshCompleted { get; set; }
        public DateTime? DonorImportCompleted { get; set; }
        public DateTime? DonorHlaProcessingCompleted { get; set; }
        public DateTime? DatabaseScalingTearDownCompleted { get; set; }
        public DateTime? QueuedDonorUpdatesCompleted { get; set; }
    }
}