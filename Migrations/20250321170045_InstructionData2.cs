using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class InstructionData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StepWorkScope",
                table: "InstructionSteps",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.InsertData(
                table: "InstructionSteps",
                columns: new[] { "InstructionStepId", "InstructionId", "InstructionStepDescription", "InstructionStepNumber", "StepWorkScope" },
                values: new object[,]
                {
                    { 1, 1, "Laying layers of carbon fiber", 1, 3 },
                    { 2, 1, "Hardening carbon fiber inside an autoclave", 2, 7 },
                    { 3, 1, "Paintjob", 3, 6 },
                    { 4, 2, "Laying layers of carbon fiber", 1, 3 },
                    { 5, 2, "Hardening carbon fiber inside an autoclave", 2, 7 },
                    { 6, 2, "Paintjob", 3, 6 },
                    { 7, 3, "Laying layers of carbon fiber", 1, 3 },
                    { 8, 3, "Hardening carbon fiber inside an autoclave", 2, 7 },
                    { 9, 3, "Paintjob", 3, 6 },
                    { 10, 4, "Laying layers of carbon fiber", 1, 3 },
                    { 11, 4, "Hardening carbon fiber inside an autoclave", 2, 7 },
                    { 12, 4, "Paintjob", 3, 6 },
                    { 13, 5, "Cutting set of metal parts", 1, 1 },
                    { 14, 5, "Welding pipes", 2, 8 },
                    { 15, 5, "Grinding and blending a weld", 3, 8 },
                    { 16, 6, "Cutting set of metal parts", 1, 1 },
                    { 17, 6, "Welding radiator plates", 2, 8 },
                    { 18, 6, "Grinding and blending a weld", 3, 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 18);

            migrationBuilder.AlterColumn<string>(
                name: "StepWorkScope",
                table: "InstructionSteps",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
