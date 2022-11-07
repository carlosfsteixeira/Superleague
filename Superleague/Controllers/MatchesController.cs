using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Helpers;
using Superleague.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Superleague.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IRoundRepository _roundRepository;

        public MatchesController(IMatchRepository matchRepository, ITeamRepository teamRepository, IRoundRepository roundRepository)
        {
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
            _roundRepository = roundRepository;
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {
            var matches = await _matchRepository.GetAll().Include(m => m.AwayTeam).Include(m => m.HomeTeam).Include(m => m.Round).ToListAsync();

            await VerifyIfMatchPlayed(matches);

            return View(matches);
        }

        public async Task VerifyIfMatchPlayed(List<Match> matches)
        {
            foreach (var match in matches)
            {
                if (match.MatchDate <= DateTime.Now)
                {
                    match.Played = true;
                    _matchRepository.UpdateAsync(match);
                }
            }
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("MatchNotFound");
            }

            MatchViewModel matchViewModel = new()
            {
                Match = new(),

                RoundsList = _roundRepository.GetAll().Select(a =>
                            new SelectListItem
                            {
                                Text = a.Description,
                                Value = a.Id.ToString()
                            }),

                TeamNamesList = _teamRepository.GetAll().Select(a =>
                            new SelectListItem
                            {
                                Text = a.Name,
                                Value = a.Id.ToString()
                            }),
            };

            matchViewModel.Match = await _matchRepository.GetByIdAsync(id.Value);

            matchViewModel.Match.Round = await _roundRepository.GetByIdAsync(matchViewModel.Match.RoundId.Value);

            if (matchViewModel == null)
            {
                return new NotFoundViewResult("MatchNotFound");
            }

            return View(matchViewModel);
        }

        // GET: Matches/Create
        public IActionResult Create()
        {
            MatchViewModel matchViewModel = new()
            {
                Match = new(),

                RoundsList =  _roundRepository.GetAll().Where(t => t.Complete == false).Select(a =>
                            new SelectListItem
                            {
                                Text = a.Description,
                                Value = a.Id.ToString()
                            }),

                TeamNamesList = _teamRepository.GetAll().Select(a =>
                            new SelectListItem
                            {
                                Text = a.Name,
                                Value = a.Id.ToString()
                            }),
            };

            return View(matchViewModel);
        }

        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchViewModel model)
        {
            model.Rounds = await _roundRepository.GetAll().ToListAsync();

            if (ModelState.IsValid)
            {
                await _matchRepository.CreateAsync(model.Match);

                foreach(var round in model.Rounds)
                {
                    if (round.Id == model.Match.RoundId)
                    {
                        var countMatchsRound = _matchRepository.GetAll().Where(t => t.RoundId == model.Match.RoundId).Count();

                        if(countMatchsRound >= 4)
                        {
                            round.Complete = true;
                            await _roundRepository.UpdateAsync(round);
                        }
                    }
                }

                TempData["success"] = $"New fixture created";

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("MatchNotFound");
            }

            MatchViewModel matchViewModel = new()
            {
                Match = new(),

                RoundsList = _roundRepository.GetAll().Where(t => t.Complete == false).Select(a =>
                            new SelectListItem
                            {
                                Text = a.Description,
                                Value = a.Id.ToString()
                            }),

                TeamNamesList = _teamRepository.GetAll().Select(a =>
                            new SelectListItem
                            {
                                Text = a.Name,
                                Value = a.Id.ToString()
                            }),
            };

            matchViewModel.Match = await _matchRepository.GetByIdAsync(id.Value);

            var getRound = await _roundRepository.GetByIdAsync(matchViewModel.Match.RoundId.Value);

            matchViewModel.Match.Round = getRound;

            var getHomeTeam = await _teamRepository.GetByIdAsync(matchViewModel.Match.HomeTeamId.Value);

            matchViewModel.Match.HomeTeam = getHomeTeam;

            var getAwayTeam = await _teamRepository.GetByIdAsync(matchViewModel.Match.AwayTeamId.Value);

            matchViewModel.Match.AwayTeam = getAwayTeam;

            if (matchViewModel == null)
            {
                return new NotFoundViewResult("MatchNotFound");
            }

            return View(matchViewModel);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _matchRepository.UpdateAsync(model.Match);

                    TempData["success"] = $"Fixture updated";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _matchRepository.ExistAsync(model.Match.Id))
                    {
                        return new NotFoundViewResult("MatchNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(model);
        }

        //// GET: Matches/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new NotFoundViewResult("MatchNotFound");
        //    }

        //    var match = await _matchRepository.GetByIdAsync(id.Value);

        //    if (match == null)
        //    {
        //        return new NotFoundViewResult("MatchNotFound");
        //    }

        //    return View(match);
        //}

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int matchId = id;

            var match = await _matchRepository.GetByIdAsync(matchId);

            var rounds = await _roundRepository.GetAll().ToListAsync();

            foreach (var round in rounds)
            {
                if (round.Id == match.RoundId)
                {
                    var countMatchsRound = _matchRepository.GetAll().Where(t => t.RoundId == match.RoundId).Count();

                    if (countMatchsRound < 4)
                    {
                        round.Complete = false;
                        await _roundRepository.UpdateAsync(round);
                    }
                }
            }

            try
            {
                await _matchRepository.DeleteAsync(match);

                TempData["success"] = $"Fixture removed";
                
            }
            catch (Exception)
            {
                ViewBag.ErrorTitle = "This Match is in use";
                ViewBag.ErrorMessage = "Consider deleting all Results appended and try again.";
                return View("Error");
            }

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var matchProperties = _matchRepository.GetAll().Include(m => m.AwayTeam).Include(m => m.HomeTeam).Include(m => m.Round);

            return Json(new { data = matchProperties });
        }

        [HttpGet]
        public IActionResult GetMatchId(int id)
        {
            var matchProperties = _matchRepository.GetAll().Include(m => m.AwayTeam).Include(m => m.HomeTeam).Include(m => m.Round).Where(t => t.Id == id);

            return Json(new { data = matchProperties });
        }

        public IActionResult MatchNotFound()
        {
            return View();
        }

    }
}
