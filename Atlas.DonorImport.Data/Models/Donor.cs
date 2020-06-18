using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Atlas.Common.Utils.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// ReSharper disable InconsistentNaming

namespace Atlas.DonorImport.Data.Models
{
    [Table("Donors")]
    public class Donor
    {
        /// <remarks>
        /// These must remaining in synch with the fields added to the rows of the
        /// data-table in <see cref="DonorImportRepository.BuildDonorInsertDataTable"/>
        /// </remarks>
        public static readonly string[] InsertionDataTableColumnNames =
        {
            nameof(AtlasId),
            nameof(ExternalDonorCode),
            nameof(DonorType),
            nameof(EthnicityCode),
            nameof(RegistryCode),
            nameof(A_1),
            nameof(A_2),
            nameof(B_1),
            nameof(B_2),
            nameof(C_1),
            nameof(C_2),
            nameof(DPB1_1),
            nameof(DPB1_2),
            nameof(DQB1_1),
            nameof(DQB1_2),
            nameof(DRB1_1),
            nameof(DRB1_2),
            nameof(Hash),
            nameof(UpdateFile),
            nameof(LastUpdated),
        };

        public static readonly string[] UpdateDbTableColumnNames = InsertionDataTableColumnNames.Except(new[]{nameof(AtlasId)}).ToArray();

        /// <summary>
        /// Donor identifier for use within ATLAS - autogenerated on import to this database.
        /// This should be the only identifier used internally in e.g. matching internal implementation.
        /// </summary>
        public int AtlasId { get; set; }

        /// <summary>
        /// Donor identifier as provided by consumer of ATLAS.
        /// This should be the only identifier exposed to the consumer in e.g. search results.
        /// </summary>
        [MaxLength(64)]
        public string ExternalDonorCode { get; set; }

        /// <summary>
        /// Records the file which last updated the donor.
        /// Intended for Diagnostics.
        /// </summary>
        [MaxLength(256)]
        public string UpdateFile { get; set; }

        /// <summary>
        /// Records the last time the donor record was updated.
        /// Intended for Diagnostics.
        /// </summary>
        public DateTimeOffset LastUpdated { get; set; }
        
        public DatabaseDonorType DonorType { get; set; }

        [MaxLength(256)]
        public string EthnicityCode { get; set; }

        [MaxLength(256)]
        public string RegistryCode { get; set; }

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
        public string Hash { get; set; }

        /// <summary>
        /// Calculates a hash of donor data.
        /// Used to efficiently determine whether an inbound donor's details matches one already stored in the system. 
        /// </summary>
        public string CalculateHash()
        {
            // Does NOT included:
            // * AtlasId
            // * UpdateFile
            // * LastUpdated
            return
                $"{ExternalDonorCode}|{DonorType}|{EthnicityCode}|{RegistryCode}|{A_1}|{A_2}|{B_1}|{B_2}|{C_1}|{C_2}|{DPB1_1}|{DPB1_2}|{DQB1_1}|{DQB1_2}|{DRB1_1}|{DRB1_2}"
                    .ToMd5Hash();
        }
    }

    public static class DonorModelBuilder
    {
        public static void SetUpDonorModel(this EntityTypeBuilder<Donor> donorModel)
        {
            donorModel.HasKey(d => d.AtlasId);
            donorModel.Property(p => p.AtlasId).ValueGeneratedOnAdd();
            donorModel.HasIndex(d => d.ExternalDonorCode).IsUnique();
            donorModel.HasIndex(d => d.Hash);
        }
    }
}