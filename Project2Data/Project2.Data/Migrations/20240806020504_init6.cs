using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project2.Data.Migrations
{
    /// <inheritdoc />
    public partial class init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActorPlayerId",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ActorPlayerId",
                table: "Inventories",
                column: "ActorPlayerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Players_ActorPlayerId",
                table: "Inventories",
                column: "ActorPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Players_ActorPlayerId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_ActorPlayerId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "ActorPlayerId",
                table: "Inventories");
        }
    }
}
