using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAssessmentTool.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserandQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TraineeAnswers_QuestionId",
                table: "TraineeAnswers");

            migrationBuilder.AddColumn<int>(
                name: "TotalScore",
                table: "Assessments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TraineeAnswers_QuestionId",
                table: "TraineeAnswers",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TraineeAnswers_QuestionId",
                table: "TraineeAnswers");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "Assessments");

            migrationBuilder.CreateIndex(
                name: "IX_TraineeAnswers_QuestionId",
                table: "TraineeAnswers",
                column: "QuestionId",
                unique: true);
        }
    }
}
