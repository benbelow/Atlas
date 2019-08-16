﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nova.SearchAlgorithm.Data.Migrations
{
    public partial class AddDonorManagementLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonorManagementLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DonorId = table.Column<int>(nullable: false),
                    SequenceNumberOfLastUpdate = table.Column<long>(nullable: false),
                    LastUpdateDateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorManagementLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                "UX_DonorId",
                "DonorManagementLogs",
                "DonorId",
                unique: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonorManagementLogs");
        }
    }
}
