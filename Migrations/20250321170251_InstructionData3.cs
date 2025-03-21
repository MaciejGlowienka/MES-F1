using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class InstructionData3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StepWorkScope",
                table: "InstructionSteps",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 1,
                column: "StepWorkScope",
                value: "CompositeLaying");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 2,
                column: "StepWorkScope",
                value: "OperatingMachines");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 3,
                column: "StepWorkScope",
                value: "Painting");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 4,
                column: "StepWorkScope",
                value: "CompositeLaying");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 5,
                column: "StepWorkScope",
                value: "OperatingMachines");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 6,
                column: "StepWorkScope",
                value: "Painting");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 7,
                column: "StepWorkScope",
                value: "CompositeLaying");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 8,
                column: "StepWorkScope",
                value: "OperatingMachines");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 9,
                column: "StepWorkScope",
                value: "Painting");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 10,
                column: "StepWorkScope",
                value: "CompositeLaying");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 11,
                column: "StepWorkScope",
                value: "OperatingMachines");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 12,
                column: "StepWorkScope",
                value: "Painting");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 13,
                column: "StepWorkScope",
                value: "MetalForming");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 14,
                column: "StepWorkScope",
                value: "Welding");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 15,
                column: "StepWorkScope",
                value: "Welding");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 16,
                column: "StepWorkScope",
                value: "MetalForming");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 17,
                column: "StepWorkScope",
                value: "Welding");

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 18,
                column: "StepWorkScope",
                value: "Welding");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StepWorkScope",
                table: "InstructionSteps",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 1,
                column: "StepWorkScope",
                value: 3);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 2,
                column: "StepWorkScope",
                value: 7);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 3,
                column: "StepWorkScope",
                value: 6);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 4,
                column: "StepWorkScope",
                value: 3);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 5,
                column: "StepWorkScope",
                value: 7);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 6,
                column: "StepWorkScope",
                value: 6);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 7,
                column: "StepWorkScope",
                value: 3);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 8,
                column: "StepWorkScope",
                value: 7);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 9,
                column: "StepWorkScope",
                value: 6);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 10,
                column: "StepWorkScope",
                value: 3);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 11,
                column: "StepWorkScope",
                value: 7);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 12,
                column: "StepWorkScope",
                value: 6);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 13,
                column: "StepWorkScope",
                value: 1);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 14,
                column: "StepWorkScope",
                value: 8);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 15,
                column: "StepWorkScope",
                value: 8);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 16,
                column: "StepWorkScope",
                value: 1);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 17,
                column: "StepWorkScope",
                value: 8);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 18,
                column: "StepWorkScope",
                value: 8);
        }
    }
}
