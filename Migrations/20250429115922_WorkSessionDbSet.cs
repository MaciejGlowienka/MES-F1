using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class WorkSessionDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkSessions",
                columns: table => new
                {
                    WorkSessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionTaskId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSessions", x => x.WorkSessionId);
                    table.ForeignKey(
                        name: "FK_WorkSessions_ProductionTasks_ProductionTaskId",
                        column: x => x.ProductionTaskId,
                        principalTable: "ProductionTasks",
                        principalColumn: "ProductionTaskId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkSessions_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkSessions_ProductionTaskId",
                table: "WorkSessions",
                column: "ProductionTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSessions_TeamId",
                table: "WorkSessions",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkSessions");
        }
    }
}
