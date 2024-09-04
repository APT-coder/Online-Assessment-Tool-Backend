using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAssessmentTool.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedDateToBatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "batch",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "batch",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "batch");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "batch");
        }
    }
}
