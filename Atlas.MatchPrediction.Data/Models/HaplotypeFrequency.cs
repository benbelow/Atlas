﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace Atlas.MatchPrediction.Data.Models
{
    public class HaplotypeFrequency
    {
        public long Id { get; set; }
        public decimal Frequency { get; set; }

        [Required]
        public string A { get; set; }

        [Required]
        public string B { get; set; }

        [Required]
        public string C { get; set; }

        [Required]
        public string DQB1 { get; set; }

        [Required]
        public string DRB1 { get; set; }

        public const string SetIdColumnName = "Set_Id";

        [ForeignKey(SetIdColumnName)]
        public HaplotypeFrequencySet Set { get; set; }
    }
}