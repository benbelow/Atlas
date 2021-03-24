﻿// <auto-generated />
using System;
using Atlas.MatchPrediction.Test.Verification.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Atlas.MatchPrediction.Test.Verification.Data.Migrations
{
    [DbContext(typeof(MatchPredictionVerificationContext))]
    partial class MatchPredictionVerificationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.ExpandedMac", b =>
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

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.MaskingRecord", b =>
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

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.NormalisedHaplotypeFrequency", b =>
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

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.NormalisedPool", b =>
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

                    b.Property<string>("TypingCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("NormalisedPool");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.Simulant", b =>
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

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.TestDonorExportRecord", b =>
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

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.TestHarness", b =>
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

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.LocusMatchCount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Locus")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<int?>("MatchCount")
                        .HasColumnType("int");

                    b.Property<int>("MatchedDonor_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchedDonor_Id", "Locus", "MatchCount");

                    b.ToTable("MatchCounts");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.MatchProbability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Locus")
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("MatchedDonor_Id")
                        .HasColumnType("int");

                    b.Property<int>("MismatchCount")
                        .HasColumnType("int");

                    b.Property<decimal?>("Probability")
                        .HasColumnType("decimal(6,5)");

                    b.HasKey("Id");

                    b.HasIndex("MatchedDonor_Id");

                    b.ToTable("MatchProbabilities");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.MatchedDonor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MatchPredictionResult")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MatchedDonorSimulant_Id")
                        .HasColumnType("int");

                    b.Property<string>("MatchingResult")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SearchRequestRecord_Id")
                        .HasColumnType("int");

                    b.Property<int>("TotalMatchCount")
                        .HasColumnType("int");

                    b.Property<int>("TypedLociCount")
                        .HasColumnType("int");

                    b.Property<bool?>("WasDonorRepresented")
                        .HasColumnType("bit");

                    b.Property<bool?>("WasPatientRepresented")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("MatchedDonorSimulant_Id");

                    b.HasIndex("SearchRequestRecord_Id", "MatchedDonorSimulant_Id", "TotalMatchCount");

                    b.ToTable("MatchedDonors");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.SearchRequestRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AtlasSearchIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("DonorMismatchCount")
                        .HasColumnType("int");

                    b.Property<double?>("MatchPredictionTimeInMs")
                        .HasColumnType("float");

                    b.Property<int?>("MatchedDonorCount")
                        .HasColumnType("int");

                    b.Property<double?>("MatchingAlgorithmTimeInMs")
                        .HasColumnType("float");

                    b.Property<double?>("OverallSearchTimeInMs")
                        .HasColumnType("float");

                    b.Property<int>("PatientSimulant_Id")
                        .HasColumnType("int");

                    b.Property<bool>("SearchResultsRetrieved")
                        .HasColumnType("bit");

                    b.Property<int>("VerificationRun_Id")
                        .HasColumnType("int");

                    b.Property<bool>("WasMatchPredictionRun")
                        .HasColumnType("bit");

                    b.Property<bool?>("WasSuccessful")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AtlasSearchIdentifier");

                    b.HasIndex("PatientSimulant_Id");

                    b.HasIndex("VerificationRun_Id", "PatientSimulant_Id", "SearchResultsRetrieved");

                    b.ToTable("SearchRequests");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.VerificationRun", b =>
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

                    b.Property<bool>("SearchRequestsSubmitted")
                        .HasColumnType("bit");

                    b.Property<int>("TestHarness_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TestHarness_Id");

                    b.ToTable("VerificationRuns");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.MaskingRecord", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.TestHarness", null)
                        .WithMany()
                        .HasForeignKey("TestHarness_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.NormalisedHaplotypeFrequency", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.NormalisedPool", null)
                        .WithMany()
                        .HasForeignKey("NormalisedPool_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.Simulant", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.TestHarness", null)
                        .WithMany()
                        .HasForeignKey("TestHarness_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.TestDonorExportRecord", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.TestHarness", null)
                        .WithMany()
                        .HasForeignKey("TestHarness_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.TestHarness", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.NormalisedPool", null)
                        .WithMany()
                        .HasForeignKey("NormalisedPool_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.LocusMatchCount", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.MatchedDonor", null)
                        .WithMany()
                        .HasForeignKey("MatchedDonor_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.MatchProbability", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.MatchedDonor", null)
                        .WithMany()
                        .HasForeignKey("MatchedDonor_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.MatchedDonor", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.Simulant", null)
                        .WithMany()
                        .HasForeignKey("MatchedDonorSimulant_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.SearchRequestRecord", null)
                        .WithMany()
                        .HasForeignKey("SearchRequestRecord_Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.SearchRequestRecord", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.Simulant", null)
                        .WithMany()
                        .HasForeignKey("PatientSimulant_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.VerificationRun", null)
                        .WithMany()
                        .HasForeignKey("VerificationRun_Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.Verification.VerificationRun", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.Entities.TestHarness.TestHarness", null)
                        .WithMany()
                        .HasForeignKey("TestHarness_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
