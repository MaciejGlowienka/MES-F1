using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class TeamEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Workers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "TeamRoles",
                columns: new[] { "TeamRoleId", "RoleDescription", "RoleName" },
                values: new object[,]
                {
                    { 1, "Developer", "Developer" },
                    { 2, "Manager", "Manager" },
                    { 3, "Marketing Specialist", "Marketing Specialist" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "TeamId", "TeamName" },
                values: new object[,]
                {
                    { 1, "Development Team" },
                    { 2, "Marketing Team" }
                });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "WorkerId", "AccountId", "WorkerName" },
                values: new object[,]
                {
                    { 1, null, "John Doe" },
                    { 2, null, "Jane Smith" },
                    { 3, null, "Alice Johnson" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TeamRoles",
                keyColumn: "TeamRoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TeamRoles",
                keyColumn: "TeamRoleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TeamRoles",
                keyColumn: "TeamRoleId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "TeamId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "TeamId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "WorkerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "WorkerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "WorkerId",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Workers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
