using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project2.Data.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_InventoryId",
                table: "Items",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Inventories_InventoryId",
                table: "Items",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Inventories_InventoryId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_InventoryId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Inventories");
        }
    }
}
