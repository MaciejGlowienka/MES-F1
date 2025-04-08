using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class AddEstimateDurationToInstructionStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstimatedDurationMinutes",
                table: "InstructionSteps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 1,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 2,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 3,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 4,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 5,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 6,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 7,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 8,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 9,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 10,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 11,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 12,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 13,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 14,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 15,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 16,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 17,
                column: "EstimatedDurationMinutes",
                value: 60);

            migrationBuilder.UpdateData(
                table: "InstructionSteps",
                keyColumn: "InstructionStepId",
                keyValue: 18,
                column: "EstimatedDurationMinutes",
                value: 60);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedDurationMinutes",
                table: "InstructionSteps");
        }
    }
}
