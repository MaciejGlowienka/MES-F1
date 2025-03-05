using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class TeamStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_AspNetUsers_IdAccount",
                table: "Workers");

            migrationBuilder.RenameColumn(
                name: "IdAccount",
                table: "Workers",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "IdWorker",
                table: "Workers",
                newName: "WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Workers_IdAccount",
                table: "Workers",
                newName: "IX_Workers_AccountId");

            migrationBuilder.CreateTable(
                name: "TeamRoles",
                columns: table => new
                {
                    TeamRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRoles", x => x.TeamRoleId);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "TeamWorkerRoleAssignments",
                columns: table => new
                {
                    TeamWorkerRoleAssignId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    TeamRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamWorkerRoleAssignments", x => x.TeamWorkerRoleAssignId);
                    table.ForeignKey(
                        name: "FK_TeamWorkerRoleAssignments_TeamRoles_TeamRoleId",
                        column: x => x.TeamRoleId,
                        principalTable: "TeamRoles",
                        principalColumn: "TeamRoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamWorkerRoleAssignments_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamWorkerRoleAssignments_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamWorkerRoleAssignments_TeamId",
                table: "TeamWorkerRoleAssignments",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamWorkerRoleAssignments_TeamRoleId",
                table: "TeamWorkerRoleAssignments",
                column: "TeamRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamWorkerRoleAssignments_WorkerId",
                table: "TeamWorkerRoleAssignments",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_AspNetUsers_AccountId",
                table: "Workers",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_AspNetUsers_AccountId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "TeamWorkerRoleAssignments");

            migrationBuilder.DropTable(
                name: "TeamRoles");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Workers",
                newName: "IdAccount");

            migrationBuilder.RenameColumn(
                name: "WorkerId",
                table: "Workers",
                newName: "IdWorker");

            migrationBuilder.RenameIndex(
                name: "IX_Workers_AccountId",
                table: "Workers",
                newName: "IX_Workers_IdAccount");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_AspNetUsers_IdAccount",
                table: "Workers",
                column: "IdAccount",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
