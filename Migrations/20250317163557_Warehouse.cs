using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class Warehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "TeamId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "TeamId",
                keyValue: 2);

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.MaterialsId);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    PartsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.PartsId);
                    table.ForeignKey(
                        name: "FK_Parts_Productions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "Productions",
                        principalColumn: "ProductionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WharehouseSpot",
                columns: table => new
                {
                    WharehouseSpotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WharehouseSpotNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WharehouseSpot", x => x.WharehouseSpotId);
                });

            migrationBuilder.CreateTable(
                name: "MaterialLocation",
                columns: table => new
                {
                    MaterialLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    MaterialLocationMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseSpotId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialLocation", x => x.MaterialLocationId);
                    table.ForeignKey(
                        name: "FK_MaterialLocation_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialLocation_WharehouseSpot_WarehouseSpotId",
                        column: x => x.WarehouseSpotId,
                        principalTable: "WharehouseSpot",
                        principalColumn: "WharehouseSpotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartLocation",
                columns: table => new
                {
                    PartLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    PartLocationMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseSpotId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartLocation", x => x.PartLocationId);
                    table.ForeignKey(
                        name: "FK_PartLocation_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartLocation_WharehouseSpot_WarehouseSpotId",
                        column: x => x.WarehouseSpotId,
                        principalTable: "WharehouseSpot",
                        principalColumn: "WharehouseSpotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialLocation_MaterialId",
                table: "MaterialLocation",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialLocation_WarehouseSpotId",
                table: "MaterialLocation",
                column: "WarehouseSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_PartLocation_PartId",
                table: "PartLocation",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_PartLocation_WarehouseSpotId",
                table: "PartLocation",
                column: "WarehouseSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_ProductionId",
                table: "Parts",
                column: "ProductionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialLocation");

            migrationBuilder.DropTable(
                name: "PartLocation");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "WharehouseSpot");

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "TeamId", "TeamName", "TeamWorkScope" },
                values: new object[,]
                {
                    { 1, "Development Team", "None" },
                    { 2, "Marketing Team", "None" }
                });
        }
    }
}
