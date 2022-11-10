using Microsoft.EntityFrameworkCore.Migrations;

namespace Superleague.Migrations
{
    public partial class red_yellow_cards_stats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalCards",
                table: "Statistics",
                newName: "TotalYellows");

            migrationBuilder.AddColumn<int>(
                name: "TotalReds",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalReds",
                table: "Statistics");

            migrationBuilder.RenameColumn(
                name: "TotalYellows",
                table: "Statistics",
                newName: "TotalCards");
        }
    }
}
