using Microsoft.EntityFrameworkCore.Migrations;

namespace Superleague.Migrations
{
    public partial class AddFlagHasResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasResult",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasResult",
                table: "Matches");
        }
    }
}
