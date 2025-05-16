using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class TeamTaskWorkerRestructuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_TeamRoles_TeamRoleId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Teams_TeamId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSessions_Teams_TeamId",
                table: "WorkSessions");

            migrationBuilder.DropIndex(
                name: "IX_WorkSessions_TeamId",
                table: "WorkSessions");

            migrationBuilder.DropIndex(
                name: "IX_Workers_TeamId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_TeamRoleId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "WorkSessions");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "TeamRoleId",
                table: "Workers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "WorkSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Workers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamRoleId",
                table: "Workers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Workers",
                keyColumn: "WorkerId",
                keyValue: 1,
                columns: new[] { "TeamId", "TeamRoleId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Workers",
                keyColumn: "WorkerId",
                keyValue: 2,
                columns: new[] { "TeamId", "TeamRoleId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Workers",
                keyColumn: "WorkerId",
                keyValue: 3,
                columns: new[] { "TeamId", "TeamRoleId" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_WorkSessions_TeamId",
                table: "WorkSessions",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_TeamId",
                table: "Workers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_TeamRoleId",
                table: "Workers",
                column: "TeamRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_TeamRoles_TeamRoleId",
                table: "Workers",
                column: "TeamRoleId",
                principalTable: "TeamRoles",
                principalColumn: "TeamRoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Teams_TeamId",
                table: "Workers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSessions_Teams_TeamId",
                table: "WorkSessions",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
