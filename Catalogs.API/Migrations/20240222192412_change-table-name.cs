using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalogs.API.Migrations
{
    /// <inheritdoc />
    public partial class changetablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_item_type_TypeId",
                table: "item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_type",
                table: "type");

            migrationBuilder.RenameTable(
                name: "type",
                newName: "item_type");

            migrationBuilder.AddPrimaryKey(
                name: "PK_item_type",
                table: "item_type",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_item_item_type_TypeId",
                table: "item",
                column: "TypeId",
                principalTable: "item_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_item_item_type_TypeId",
                table: "item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_item_type",
                table: "item_type");

            migrationBuilder.RenameTable(
                name: "item_type",
                newName: "type");

            migrationBuilder.AddPrimaryKey(
                name: "PK_type",
                table: "type",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_item_type_TypeId",
                table: "item",
                column: "TypeId",
                principalTable: "type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
