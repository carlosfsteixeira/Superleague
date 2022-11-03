using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Superleague.Controllers
{
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
        public IActionResult Index()
        {
            //RefreshStatisticsTable();
            CalculateStatistics();

            var stats = _statisticsRepository.GetAll().Include(t => t.Team)
                .OrderByDescending(s => s.Points)
                .OrderByDescending(s => s.Wins)
                .OrderByDescending(s => s.GoalsScored);

            int positionCounter = 1;

            foreach (var statistic in stats)
            {
                if (statistic.Position == 0)
                {
                    statistic.Position = positionCounter++;
                }
            }

            return View(stats);
        }

        public async void CalculateStatistics()
        {
            var teams = _teamRepository.GetAll();

            StatisticsViewModel model = new() { };

            foreach (var team in teams)
            {
                model.Statistics = new Statistics
                {
                    TeamId = team.Id,
                };
                //

                var results = _resultRepository.GetAll().Where(u => u.AwayTeamId == model.Statistics.TeamId || u.HomeTeamId == model.Statistics.TeamId);

                //calculate statistics of team (visiting each result)
                foreach (Result result in results)
                {
                    //if team is HomeTeam (plays at home)
                    if (result.HomeTeamId == model.Statistics.TeamId)
                    {
                        model.Statistics.GoalsScored += result.HomeGoals;
                    }
                    else if (result.AwayTeamId == model.Statistics.TeamId)
                    {
                        model.Statistics.GoalsScored += result.AwayGoals;
                    }

                    model.Statistics.GoalsConceded += (result.HomeGoals + result.AwayGoals) - model.Statistics.GoalsScored;

                    if (model.Statistics.GoalsConceded < model.Statistics.GoalsScored)
                    {
                        model.Statistics.Wins++;
                        model.Statistics.Points += 3;
                    }
                    else if (model.Statistics.GoalsConceded == model.Statistics.GoalsScored)
                    {
                        model.Statistics.Draws++;
                        model.Statistics.Points += 1;
                    }
                    else
                    {
                        model.Statistics.Losses++;
                    }

                    model.Statistics.TotalMatches++;
                }
                //
                if (ModelState.IsValid)
                {
                    _statisticsRepository.Create(model.Statistics);
                }
            }
        }

        public void RefreshStatisticsTable()
        {
            var stats = _statisticsRepository.GetAll();

            _statisticsRepository.RemoveRangeAsync(stats);
        }

    }
}
