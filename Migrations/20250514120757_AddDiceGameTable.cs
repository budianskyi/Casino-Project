using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddDiceGameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiceGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BetAmount = table.Column<int>(type: "int", nullable: false),
                    PlayerBet = table.Column<int>(type: "int", nullable: false),
                    NumberOfDice = table.Column<int>(type: "int", nullable: false),
                    TotalDiceResult = table.Column<int>(type: "int", nullable: false),
                    Coefficient = table.Column<int>(type: "int", nullable: false),
                    ResultMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiceGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiceGames_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiceGames_UserId",
                table: "DiceGames",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiceGames");
        }
    }
}
