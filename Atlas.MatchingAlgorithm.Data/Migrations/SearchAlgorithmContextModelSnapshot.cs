﻿// <auto-generated />
using System;
using Atlas.MatchingAlgorithm.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Atlas.MatchingAlgorithm.Data.Migrations
{
    [DbContext(typeof(SearchAlgorithmContext))]
    partial class SearchAlgorithmContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.Donor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("A_1")
                        .IsRequired();

                    b.Property<string>("A_2")
                        .IsRequired();

                    b.Property<string>("B_1")
                        .IsRequired();

                    b.Property<string>("B_2")
                        .IsRequired();

                    b.Property<string>("C_1");

                    b.Property<string>("C_2");

                    b.Property<string>("DPB1_1");

                    b.Property<string>("DPB1_2");

                    b.Property<string>("DQB1_1");

                    b.Property<string>("DQB1_2");

                    b.Property<string>("DRB1_1")
                        .IsRequired();

                    b.Property<string>("DRB1_2")
                        .IsRequired();

                    b.Property<int>("DonorId");

                    b.Property<int>("DonorType");

                    b.Property<bool>("IsAvailableForSearch")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.HasKey("Id");

                    b.HasIndex("DonorId")
                        .HasName("FI_DonorIdsWithoutLocusDQB1")
                        .HasFilter("[DQB1_1] IS NULL AND [DQB1_2] IS NULL")
                        .HasAnnotation("SqlServer:Include", new[] { "DQB1_1", "DQB1_2" });

                    b.ToTable("Donors");
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.DonorManagementLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DonorId");

                    b.Property<DateTimeOffset>("LastUpdateDateTime");

                    b.Property<long>("SequenceNumberOfLastUpdate");

                    b.HasKey("Id");

                    b.HasIndex("DonorId")
                        .IsUnique();

                    b.ToTable("DonorManagementLogs");
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.MatchingHla.MatchingHlaAtA", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DonorId");

                    b.Property<int?>("PGroup_Id");

                    b.Property<int>("TypePosition");

                    b.HasKey("Id");

                    b.HasIndex("PGroup_Id");

                    b.ToTable("MatchingHlaAtA");
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.MatchingHla.MatchingHlaAtB", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DonorId");

                    b.Property<int?>("PGroup_Id");

                    b.Property<int>("TypePosition");

                    b.HasKey("Id");

                    b.HasIndex("PGroup_Id");

                    b.ToTable("MatchingHlaAtB");
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.MatchingHla.MatchingHlaAtC", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DonorId");

                    b.Property<int?>("PGroup_Id");

                    b.Property<int>("TypePosition");

                    b.HasKey("Id");

                    b.HasIndex("PGroup_Id");

                    b.ToTable("MatchingHlaAtC");
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.MatchingHla.MatchingHlaAtDqb1", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DonorId");

                    b.Property<int?>("PGroup_Id");

                    b.Property<int>("TypePosition");

                    b.HasKey("Id");

                    b.HasIndex("PGroup_Id");

                    b.ToTable("MatchingHlaAtDQB1");
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.MatchingHla.MatchingHlaAtDrb1", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DonorId");

                    b.Property<int?>("PGroup_Id");

                    b.Property<int>("TypePosition");

                    b.HasKey("Id");

                    b.HasIndex("PGroup_Id");

                    b.ToTable("MatchingHlaAtDRB1");
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.PGroupName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("PGroupNames");
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.MatchingHla.MatchingHlaAtA", b =>
                {
                    b.HasOne("Atlas.MatchingAlgorithm.Data.Models.PGroupName", "PGroup")
                        .WithMany()
                        .HasForeignKey("PGroup_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.MatchingHla.MatchingHlaAtB", b =>
                {
                    b.HasOne("Atlas.MatchingAlgorithm.Data.Models.PGroupName", "PGroup")
                        .WithMany()
                        .HasForeignKey("PGroup_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.MatchingHla.MatchingHlaAtC", b =>
                {
                    b.HasOne("Atlas.MatchingAlgorithm.Data.Models.PGroupName", "PGroup")
                        .WithMany()
                        .HasForeignKey("PGroup_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.MatchingHla.MatchingHlaAtDqb1", b =>
                {
                    b.HasOne("Atlas.MatchingAlgorithm.Data.Models.PGroupName", "PGroup")
                        .WithMany()
                        .HasForeignKey("PGroup_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Atlas.MatchingAlgorithm.Data.Models.Entities.MatchingHla.MatchingHlaAtDrb1", b =>
                {
                    b.HasOne("Atlas.MatchingAlgorithm.Data.Models.PGroupName", "PGroup")
                        .WithMany()
                        .HasForeignKey("PGroup_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
