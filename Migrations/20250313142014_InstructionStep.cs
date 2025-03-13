using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class InstructionStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstructionSteps",
                columns: table => new
                {
                    InstructionStepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructionId = table.Column<int>(type: "int", nullable: false),
                    InstructionStepNumber = table.Column<int>(type: "int", nullable: false),
                    StepWorkScope = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    InstructionStepDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructionSteps", x => x.InstructionStepId);
                    table.ForeignKey(
                        name: "FK_InstructionSteps_Instructions_InstructionId",
                        column: x => x.InstructionId,
                        principalTable: "Instructions",
                        principalColumn: "InstructionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructionSteps_InstructionId",
                table: "InstructionSteps",
                column: "InstructionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstructionSteps");
        }
    }
}
