using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Helpers;
using Superleague.Models;
using static System.Net.WebRequestMethods;

namespace Superleague.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PlayersController(IPlayerRepository playerRepository,
                                    ITeamRepository teamRepository,
                                    ICountryRepository countryRepository,
                                    IPositionRepository positionRepository,
                                    IImageHelper imageHelper,
                                    IWebHostEnvironment hostEnvironment)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _countryRepository = countryRepository;
            _positionRepository = positionRepository;
            _imageHelper = imageHelper;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Players
        public IActionResult Index()
        {
            var players = _playerRepository.GetAll().OrderBy(e => e.Name).Include(p => p.Country).Include(p => p.Position).Include(p => p.Team);

            return View(players);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerViewModel model, IFormFile? file, int id)
        {
            model.Player.TeamId = id;

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();

                    var upload = Path.Combine(wwwRootPath, @"images\players");

                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    model.Player.ImageURL = @"\images\players\" + fileName + extension;
                }

                var playerNumberExists = _playerRepository.GetAll().Where(t => t.Number == model.Player.Number).Count();

                if (playerNumberExists >= 1)
                {
                    ModelState.AddModelError("Player.Number", "This number is already in use");

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

                    return View(playerViewModel);
                }
                else
                {
                    await _playerRepository.CreateAsync(model.Player);

                    TempData["success"] = $"New player added";

                    var idiscas = model.Player.TeamId;

                    return RedirectToAction("Edit", "Teams", new { id = model.Player.TeamId });
                }
            }

            return View(model);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PlayerNotFound");
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
                return new NotFoundViewResult("PlayerNotFound");
            }

            return View(playerViewModel);
        }

        // POST: Players/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlayerViewModel model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString();

                        var upload = Path.Combine(wwwRootPath, @"images\players");

                        var extension = Path.GetExtension(file.FileName);

                        if (model.Player.ImageURL != null)
                        {
                            var currentImagePath = Path.Combine(wwwRootPath, model.Player.ImageURL.TrimStart('\\'));

                            if (System.IO.File.Exists(currentImagePath))
                            {
                                System.IO.File.Delete(currentImagePath);
                            }
                        }

                        using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                        }

                        model.Player.ImageURL = @"\images\players\" + fileName + extension;
                    }

                    var playerFromBD = await _playerRepository.GetByIdAsync(model.Player.Id);

                    if (playerFromBD.Name != model.Player.Name)
                    {
                        playerFromBD.Name = model.Player.Name;
                    }

                    if (playerFromBD.Number != model.Player.Number)
                    {
                        var playerNumberExists = _playerRepository.GetAll().Where(t => t.Number == model.Player.Number).Count();

                        if (playerNumberExists >= 1)
                        {
                            ModelState.AddModelError("Player.Number", "This number is already in use");

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

                            return View(playerViewModel);
                        }

                        playerFromBD.Number = model.Player.Number;
                    }

                    if (playerFromBD.PositionId != model.Player.PositionId)
                    {
                        playerFromBD.PositionId = model.Player.PositionId;
                    }

                    if (playerFromBD.CountryId != model.Player.CountryId)
                    {
                        playerFromBD.CountryId = model.Player.CountryId;
                    }

                    await _playerRepository.UpdateAsync(model.Player);

                    TempData["success"] = $"{model.Player.Name} updated";

                    return RedirectToAction("Edit", "Teams", new { id = model.Player.TeamId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _playerRepository.ExistAsync(model.Player.Id))
                    {
                        return new NotFoundViewResult("PlayerNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
              
            }

            return View(model);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int playerid)
        {
            var player = await _playerRepository.GetByIdAsync(playerid);

            await _playerRepository.DeleteAsync(player);

            TempData["success"] = $"{player.Name} removed";

            return RedirectToAction("Edit", "Teams", new { id = player.TeamId });
        }

        // POST: Players/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteFromTable(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);

            await _playerRepository.DeleteAsync(player);

            TempData["success"] = $"{player.Name} removed";

            return RedirectToAction("Index");
        }

        public IActionResult PlayerNotFound()
        {
            return View();
        }
    }
}
