using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Models;

namespace Superleague.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IStaffRepository _staffRepository;

        public TeamsController(ITeamRepository teamRepository, 
                               ICountryRepository countryRepository, 
                               IPlayerRepository playerRepository, 
                               IStaffRepository staffRepository)
        {
            _teamRepository = teamRepository;
            _countryRepository = countryRepository;
            _playerRepository = playerRepository;
            _staffRepository = staffRepository;
        }

        // GET: Teams
        public IActionResult Index()
        {
            var teamList = _teamRepository.GetAll().Include("Country").OrderBy(e => e.Name);

            return View(teamList);
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TeamViewModel teamViewModel = new()
            {
                Team = new(),

                PlayerList = _playerRepository.GetAll().Include("Country.Position").Where(e => e.TeamId == id).OrderBy(e => e.Name),

                StaffList = _staffRepository.GetAll().Include("Country.Function").Where(e => e.TeamId == id).OrderBy(e => e.Name),

                CountryList = _countryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),

                //Statistics = _statisticsRepository.GetFirstOrDefault(u => u.Team.Id == id),
            };

            teamViewModel.Team = await _teamRepository.GetByIdAsync(id.Value);

            teamViewModel.Team.Country = await _countryRepository.GetByIdAsync(teamViewModel.Team.CountryId.Value);

            if (teamViewModel == null)
            {
                return NotFound();
            }

            return View(teamViewModel);
        }

        // GET: Teams/Create
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
        public async Task<IActionResult> Create(TeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();

                    var file = $"{guid}.jpg";

                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\teams", file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/teams/{file}";

                    model.Team.ImageURL = path;
                }

                await _teamRepository.CreateAsync(model.Team);

                return RedirectToAction(nameof(Index));
            }
  
            return View(model);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TeamViewModel teamViewModel = new()
            {
                Team = new(),

                PlayerList = _playerRepository.GetAll().Include("Country.Position").Where(e => e.TeamId == id).OrderBy(e => e.Name),

                StaffList = _staffRepository.GetAll().Include("Country.Function").Where(e => e.TeamId == id).OrderBy(e => e.Name),

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
                return NotFound();
            }
            
            return View(teamViewModel);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.Team.ImageURL;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();

                        var file = $"{guid}.jpg";

                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\teams", file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/teams/{file}";
                    }


                    await _teamRepository.UpdateAsync(model.Team);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _teamRepository.ExistAsync(model.Team.Id))
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

            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", team.CountryId);

            return View(model);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _teamRepository.GetByIdAsync(id.Value);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);

            await _teamRepository.DeleteAsync(team);

            return RedirectToAction(nameof(Index));
        }
    }
}
