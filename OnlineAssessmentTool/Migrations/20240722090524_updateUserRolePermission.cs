using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAssessmentTool.Migrations
{
    /// <inheritdoc />
    public partial class updateUserRolePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permissions_PermissionsId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Roles_RolesId",
                table: "RolePermission");

            migrationBuilder.RenameColumn(
                name: "RolesId",
                table: "RolePermission",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "PermissionsId",
                table: "RolePermission",
                newName: "PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_RolesId",
                table: "RolePermission",
                newName: "IX_RolePermission_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permissions_PermissionId",
                table: "RolePermission",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Roles_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permissions_PermissionId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Roles_RoleId",
                table: "RolePermission");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "RolePermission",
                newName: "RolesId");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "RolePermission",
                newName: "PermissionsId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                newName: "IX_RolePermission_RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permissions_PermissionsId",
                table: "RolePermission",
                column: "PermissionsId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Roles_RolesId",
                table: "RolePermission",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
