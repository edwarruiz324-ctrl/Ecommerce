using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductService.Infrastructure.Migrations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
