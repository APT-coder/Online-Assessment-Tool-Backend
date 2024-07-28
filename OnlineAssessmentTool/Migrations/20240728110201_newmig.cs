using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAssessmentTool.Migrations
{
    /// <inheritdoc />
    public partial class newmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "TraineeAnswers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "ScheduledAssessments",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "Roles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Option4",
                table: "QuestionOptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Option3",
                table: "QuestionOptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Option2",
                table: "QuestionOptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Option1",
                table: "QuestionOptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PermissionName",
                table: "Permissions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Permissions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledAssessments_AssessmentId",
                table: "ScheduledAssessments",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledAssessments_BatchId",
                table: "ScheduledAssessments",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentScores_ScheduledAssessmentId",
                table: "AssessmentScores",
                column: "ScheduledAssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentScores_ScheduledAssessments_ScheduledAssessmentId",
                table: "AssessmentScores",
                column: "ScheduledAssessmentId",
                principalTable: "ScheduledAssessments",
                principalColumn: "ScheduledAssessmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledAssessments_Assessments_AssessmentId",
                table: "ScheduledAssessments",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "AssessmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledAssessments_batch_BatchId",
                table: "ScheduledAssessments",
                column: "BatchId",
                principalTable: "batch",
                principalColumn: "batchid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentScores_ScheduledAssessments_ScheduledAssessmentId",
                table: "AssessmentScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledAssessments_Assessments_AssessmentId",
                table: "ScheduledAssessments");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledAssessments_batch_BatchId",
                table: "ScheduledAssessments");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledAssessments_AssessmentId",
                table: "ScheduledAssessments");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledAssessments_BatchId",
                table: "ScheduledAssessments");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentScores_ScheduledAssessmentId",
                table: "AssessmentScores");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "ScheduledAssessments");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "TraineeAnswers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "Roles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Option4",
                table: "QuestionOptions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Option3",
                table: "QuestionOptions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Option2",
                table: "QuestionOptions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Option1",
                table: "QuestionOptions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PermissionName",
                table: "Permissions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Permissions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
