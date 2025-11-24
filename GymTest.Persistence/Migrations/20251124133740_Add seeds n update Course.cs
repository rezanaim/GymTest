using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymTest.Persistence.Migrations
{
    public partial class AddseedsnupdateCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "DurationInDays",
                table: "Courses",
                newName: "LectureCount");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Courses",
                newName: "DurationInMins");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsRemoved", "Name", "ParentCategoryId", "ParentId", "RemoveTime", "UpdateTime" },
                values: new object[,]
                {
                    { 1L, false, "Ball", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2L, false, "Mind", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3L, false, "Fight", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4L, false, "Winter", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Sports",
                columns: new[] { "Id", "CategoryId", "Description", "IsRemoved", "Name", "RemoveTime", "UpdateTime" },
                values: new object[,]
                {
                    { 1L, 1L, "", false, "Football", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2L, 1L, "", false, "Pingpong", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3L, 2L, "", false, "Chess", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4L, 3L, "", false, "Wrestling", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5L, 3L, "", false, "Boxing", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 6L, 4L, "", false, "Ski", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.RenameColumn(
                name: "LectureCount",
                table: "Courses",
                newName: "DurationInDays");

            migrationBuilder.RenameColumn(
                name: "DurationInMins",
                table: "Courses",
                newName: "Capacity");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
