using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class defaultAdmin2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-123",
                column: "PasswordHash",
                value: "61NAlDODEU7eHVwDI4SKrlPl-UepNOB6jWkU_7nAYks=");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "director-id-456",
                column: "PasswordHash",
                value: "61NAlDODEU7eHVwDI4SKrlPl-UepNOB6jWkU_7nAYks=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-123",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKAGu++1cx6whmHXE4spxwVRL+k43Yb4U09MIVuMShpL2N+EKn0bZ5cuJbnAHP1cLw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "director-id-456",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKAGu++1cx6whmHXE4spxwVRL+k43Yb4U09MIVuMShpL2N+EKn0bZ5cuJbnAHP1cLw==");
        }
    }
}
