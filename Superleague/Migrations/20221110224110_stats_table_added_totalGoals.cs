using Microsoft.EntityFrameworkCore.Migrations;



namespace Superleague.Migrations
{
    public partial class stats_table_added_totalGoals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalGoals",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalGoals",
                table: "Statistics");
        }
    }
}
