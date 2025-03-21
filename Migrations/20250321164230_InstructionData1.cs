using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class InstructionData1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Instructions",
                columns: new[] { "InstructionId", "InstructionName", "InstructionPartType", "InstructionURL" },
                values: new object[,]
                {
                    { 1, "Front Wing 2025 mid v.1.0", "FrontWing", null },
                    { 2, "Front Wing 2025 low v.1.0", "FrontWing", null },
                    { 3, "Front Wing 2025 high v.1.0", "FrontWing", null },
                    { 4, "Front Wing 2025 mid v.1.1", "FrontWing", null },
                    { 5, "Right Sidepod Cooling pipe 2025 v.1.0", "CoolingPipe", null },
                    { 6, "MGU-K Radiator 2025 v.1.0", "Radiator", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 6);
        }
    }
}
