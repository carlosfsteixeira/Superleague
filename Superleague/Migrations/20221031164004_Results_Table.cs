using Microsoft.EntityFrameworkCore.Migrations;

namespace Superleague.Migrations
{
    public partial class Results_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalMatches = table.Column<int>(type: "int", nullable: false),
                    TotalGoals = table.Column<int>(type: "int", nullable: false),
                    GoalAverage = table.Column<double>(type: "float", nullable: false),
                    HomeWins = table.Column<double>(type: "float", nullable: false),
                    AwayWins = table.Column<double>(type: "float", nullable: false),
                    Draws = table.Column<double>(type: "float", nullable: false),
                    TotalYellowCards = table.Column<int>(type: "int", nullable: false),
                    TotalRedCards = table.Column<int>(type: "int", nullable: false),
                    BestAttack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorstAttack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BestDefence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorstDefence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MostWins = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessWins = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MostDefeats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessDefeats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MostDraws = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessDraws = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MostYellowCards = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessYellowCards = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MostRedCards = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessRedCards = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeGoals = table.Column<int>(type: "int", nullable: false),
                    AwayGoals = table.Column<int>(type: "int", nullable: false),
                    HomeTeamId = table.Column<int>(type: "int", nullable: true),
                    AwayTeamId = table.Column<int>(type: "int", nullable: true),
                    RoundId = table.Column<int>(type: "int", nullable: true),
                    HomeYellowCards = table.Column<int>(type: "int", nullable: false),
                    AwayYellowCards = table.Column<int>(type: "int", nullable: false),
                    HomeRedCards = table.Column<int>(type: "int", nullable: false),
                    AwayRedCards = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Results_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Results_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Results_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results_AwayTeamId",
                table: "Results",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_HomeTeamId",
                table: "Results",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_MatchId",
                table: "Results",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_RoundId",
                table: "Results",
                column: "RoundId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalStatistics");

            migrationBuilder.DropTable(
                name: "Results");
        }
    }
}
