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
        public string WmdaDatabaseVersion { get; set; } //QQ Rename this - DB column, tho'.
        public bool? WasSuccessful { get; set; }
    }
}