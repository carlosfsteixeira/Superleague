using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    public class StaffsController : Controller
    {
        private readonly IStaffRepository _staffRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IImageHelper _imageHelper;

        public StaffsController(IStaffRepository staffRepository,
                                    ITeamRepository teamRepository,
                                    ICountryRepository countryRepository,
                                    IFunctionRepository functionRepository,
                                    IImageHelper imageHelper)
        {
            _staffRepository = staffRepository;
            _teamRepository = teamRepository;
            _countryRepository = countryRepository;
            _functionRepository = functionRepository;
            _imageHelper = imageHelper;
        }

        // GET: Staffs
        public IActionResult Index()
        {
            return View(_staffRepository.GetAll().OrderBy(e => e.Name).Include(p => p.Country).Include(p => p.Function).Include(p => p.Team));
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            StaffViewModel staffViewModel = new()
            {
                Staff = new(),

                TeamList = _teamRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),

                FunctionList = _functionRepository.GetAll().Select(i => new SelectListItem
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

            return View(staffViewModel);
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffViewModel model, int id)
        {
            model.Staff.TeamId = id;

            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "staff");

                    model.Staff.ImageURL = path;
                }

                await _staffRepository.CreateAsync(model.Staff);

                return RedirectToAction("Index", new { id = model.TeamId });
            }

            return View(model);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StaffViewModel staffViewModel = new()
            {
                Staff = new(),

                FunctionList = _functionRepository.GetAll().Select(i => new SelectListItem
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

            staffViewModel.Staff = await _staffRepository.GetByIdAsync(id.Value);

            if (staffViewModel == null)
            {
                return NotFound();
            }

            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", staff.CountryId);
            //ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Description", staff.FunctionId);
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", staff.TeamId);

            return View(staffViewModel);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.Staff.ImageURL;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "staff");
                    }

                    await _staffRepository.UpdateAsync(model.Staff);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _staffRepository.ExistAsync(model.Staff.Id))
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
            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", staff.CountryId);
            //ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Description", staff.FunctionId);
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", staff.TeamId);

            return View(model);
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _staffRepository.GetByIdAsync(id.Value);

            //var staff = await _context.Staffs
            //    .Include(s => s.Country)
            //    .Include(s => s.Function)
            //    .Include(s => s.Team)
            //    .FirstOrDefaultAsync(m => m.Id == id);

            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);

            await _staffRepository.DeleteAsync(staff);

            return RedirectToAction(nameof(Index));
        }
    }
}
