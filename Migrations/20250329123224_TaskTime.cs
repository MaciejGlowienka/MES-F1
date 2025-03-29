using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class TaskTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualEndTime",
                table: "ProductionTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualStartTime",
                table: "ProductionTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedEndTime",
                table: "ProductionTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedStartTime",
                table: "ProductionTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualEndTime",
                table: "ProductionTasks");

            migrationBuilder.DropColumn(
                name: "ActualStartTime",
                table: "ProductionTasks");

            migrationBuilder.DropColumn(
                name: "PlannedEndTime",
                table: "ProductionTasks");

            migrationBuilder.DropColumn(
                name: "PlannedStartTime",
                table: "ProductionTasks");
        }
    }
}
