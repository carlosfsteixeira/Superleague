using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Helpers;
using Superleague.Models;

namespace Superleague.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TeamsController(ITeamRepository teamRepository, 
                               ICountryRepository countryRepository, 
                               IPlayerRepository playerRepository, 
                               IStaffRepository staffRepository,
                               IStatisticsRepository statisticsRepository,
                               IImageHelper imageHelper,
                               IWebHostEnvironment hostEnvironment)
        {
            _teamRepository = teamRepository;
            _countryRepository = countryRepository;
            _playerRepository = playerRepository;
            _staffRepository = staffRepository;
            _statisticsRepository = statisticsRepository;
            _imageHelper = imageHelper;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Teams
        [AllowAnonymous]
        public IActionResult Index()
        {
            var teamList = _teamRepository.GetAll().Include("Country").OrderBy(e => e.Name);

            return View(teamList);
        }

        [AllowAnonymous]
        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("TeamNotFound");
            }

            TeamViewModel teamViewModel = new()
            {
                Team = new(),

                PlayerList = _playerRepository.GetAll().Where(e => e.TeamId == id).OrderBy(e => e.Name),

                StaffList = _staffRepository.GetAll().Where(e => e.TeamId == id).OrderBy(e => e.Name),

                CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),

                Statistics = _statisticsRepository.GetById(id.Value),
            };

            teamViewModel.Team = await _teamRepository.GetByIdAsync(id.Value);

            teamViewModel.Team.Country = await _countryRepository.GetByIdAsync(teamViewModel.Team.CountryId.Value);

            if (teamViewModel == null)
            {
                return new NotFoundViewResult("TeamNotFound");
            }

            return View(teamViewModel);
        }


        // GET: Teams/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            TeamViewModel teamViewModel = new TeamViewModel
            {
                CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
            };

            return View(teamViewModel);
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(TeamViewModel model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();

                    var upload = Path.Combine(wwwRootPath, @"images\teams");

                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    model.Team.ImageURL = @"\images\teams\" + fileName + extension;
                }

                var teamNameExists = _teamRepository.GetAll().Where(t => t.Name.ToLower() == model.Team.Name.ToLower()).Count();

                if (teamNameExists >= 1)
                {
                    ModelState.AddModelError("Team.Name", "This Team name is already in use");

                    TeamViewModel teamViewModel = new TeamViewModel
                    {
                        CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString(),
                        }),
                    };

                    return View(teamViewModel);
                }
                else
                {
                    await _teamRepository.CreateAsync(model.Team);

                    TempData["success"] = $"New team added";

                    return RedirectToAction(nameof(Index));
                }
            }
  
            return View(model);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("TeamNotFound");
            }

            TeamViewModel teamViewModel = new()
            {
                Team = new(),

                PlayerList = _playerRepository.GetAll().Include(p => p.Position).Include(p => p.Country).Where(e => e.TeamId == id).OrderBy(e => e.Name),

                StaffList = _staffRepository.GetAll().Include(p => p.Function).Include(p => p.Country).Where(e => e.TeamId == id).OrderBy(e => e.Name),

                CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),

                //Statistics = _context.Statistics.GetFirstOrDefault(u => u.Team.Id == id),
            };

            teamViewModel.Team = await _teamRepository.GetByIdAsync(id.Value);

            if (teamViewModel == null)
            {
                return new NotFoundViewResult("TeamNotFound");
            }
            
            return View(teamViewModel);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamViewModel model, IFormFile? file, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString();

                        var upload = Path.Combine(wwwRootPath, @"images\teams");

                        var extension = Path.GetExtension(file.FileName);

                        if (model.Team.ImageURL != null)
                        {
                            var currentImagePath = Path.Combine(wwwRootPath, model.Team.ImageURL.TrimStart('\\'));

                            if (System.IO.File.Exists(currentImagePath))
                            {
                                System.IO.File.Delete(currentImagePath);
                            }
                        }

                        using (var fileStreams = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                        }

                        model.Team.ImageURL = @"\images\teams\" + fileName + extension;
                    }

                    var teamFromBD = _teamRepository.GetById(id);

                    if (teamFromBD.Name != model.Team.Name)
                    {
                        var teamNameExists = _teamRepository.GetAll().Where(t => t.Name.ToLower() == model.Team.Name.ToLower()).Count();

                        if (teamNameExists >= 1)
                        {
                            ModelState.AddModelError("Team.Name", "This Team name is already in use");

                            TeamViewModel teamViewModel = new()
                            {
                                Team = new(),

                                PlayerList = _playerRepository.GetAll().Include(p => p.Position).Include(p => p.Country).Where(e => e.TeamId == id).OrderBy(e => e.Name),

                                StaffList = _staffRepository.GetAll().Include(p => p.Function).Include(p => p.Country).Where(e => e.TeamId == id).OrderBy(e => e.Name),

                                CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                                {
                                    Text = i.Name,
                                    Value = i.Id.ToString(),
                                }),

                                //Statistics = _context.Statistics.GetFirstOrDefault(u => u.Team.Id == id),
                            };

                            return View(teamViewModel);
                        }

                    }

                    if (teamFromBD.CountryId != model.Team.CountryId)
                    {
                        teamFromBD.CountryId = model.Team.CountryId;
                    }

                    if (teamFromBD.Venue != model.Team.Venue)
                    {
                        teamFromBD.Venue = model.Team.Venue;
                    }

                    await _teamRepository.UpdateAsync(model.Team);

                    TempData["success"] = $"{model.Team.Name} updated";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _teamRepository.ExistAsync(model.Team.Id))
                    {
                        return new NotFoundViewResult("TeamNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(model);
        }

        // GET: Teams/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("TeamNotFound");
            }

            var team = await _teamRepository.GetByIdAsync(id.Value);

            if (team == null)
            {
                return new NotFoundViewResult("TeamNotFound");
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);

            try
            {
                await _teamRepository.DeleteAsync(team);

                TempData["success"] = $"{team.Name} removed";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.ErrorTitle = "This Club is in use";
                ViewBag.ErrorMessage = "Consider deleting all Matches appended and try again.";
                return View("Error");
            }
        }

        public IActionResult TeamNotFound()
        {
            return View();
        }
    }
}
