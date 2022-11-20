using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Helpers;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Superleague.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository _context;
        private readonly IFlashMessage _flashMessage;

        public CountriesController(ICountryRepository context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        // GET: Countries
        public IActionResult Index()
        {
            return View(_context.GetAll().OrderBy(e => e.Name));

        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.CreateAsync(country);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    _flashMessage.Danger("There is already a country with this name");
                }

                return View(country);
            }

            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            var country = await _context.GetByIdAsync(id.Value);

            if (country == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.Id)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateAsync(country);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.ExistAsync(country.Id))
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
            return View(country);
        }

        // POST: Countries/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var country = await _context.GetByIdAsync(id);

            try
            {
                await _context.DeleteAsync(country);

                TempData["success"] = "Country removed";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.ErrorTitle = "This Country is in use";
                ViewBag.ErrorMessage = "Consider deleting all Clubs, Players and Staff Members appended and try again.";
                return View("Error");
            }

        }

        public IActionResult CountryNotFound()
        {
            return View();
        }
    }
}
