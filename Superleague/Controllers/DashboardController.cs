using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Helpers;
using Superleague.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Superleague.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserHelper _userHelper;
        private readonly UserManager<User> _userManager;
        private readonly IStaffRepository _staffRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IGlobalStatsRepository _globalStatsRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IRoundRepository _roundRepository;

        public DashboardController(ITeamRepository teamRepository, IPlayerRepository playerRepository, IUserHelper userHelper,
            UserManager<User> userManager, IStaffRepository staffRepository, IMatchRepository matchRepository, 
            IStatisticsRepository statisticsRepository, IGlobalStatsRepository globalStatsRepository, IResultRepository resultRepository,
            IRoundRepository roundRepository)
        {
            _teamRepository = teamRepository;
            _userHelper = userHelper;
            _userManager = userManager;
            _staffRepository = staffRepository;
            _matchRepository = matchRepository;
            _statisticsRepository = statisticsRepository;
            _globalStatsRepository = globalStatsRepository;
            _resultRepository = resultRepository;
            _roundRepository = roundRepository;
            _playerRepository = playerRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            DashboardViewModel dashboardViewModel = new()
            {
                TeamsList = _teamRepository.GetAll().Include("Country").OrderBy(e => e.Name),

                TotalTeams = _teamRepository.GetAll().Count(),

                UsersList = _userManager.Users.OrderBy(e => e.FirstName),

                TotalUsers = _userManager.Users.Count(),

                PlayersList = _playerRepository.GetAll().Include("Team").OrderBy(e => e.Name),

                TotalPlayers = _playerRepository.GetAll().Count(),

                StaffsList = _staffRepository.GetAll().Include("Team").OrderBy(e => e.Name),

                TotalStaffs = _staffRepository.GetAll().Count(),

                Rounds = _roundRepository.GetAll().OrderBy(e => e.Description),

                Matches = _matchRepository.GetAll().Include("Round").Include("HomeTeam").Include("AwayTeam").OrderByDescending(e => e.Round.Description),
            };

            return View(dashboardViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CloseRound(int id)
        {
            var round = _roundRepository.GetById(id);

            round.Closed = true;

            await _roundRepository.UpdateAsync(round);

            TempData["success"] = $"Round closed";

            return RedirectToAction("Index");
        }
    }
}
