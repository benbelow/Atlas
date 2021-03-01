﻿// <auto-generated />
using Atlas.RepeatSearch.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Atlas.RepeatSearch.Data.Migrations
{
    [DbContext(typeof(RepeatSearchContext))]
    partial class RepeatSearchContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("RepeatSearch")
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Atlas.RepeatSearch.Data.Models.CanonicalResultSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("OriginalSearchRequestId")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("OriginalSearchRequestId")
                        .IsUnique();

                    b.ToTable("CanonicalResultSets");
                });

            modelBuilder.Entity("Atlas.RepeatSearch.Data.Models.SearchResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AtlasDonorId")
                        .HasColumnType("int");

                    b.Property<int>("CanonicalResultSetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CanonicalResultSetId");

                    b.HasIndex("AtlasDonorId", "CanonicalResultSetId")
                        .IsUnique();

                    b.ToTable("SearchResults");
                });

            modelBuilder.Entity("Atlas.RepeatSearch.Data.Models.SearchResult", b =>
                {
                    b.HasOne("Atlas.RepeatSearch.Data.Models.CanonicalResultSet", null)
                        .WithMany("SearchResults")
                        .HasForeignKey("CanonicalResultSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
