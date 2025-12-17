using Microsoft.EntityFrameworkCore.Migrations;

namespace GymTest.Persistence.Migrations
{
    public partial class mintosec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationInMins",
                table: "Courses",
                newName: "DurationInSecs");

            migrationBuilder.AddColumn<int>(
                name: "DurationInSecs",
                table: "Lectures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInSecs",
                table: "Lectures");

            migrationBuilder.RenameColumn(
                name: "DurationInSecs",
                table: "Courses",
                newName: "DurationInMins");
        }
    }
}
