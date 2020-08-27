﻿// <auto-generated />
using System;
using Atlas.MatchPrediction.Test.Verification.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Atlas.MatchPrediction.Test.Verification.Data.Migrations
{
    [DbContext(typeof(MatchPredictionVerificationContext))]
    [Migration("20200827152404_CreateVerificationTables")]
    partial class CreateVerificationTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.ExpandedMac", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("SecondField")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("Code", "SecondField")
                        .IsUnique();

                    b.ToTable("ExpandedMacs");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.MaskingRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Locus")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("MaskingRequests")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestHarness_Id")
                        .HasColumnType("int");

                    b.Property<string>("TestIndividualCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("TestHarness_Id");

                    b.ToTable("MaskingRecords");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.NormalisedHaplotypeFrequency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("A")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("B")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("C")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<int>("CopyNumber")
                        .HasColumnType("int");

                    b.Property<string>("DQB1")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("DRB1")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<decimal>("Frequency")
                        .HasColumnType("decimal(20,20)");

                    b.Property<int>("NormalisedPool_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalisedPool_Id");

                    b.ToTable("NormalisedHaplotypeFrequencies");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.NormalisedPool", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("HaplotypeFrequenciesDataSource")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<int>("HaplotypeFrequencySetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("NormalisedPool");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.Simulant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("A_1")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("A_2")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("B_1")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("B_2")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("C_1")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("C_2")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("DQB1_1")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("DQB1_2")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("DRB1_1")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("DRB1_2")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("SimulatedHlaTypingCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<int?>("SourceSimulantId")
                        .HasColumnType("int");

                    b.Property<int>("TestHarness_Id")
                        .HasColumnType("int");

                    b.Property<string>("TestIndividualCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("TestHarness_Id", "TestIndividualCategory", "SimulatedHlaTypingCategory");

                    b.ToTable("Simulants");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.TestDonorExportRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset?>("DataRefreshCompleted")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("DataRefreshRecordId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("Exported")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("Started")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("TestHarness_Id")
                        .HasColumnType("int");

                    b.Property<bool?>("WasDataRefreshSuccessful")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("TestHarness_Id");

                    b.ToTable("TestDonorExportRecords");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.TestHarness", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("NormalisedPool_Id")
                        .HasColumnType("int");

                    b.Property<bool>("WasCompleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("NormalisedPool_Id");

                    b.ToTable("TestHarnesses");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.MatchProbability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Locus")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("MatchedDonor_Id")
                        .HasColumnType("int");

                    b.Property<int>("MismatchCount")
                        .HasColumnType("int");

                    b.Property<decimal>("Probability")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MatchedDonor_Id");

                    b.ToTable("MatchProbabilities");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.MatchedDonor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MatchedDonorSimulant_Id")
                        .HasColumnType("int");

                    b.Property<int>("SearchRequestRecord_Id")
                        .HasColumnType("int");

                    b.Property<int>("TotalMatchCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchedDonorSimulant_Id");

                    b.HasIndex("SearchRequestRecord_Id");

                    b.ToTable("MatchedDonors");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.SearchRequestRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AtlasSearchIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PatientSimulant_Id")
                        .HasColumnType("int");

                    b.Property<int>("VerificationRun_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PatientSimulant_Id");

                    b.HasIndex("VerificationRun_Id");

                    b.ToTable("SearchRequests");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.VerificationRun", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("SearchLociCount")
                        .HasColumnType("int");

                    b.Property<string>("SearchRequest")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestHarness_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TestHarness_Id");

                    b.ToTable("VerificationRuns");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.MaskingRecord", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.TestHarness", null)
                        .WithMany()
                        .HasForeignKey("TestHarness_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.NormalisedHaplotypeFrequency", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.NormalisedPool", null)
                        .WithMany()
                        .HasForeignKey("NormalisedPool_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.Simulant", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.TestHarness", null)
                        .WithMany()
                        .HasForeignKey("TestHarness_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.TestDonorExportRecord", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.TestHarness", null)
                        .WithMany()
                        .HasForeignKey("TestHarness_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.TestHarness", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.NormalisedPool", null)
                        .WithMany()
                        .HasForeignKey("NormalisedPool_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.MatchProbability", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.MatchedDonor", null)
                        .WithMany()
                        .HasForeignKey("MatchedDonor_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.MatchedDonor", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.Simulant", null)
                        .WithMany()
                        .HasForeignKey("MatchedDonorSimulant_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.SearchRequestRecord", null)
                        .WithMany()
                        .HasForeignKey("SearchRequestRecord_Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.SearchRequestRecord", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.Simulant", null)
                        .WithMany()
                        .HasForeignKey("PatientSimulant_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.VerificationRun", null)
                        .WithMany()
                        .HasForeignKey("VerificationRun_Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Verification.VerificationRun", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness.TestHarness", null)
                        .WithMany()
                        .HasForeignKey("TestHarness_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
