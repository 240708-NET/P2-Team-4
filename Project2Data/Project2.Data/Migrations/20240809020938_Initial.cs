using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enemies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Attributes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttackUnarmed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttackList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefenseArmor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Health = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HealthDice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proficiency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enemies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Attributes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttackUnarmed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttackList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefenseArmor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Health = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HealthDice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proficiency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Combats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActorEnemyId = table.Column<int>(type: "int", nullable: false),
                    EnemyACLow = table.Column<int>(type: "int", nullable: false),
                    EnemyACHigh = table.Column<int>(type: "int", nullable: false),
                    EnemyACRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActorPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Combats_Enemies_ActorEnemyId",
                        column: x => x.ActorEnemyId,
                        principalTable: "Enemies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Combats_Players_ActorPlayerId",
                        column: x => x.ActorPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ActorPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Players_ActorPlayerId",
                        column: x => x.ActorPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ActorPlayerId",
                table: "Inventories",
                column: "ActorPlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_InventoryId",
                table: "Items",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                table: "Players",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Combats");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Enemies");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
