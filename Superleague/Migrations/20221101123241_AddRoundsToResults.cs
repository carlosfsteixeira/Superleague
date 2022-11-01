using Microsoft.EntityFrameworkCore.Migrations;

namespace Superleague.Migrations
{
    public partial class AddRoundsToResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoundId",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Results_RoundId",
                table: "Results",
                column: "RoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Rounds_RoundId",
                table: "Results",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Rounds_RoundId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_RoundId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "RoundId",
                table: "Results");
        }
    }
}
