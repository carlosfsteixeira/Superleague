using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Superleague.Controllers
{
    [AllowAnonymous]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IResultRepository _resultRepository;

        public StatisticsController(IStatisticsRepository statisticsRepository, ITeamRepository teamRepository, IResultRepository resultRepository)
        {
            _statisticsRepository = statisticsRepository;
            _teamRepository = teamRepository;
            _resultRepository = resultRepository;
        }

        // GET: Statistics
        public async Task<IActionResult> IndexAsync()
        {
            var stats = await _statisticsRepository.GetAll().ToListAsync();

            await _statisticsRepository.RemoveRangeAsync(stats);

            await CalculateStatistics();

            var statistics = await _statisticsRepository.GetAll().Include(t => t.Team)
            .OrderByDescending(s => s.Points).ThenByDescending(s => s.Wins).ThenByDescending(s => s.GoalsScored).ToListAsync();

            int positionCounter = 1;

            foreach (var statistic in statistics)
            {
                if (statistic.Position == 0)
                {
                    statistic.Position = positionCounter++;
                }
            }

            return View(statistics);
        }

        public async Task CalculateStatistics()
        {
            var teams = _teamRepository.GetAll().ToList();

            foreach (var team in teams)
            {
                Statistics statistics = new Statistics();

                statistics.TeamId = team.Id;

                var results = _resultRepository.GetAll().Where(u => u.AwayTeamId == statistics.TeamId || u.HomeTeamId == statistics.TeamId);

                if(results.Any())
                {
                    //calculate statistics of team (visiting each result)
                    foreach (Result result in results)
                    {
                        //if team is HomeTeam (plays at home)
                        if (result.HomeTeamId == statistics.TeamId)
                        {
                            statistics.GoalsScored += result.HomeGoals;
                            statistics.TotalYellows += result.HomeYellowCards;
                            statistics.TotalReds += result.HomeRedCards;
                        }
                        else if (result.AwayTeamId == statistics.TeamId)
                        {
                            statistics.GoalsScored += result.AwayGoals;
                            statistics.TotalYellows += result.AwayYellowCards;
                            statistics.TotalReds += result.AwayRedCards;
                        }

                        statistics.GoalsConceded += (result.HomeGoals + result.AwayGoals) - statistics.GoalsScored;

                        if (statistics.GoalsConceded < statistics.GoalsScored)
                        {
                            statistics.Wins++;
                            statistics.Points += 3;
                        }
                        else if (statistics.GoalsConceded == statistics.GoalsScored)
                        {
                            statistics.Draws++;
                            statistics.Points += 1;
                        }
                        else
                        {
                            statistics.Losses++;
                        }

                        statistics.TotalMatches++;

                        statistics.GoalAverage = statistics.GoalsScored / statistics.TotalMatches;
                    }

                    await _statisticsRepository.CreateAsync(statistics);
                }
                
            }
        }

    }
}
