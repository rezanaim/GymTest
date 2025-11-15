using Microsoft.EntityFrameworkCore.Migrations;

namespace GymTest.Persistence.Migrations
{
    public partial class AddRoleUserInRoleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInRole_Role_RoleId",
                table: "UserInRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInRole_Users_UserId",
                table: "UserInRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInRole",
                table: "UserInRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "UserInRole",
                newName: "UserInRoles");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameIndex(
                name: "IX_UserInRole_UserId",
                table: "UserInRoles",
                newName: "IX_UserInRoles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserInRole_RoleId",
                table: "UserInRoles",
                newName: "IX_UserInRoles_RoleId");

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "UserInRoles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInRoles",
                table: "UserInRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInRoles_Roles_RoleId",
                table: "UserInRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInRoles_Users_UserId",
                table: "UserInRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInRoles_Roles_RoleId",
                table: "UserInRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInRoles_Users_UserId",
                table: "UserInRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInRoles",
                table: "UserInRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "UserInRoles",
                newName: "UserInRole");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameIndex(
                name: "IX_UserInRoles_UserId",
                table: "UserInRole",
                newName: "IX_UserInRole_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserInRoles_RoleId",
                table: "UserInRole",
                newName: "IX_UserInRole_RoleId");

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "UserInRole",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInRole",
                table: "UserInRole",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInRole_Role_RoleId",
                table: "UserInRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInRole_Users_UserId",
                table: "UserInRole",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
