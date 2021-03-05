﻿// <auto-generated />
using System;
using Atlas.DonorImport.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Atlas.DonorImport.Data.Migrations
{
    [DbContext(typeof(DonorContext))]
    [Migration("20210305160632_AddLastUpdatedIndex")]
    partial class AddLastUpdatedIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Donors")
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Atlas.DonorImport.Data.Models.Donor", b =>
                {
                    b.Property<int>("AtlasId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("A_1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("A_2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("B_1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("B_2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("C_1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("C_2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DPB1_1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DPB1_2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DQB1_1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DQB1_2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DRB1_1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DRB1_2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DonorType")
                        .HasColumnType("int");

                    b.Property<string>("EthnicityCode")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ExternalDonorCode")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Hash")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("RegistryCode")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("UpdateFile")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("AtlasId");

                    b.HasIndex("ExternalDonorCode")
                        .IsUnique()
                        .HasFilter("[ExternalDonorCode] IS NOT NULL");

                    b.HasIndex("Hash");

                    b.HasIndex("LastUpdated");

                    b.ToTable("Donors");
                });

            modelBuilder.Entity("Atlas.DonorImport.Data.Models.DonorImportHistoryRecord", b =>
                {
                    b.Property<string>("Filename")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UploadTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("FailureCount")
                        .HasColumnType("int");

                    b.Property<string>("FileStateString")
                        .HasColumnName("FileState")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ImportBegin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ImportEnd")
                        .HasColumnType("datetime2");

                    b.Property<int>("ImportedDonorsCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("ServiceBusMessageId")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Filename", "UploadTime");

                    b.ToTable("DonorImportHistory");
                });

            modelBuilder.Entity("Atlas.DonorImport.Data.Models.DonorLog", b =>
                {
                    b.Property<string>("ExternalDonorCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("LastUpdateFileUploadTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ExternalDonorCode");

                    b.HasIndex("ExternalDonorCode")
                        .IsUnique();

                    b.ToTable("DonorLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
