using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAssessmentTool.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToTrainee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Trainees",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPasswordReset",
                table: "Trainees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Trainees",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "LastPasswordReset",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Trainees");
        }
    }
}
