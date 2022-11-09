using Microsoft.EntityFrameworkCore.Migrations;

namespace Superleague.Migrations
{
    public partial class alter_table_statistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessRedCards",
                table: "GlobalStatistics");

            migrationBuilder.DropColumn(
                name: "LessYellowCards",
                table: "GlobalStatistics");

            migrationBuilder.DropColumn(
                name: "MostRedCards",
                table: "GlobalStatistics");

            migrationBuilder.DropColumn(
                name: "MostYellowCards",
                table: "GlobalStatistics");

            migrationBuilder.AddColumn<int>(
                name: "GoalAverage",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCards",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalAverage",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "TotalCards",
                table: "Statistics");

            migrationBuilder.AddColumn<string>(
                name: "LessRedCards",
                table: "GlobalStatistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LessYellowCards",
                table: "GlobalStatistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MostRedCards",
                table: "GlobalStatistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MostYellowCards",
                table: "GlobalStatistics",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
