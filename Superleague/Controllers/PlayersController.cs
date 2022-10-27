using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Helpers;
using Superleague.Models;

namespace Superleague.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IImageHelper _imageHelper;

        public PlayersController(IPlayerRepository playerRepository,
                                    ITeamRepository teamRepository,
                                    ICountryRepository countryRepository,
                                    IPositionRepository positionRepository,
                                    IImageHelper imageHelper)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _countryRepository = countryRepository;
            _positionRepository = positionRepository;
            _imageHelper = imageHelper;
        }

        // GET: Players
        public IActionResult Index()
        {
            return View(_playerRepository.GetAll().OrderBy(e => e.Name).Include(p => p.Country).Include(p => p.Position).Include(p => p.Team));

            //var playerList = _playerRepository.GetAll().Include("Country.Position.Team").OrderBy(e => e.Name);

            //return View(playerList);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            PlayerViewModel playerViewModel = new()
            {
                Player = new(),

                TeamList = _teamRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),

                PositionList = _positionRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Description,
                    Value = i.Id.ToString(),
                }),

                CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
            };

            return View(playerViewModel);
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerViewModel model, int id)
        {
            model.Player.TeamId = id;

            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "players");

                    model.Player.ImageURL = path;
                }

                await _playerRepository.CreateAsync(model.Player);

                return RedirectToAction("Index", new { id = model.TeamId });
            }

            return View(model);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PlayerViewModel playerViewModel = new()
            {
                Player = new(),

                PositionList = _positionRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Description,
                    Value = i.Id.ToString(),
                }),
                CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
            };

            playerViewModel.Player = await _playerRepository.GetByIdAsync(id.Value);

            if (playerViewModel == null)
            {
                return NotFound();
            }

            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", player.CountryId);
            //ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Description", player.PositionId);
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", player.TeamId);

            return View(playerViewModel);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlayerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.Player.ImageURL;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "players");
                    }

                    await _playerRepository.UpdateAsync(model.Player);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _playerRepository.ExistAsync(model.Player.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", player.CountryId);
            //ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Description", player.PositionId);
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", player.TeamId);

            return View(model);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetByIdAsync(id.Value);

            //var player = await _context.Players
            //    .Include(p => p.Country)
            //    .Include(p => p.Position)
            //    .Include(p => p.Team)
            //    .FirstOrDefaultAsync(m => m.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);

            await _playerRepository.DeleteAsync(player);

            return RedirectToAction(nameof(Index));
        }
    }
}
