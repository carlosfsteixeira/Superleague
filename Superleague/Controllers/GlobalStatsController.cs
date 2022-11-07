using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Models;

namespace Superleague.Controllers
{
    public class GlobalStatsController : Controller
    {
        private readonly IGlobalStatsRepository _globalStatsRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly ITeamRepository _teamRepository;

        public GlobalStatsController(IGlobalStatsRepository globalStatsRepository, 
            IResultRepository resultRepository, 
            IStatisticsRepository statisticsRepository,
            ITeamRepository teamRepository)
        {
            _globalStatsRepository = globalStatsRepository;
            _resultRepository = resultRepository;
            _statisticsRepository = statisticsRepository;
            _teamRepository = teamRepository;
        }

        // GET: GlobalStats
        public async Task<IActionResult> Index()
        {
            RefreshGlobalStatisticsTable();

            GlobalStatsViewModel model = new() { };

            model.GlobalStats = new GlobalStats();

            CalculateGlobalStats(model);

            if (ModelState.IsValid)
            {
                await _globalStatsRepository.CreateAsync(model.GlobalStats);
            }

            var globalStatistics = _globalStatsRepository.GetAll();

            return View(globalStatistics);

        }

        private void CalculateGlobalStats(GlobalStatsViewModel model)
        {
            model.GlobalStats.TotalMatches = _resultRepository.GetAll().Count();

            var homeGoals = _resultRepository.GetAll().Sum(u => u.HomeGoals);

            var awayGoals = _resultRepository.GetAll().Sum(u => u.AwayGoals);

            model.GlobalStats.TotalGoals = homeGoals + awayGoals;

            model.GlobalStats.GoalAverage = model.GlobalStats.TotalGoals / model.GlobalStats.TotalMatches;

            model.GlobalStats.HomeWins = _resultRepository.GetAll().Where(u => u.HomeGoals > u.AwayGoals).Count();

            model.GlobalStats.AwayWins = _resultRepository.GetAll().Where(u => u.AwayGoals > u.HomeGoals).Count();

            model.GlobalStats.Draws = _resultRepository.GetAll().Where(u => u.AwayGoals == u.HomeGoals).Count();

            int best = (int)_statisticsRepository.GetAll().Max(u => u.GoalsScored);

            var statistic = _statisticsRepository.GetAll().Where(u => u.GoalsScored == best);

            //model.GlobalStats.BestAttack = statistic.Team.Name;
        }

        private void RefreshGlobalStatisticsTable()
        {
            var globalStats = _globalStatsRepository.GetAll();

            //_globalStatsRepository.RemoveRange(globalStats);
        }
    }
}
