using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MES_F1.Migrations
{
    /// <inheritdoc />
    public partial class NoDefaultUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-admin-id", "admin-id-123" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "role-director-id", "director-id-456" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-id-123");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "director-id-456");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Birthday", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "admin-id-123", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", "admin@example.com", true, false, null, " ", "ADMIN@EXAMPLE.COM", "DEFAULTADMIN", "61NAlDODEU7eHVwDI4SKrlPl-UepNOB6jWkU_7nAYks=", null, false, "aaaaaaaa - aaaa - aaaa - aaaa - aaaaaaaaaaaa", " ", false, "defaultadmin" },
                    { "director-id-456", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dddddddd-dddd-dddd-dddd-dddddddddddd", "director@example.com", true, false, null, " ", "DIRECTOR@EXAMPLE.COM", "DEFAULTDIRECTOR", "61NAlDODEU7eHVwDI4SKrlPl-UepNOB6jWkU_7nAYks=", null, false, "cccccccc-cccc-cccc-cccc-cccccccccccc", " ", false, "defaultdirector" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "role-admin-id", "admin-id-123" },
                    { "role-director-id", "director-id-456" }
                });
        }
    }
}
