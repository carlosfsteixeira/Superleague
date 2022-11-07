using Microsoft.EntityFrameworkCore.Migrations;

namespace Superleague.Migrations
{
    public partial class AddFlagPlayedToMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Played",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Description",
                table: "Positions",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Functions_Description",
                table: "Functions",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Positions_Description",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Functions_Description",
                table: "Functions");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Name",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Played",
                table: "Matches");
        }
    }
}
