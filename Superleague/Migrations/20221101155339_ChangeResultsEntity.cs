using Microsoft.EntityFrameworkCore.Migrations;

namespace Superleague.Migrations
{
    public partial class ChangeResultsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Results_AwayTeamId",
                table: "Results",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_HomeTeamId",
                table: "Results",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Teams_AwayTeamId",
                table: "Results",
                column: "AwayTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Teams_HomeTeamId",
                table: "Results",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Teams_AwayTeamId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Teams_HomeTeamId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_AwayTeamId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_HomeTeamId",
                table: "Results");
        }
    }
}
