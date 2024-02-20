using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalogs.API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vendor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_item_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "brand",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Starbucks" },
                    { 2, "Apple" },
                    { 3, "Nike" },
                    { 4, "CodeMaze" },
                    { 5, "Lego" }
                });

            migrationBuilder.InsertData(
                table: "type",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Learning" },
                    { 2, "Electronics" },
                    { 3, "Clothes" },
                    { 4, "Food" },
                    { 5, "Software" }
                });

            migrationBuilder.InsertData(
                table: "vendor",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Amazon" },
                    { 2, "Walmart" },
                    { 3, "Starbucks" },
                    { 4, "Netflix" },
                    { 5, "MediaMarkt" }
                });

            migrationBuilder.InsertData(
                table: "item",
                columns: new[] { "Id", "BrandId", "ImageUrl", "Name", "Price", "Stock", "TypeId", "VendorId" },
                values: new object[,]
                {
                    { 1, 2, "headphones.jpg", "Headphones", 49.99m, 15, 2, 5 },
                    { 2, 4, "book.jpg", "Book", 19.99m, 50, 1, 1 },
                    { 3, 1, "mug.jpg", "Coffee Mug", 9.99m, 20, 4, 3 },
                    { 4, 3, "tshirt.jpg", "T-Shirt", 14.99m, 30, 3, 1 },
                    { 5, 2, "headphones.jpg", "Headphones", 49.99m, 15, 2, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_BrandId",
                table: "item",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_item_TypeId",
                table: "item",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_item_VendorId",
                table: "item",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "brand");

            migrationBuilder.DropTable(
                name: "type");

            migrationBuilder.DropTable(
                name: "vendor");
        }
    }
}
