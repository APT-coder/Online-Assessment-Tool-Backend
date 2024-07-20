using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OnlineAssessmentTool.Migrations
{
    /// <inheritdoc />
    public partial class addedAssessmentScheduleandAnswerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentScores",
                columns: table => new
                {
                    AssessmentScoreId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduledAssessmentId = table.Column<int>(type: "integer", nullable: false),
                    TraineeId = table.Column<int>(type: "integer", nullable: false),
                    AvergeScore = table.Column<int>(type: "integer", nullable: false),
                    CalculatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentScores", x => x.AssessmentScoreId);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledAssessments",
                columns: table => new
                {
                    ScheduledAssessmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BatchId = table.Column<int>(type: "integer", nullable: false),
                    AssessmentId = table.Column<int>(type: "integer", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AssessmentDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CanRandomizeQuestion = table.Column<bool>(type: "boolean", nullable: false),
                    CanDisplayResult = table.Column<bool>(type: "boolean", nullable: false),
                    CanSubmitBeforeEnd = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledAssessments", x => x.ScheduledAssessmentId);
                });

            migrationBuilder.CreateTable(
                name: "TraineeAnswers",
                columns: table => new
                {
                    TraineeAnswerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduledAssessmentId = table.Column<int>(type: "integer", nullable: false),
                    TraineeId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraineeAnswers", x => x.TraineeAnswerId);
                    table.ForeignKey(
                        name: "FK_TraineeAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatedBy",
                table: "Questions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_CreatedBy",
                table: "Assessments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TraineeAnswers_QuestionId",
                table: "TraineeAnswers",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Trainers_CreatedBy",
                table: "Assessments",
                column: "CreatedBy",
                principalTable: "Trainers",
                principalColumn: "TrainerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Trainers_CreatedBy",
                table: "Questions",
                column: "CreatedBy",
                principalTable: "Trainers",
                principalColumn: "TrainerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Trainers_CreatedBy",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Trainers_CreatedBy",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "AssessmentScores");

            migrationBuilder.DropTable(
                name: "ScheduledAssessments");

            migrationBuilder.DropTable(
                name: "TraineeAnswers");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CreatedBy",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_CreatedBy",
                table: "Assessments");
        }
    }
}
