using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAssessmentTool.Migrations
{
    /// <inheritdoc />
    public partial class QuestionOptionUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "Option1",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "Option2",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "Option3",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "Option4",
                table: "QuestionOptions");

            migrationBuilder.AddColumn<List<string>>(
                name: "CorrectAnswers",
                table: "QuestionOptions",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Options",
                table: "QuestionOptions",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "Options",
                table: "QuestionOptions");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "QuestionOptions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Option1",
                table: "QuestionOptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option2",
                table: "QuestionOptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option3",
                table: "QuestionOptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option4",
                table: "QuestionOptions",
                type: "text",
                nullable: true);
        }
    }
}
