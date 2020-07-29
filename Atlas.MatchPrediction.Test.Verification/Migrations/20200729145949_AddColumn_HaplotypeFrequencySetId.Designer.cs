﻿// <auto-generated />
using System;
using Atlas.MatchPrediction.Test.Verification.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Atlas.MatchPrediction.Test.Verification.Migrations
{
    [DbContext(typeof(MatchPredictionVerificationContext))]
    [Migration("20200729145949_AddColumn_HaplotypeFrequencySetId")]
    partial class AddColumn_HaplotypeFrequencySetId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.NormalisedHaplotypeFrequency", b =>
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

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.NormalisedPool", b =>
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

                    b.Property<int>("HaplotypeFrequencySetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("NormalisedPool");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Simulant", b =>
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
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("C_2")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("DQB1_1")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("DQB1_2")
                        .IsRequired()
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

                    b.HasIndex("TestHarness_Id");

                    b.HasIndex("TestIndividualCategory", "SimulatedHlaTypingCategory");

                    b.ToTable("Simulants");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness", b =>
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

                    b.HasKey("Id");

                    b.HasIndex("NormalisedPool_Id");

                    b.ToTable("TestHarnesses");
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.NormalisedHaplotypeFrequency", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.NormalisedPool", null)
                        .WithMany()
                        .HasForeignKey("NormalisedPool_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.Simulant", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness", null)
                        .WithMany()
                        .HasForeignKey("TestHarness_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Atlas.MatchPrediction.Test.Verification.Data.Models.TestHarness", b =>
                {
                    b.HasOne("Atlas.MatchPrediction.Test.Verification.Data.Models.NormalisedPool", null)
                        .WithMany()
                        .HasForeignKey("NormalisedPool_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
