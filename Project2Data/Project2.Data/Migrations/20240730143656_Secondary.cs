using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Secondary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttackList",
                table: "Enemies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AttackUnarmed",
                table: "Enemies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Attributes",
                table: "Enemies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DefenseArmor",
                table: "Enemies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Enemies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttackList",
                table: "Enemies");

            migrationBuilder.DropColumn(
                name: "AttackUnarmed",
                table: "Enemies");

            migrationBuilder.DropColumn(
                name: "Attributes",
                table: "Enemies");

            migrationBuilder.DropColumn(
                name: "DefenseArmor",
                table: "Enemies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Enemies");
        }
    }
}
