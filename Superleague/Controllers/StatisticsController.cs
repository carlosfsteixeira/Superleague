using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Models;
using System.Linq;

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
            RefreshStatisticsTable();

            var teams = _teamRepository.GetAll();

            foreach (var team in teams)
            {
                CreateTeamStatistics(team);
            }

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

        private void CreateTeamStatistics(Team team)
        {
            StatisticsViewModel model = new() { };

            var statsTeam = _statisticsRepository.GetByIdAsync(team.Id);

            model.Statistics = new Statistics
            {
                TeamId = team.Id,
            };

            CalculateTeamStatistics(model);

            if (ModelState.IsValid)
            {
                _statisticsRepository.CreateAsync(model.Statistics);
            }
        }

        private void CalculateTeamStatistics(StatisticsViewModel model)
        {
            var results = _resultRepository.GetAll().Where(u => u.AwayTeam.Id == model.Statistics.TeamId || u.HomeTeam.Id == model.Statistics.TeamId);

            //calculate statistics of team (visiting each result)
            foreach (Result result in results)
            {
                //if team is HomeTeam (plays at home)
                if (result.HomeTeam.Id == model.Statistics.TeamId)
                {
                    model.Statistics.GoalsScored += result.HomeGoals;
                }
                else if (result.AwayTeam.Id == model.Statistics.TeamId)
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
        }

        public void RefreshStatisticsTable()
        {
            var stats = _statisticsRepository.GetAll();

            _statisticsRepository.RemoveRangeAsync(stats);
        }

    }
}
