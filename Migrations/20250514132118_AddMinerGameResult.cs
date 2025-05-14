using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddMinerGameResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MinerGameResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BetAmount = table.Column<int>(type: "int", nullable: false),
                    WinAmount = table.Column<double>(type: "float", nullable: false),
                    IsWin = table.Column<bool>(type: "bit", nullable: false),
                    PlayedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinerGameResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MinerGameResults_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MinerGameResults_UserId",
                table: "MinerGameResults",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinerGameResults");
        }
    }
}
