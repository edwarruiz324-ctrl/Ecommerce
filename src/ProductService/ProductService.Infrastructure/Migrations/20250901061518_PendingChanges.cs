using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: -1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Price", "Stock", "UpdatedAt" },
                values: new object[,]
                {
                    { -4, new DateTime(2025, 9, 1, 5, 40, 0, 820, DateTimeKind.Utc).AddTicks(4588), "Monitor Gamer", "Monitor Gamer", 50m, 25, new DateTime(2025, 9, 1, 5, 40, 0, 820, DateTimeKind.Utc).AddTicks(4589) },
                    { -3, new DateTime(2025, 9, 1, 5, 40, 0, 820, DateTimeKind.Utc).AddTicks(4587), "Laptop Gamer", "Mouse Inalámbrico", 50m, 25, new DateTime(2025, 9, 1, 5, 40, 0, 820, DateTimeKind.Utc).AddTicks(4588) },
                    { -2, new DateTime(2025, 9, 1, 5, 40, 0, 820, DateTimeKind.Utc).AddTicks(4586), "Laptop Gamer", "Teclado Mecánico", 100m, 200, new DateTime(2025, 9, 1, 5, 40, 0, 820, DateTimeKind.Utc).AddTicks(4586) },
                    { -1, new DateTime(2025, 9, 1, 5, 40, 0, 820, DateTimeKind.Utc).AddTicks(3979), "Laptop Gamer", "Laptop Gamer", 1500m, 100, new DateTime(2025, 9, 1, 5, 40, 0, 820, DateTimeKind.Utc).AddTicks(3984) }
                });
        }
    }
}
