using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Models;

namespace Superleague.Controllers
{
    [AllowAnonymous]
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
            await _globalStatsRepository.RefreshGlobalStatisticsTableAsync();

            GlobalStatsViewModel model = new() { };

            model.GlobalStats = await CalculateGlobalStatsAsync();

            if (model.GlobalStats != null)
            {
                await _globalStatsRepository.CreateAsync(model.GlobalStats);
            }

            var globalStatistics = await _globalStatsRepository.GetAll().FirstAsync();

            var teams = await _teamRepository.GetAll().OrderBy(e => e.Name).ToListAsync();

            model.GlobalStats = globalStatistics;

            model.Teams = teams;

            return View(model);
        }

        private async Task<GlobalStats> CalculateGlobalStatsAsync()
        {
            GlobalStats globalStats = new GlobalStats();

            var results = _resultRepository.GetAll();

            if (results.Any())
            {
                globalStats.TotalMatches = _resultRepository.GetAll().Count();

                // total goals
                var homeGoals = _resultRepository.GetAll().Sum(u => u.HomeGoals);

                var awayGoals = _resultRepository.GetAll().Sum(u => u.AwayGoals);

                globalStats.TotalGoals = homeGoals + awayGoals;

                // goal average (if totalMatches = 0, the quocient will be always 0)
                if (globalStats.TotalMatches == 0)
                {
                    globalStats.GoalAverage = 0;
                }
                else
                {
                    globalStats.GoalAverage = globalStats.TotalGoals / globalStats.TotalMatches;
                }

                // total yellow cards
                var homeYellows = _resultRepository.GetAll().Sum(u => u.HomeYellowCards);

                var awayYellows = _resultRepository.GetAll().Sum(u => u.AwayYellowCards);

                globalStats.TotalYellowCards = homeYellows + awayYellows;

                // total red cards
                var homeReds = _resultRepository.GetAll().Sum(u => u.HomeRedCards);

                var awayReds = _resultRepository.GetAll().Sum(u => u.AwayRedCards);

                globalStats.TotalRedCards = homeReds + awayReds;

                // results divided
                globalStats.HomeWins = _resultRepository.GetAll().Where(u => u.HomeGoals > u.AwayGoals).Count();

                globalStats.AwayWins = _resultRepository.GetAll().Where(u => u.AwayGoals > u.HomeGoals).Count();

                globalStats.Draws = _resultRepository.GetAll().Where(u => u.AwayGoals == u.HomeGoals).Count();

                await getBestAndWorstAttack(globalStats);

                await getBestAndWorstDefense(globalStats);

                await getMostAndLessWins(globalStats);

                await getMostAndLessDraws(globalStats);

                await getMostAndLessLosses(globalStats);
            }

            return globalStats;
        }

        public async Task getBestAndWorstAttack(GlobalStats globalStats)
        {
            // stats by team -> BEST ATTACK
            var maxGoalsScored = await _statisticsRepository.GetAll().MaxAsync(t => t.GoalsScored);
            var teamBestAttack = await _statisticsRepository.GetAll().Include(t => t.Team).Where(t => t.GoalsScored == maxGoalsScored).FirstAsync();
            globalStats.BestAttack = teamBestAttack.Team.Name;

            // stats by team -> WORST ATTACK
            var minGoalsScored = await _statisticsRepository.GetAll().MinAsync(t => t.GoalsScored);
            var teamWorstAttack = await _statisticsRepository.GetAll().Include(t => t.Team).Where(t => t.GoalsScored == minGoalsScored).FirstAsync();
            globalStats.WorstAttack = teamWorstAttack.Team.Name;
        }

        public async Task getBestAndWorstDefense(GlobalStats globalStats)
        {
            // stats by team -> BEST DEFENSE
            var minGoalsConceeded = await _statisticsRepository.GetAll().MinAsync(t => t.GoalsConceded);
            var teamBestDefense = await _statisticsRepository.GetAll().Include(t => t.Team).Where(t => t.GoalsConceded == minGoalsConceeded).FirstAsync();
            globalStats.BestDefence = teamBestDefense.Team.Name;

            // stats by team -> WORST DEFENSE
            int maxGoalsConceeded = _statisticsRepository.GetAll().MaxAsync(t => t.GoalsConceded).Result;
            var teamWorstDefense = _statisticsRepository.GetAll().Include(t => t.Team).Where(t => t.GoalsConceded == maxGoalsConceeded).First();
            globalStats.WorstDefence = teamWorstDefense.Team.Name;
        }

        public async Task getMostAndLessWins(GlobalStats globalStats)
        {
            // stats by team -> MOST WINS
            var maxWins = await _statisticsRepository.GetAll().MaxAsync(t => t.Wins);
            var teamMostWins = await _statisticsRepository.GetAll().Include(t => t.Team).Where(t => t.Wins == maxWins).FirstAsync();
            globalStats.MostWins = teamMostWins.Team.Name;

            // stats by team -> LESS WINS
            var minWins = await _statisticsRepository.GetAll().MinAsync(t => t.Wins);
            var teamMinWins = await _statisticsRepository.GetAll().Include(t => t.Team).Where(t => t.Wins == minWins).FirstAsync();
            globalStats.LessWins = teamMinWins.Team.Name;
        }

        public async Task getMostAndLessDraws(GlobalStats globalStats)
        {
            // stats by team -> LESS DRAWS
            var lessDraws = await _statisticsRepository.GetAll().MinAsync(t => t.Draws);
            var teamlessDraws = await _statisticsRepository.GetAll().Include(t => t.Team).Where(t => t.Draws == lessDraws).FirstAsync();
            globalStats.LessDraws = teamlessDraws.Team.Name;

            // stats by team -> MOST DRAWS
            var mostDraws = await _statisticsRepository.GetAll().MaxAsync(t => t.Draws);
            var teamMostDraws = await _statisticsRepository.GetAll().Include(t => t.Team).Where(t => t.Draws == mostDraws).FirstAsync();
            globalStats.MostDraws = teamMostDraws.Team.Name;
        }

        public async Task getMostAndLessLosses(GlobalStats globalStats)
        {
            // stats by team -> LESS LOSSES
            var lessLosses = await _statisticsRepository.GetAll().MinAsync(t => t.Losses);
            var teamlessLosses = await _statisticsRepository.GetAll().Include(t => t.Team).Where(t => t.Losses == lessLosses).FirstAsync();
            globalStats.LessDefeats = teamlessLosses.Team.Name;

            // stats by team -> MOST LOSSES
            int mostLosses = _statisticsRepository.GetAll().MaxAsync(t => t.Losses).Result;
            var teamMostLosses = _statisticsRepository.GetAll().Include(t => t.Team).Where(t => t.Losses == mostLosses).First();
            globalStats.MostDefeats = teamMostLosses.Team.Name;
        }
    }
}
