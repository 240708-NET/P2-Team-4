using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project2.Data.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Combats_ActorEnemyId",
                table: "Combats",
                column: "ActorEnemyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Combats_ActorPlayerId",
                table: "Combats",
                column: "ActorPlayerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Combats_Enemies_ActorEnemyId",
                table: "Combats",
                column: "ActorEnemyId",
                principalTable: "Enemies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Combats_Players_ActorPlayerId",
                table: "Combats",
                column: "ActorPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Combats_Enemies_ActorEnemyId",
                table: "Combats");

            migrationBuilder.DropForeignKey(
                name: "FK_Combats_Players_ActorPlayerId",
                table: "Combats");

            migrationBuilder.DropIndex(
                name: "IX_Combats_ActorEnemyId",
                table: "Combats");

            migrationBuilder.DropIndex(
                name: "IX_Combats_ActorPlayerId",
                table: "Combats");
        }
    }
}
