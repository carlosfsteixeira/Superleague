using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Superleague.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PositionsController : Controller
    {
        private readonly IPositionRepository _context;
        private readonly IFlashMessage _flashMessage;

        public PositionsController(IPositionRepository context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        // GET: Positions
        public IActionResult Index()
        {
            return View(_context.GetAll().OrderBy(e => e.Description));
        }

        // GET: Positions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.CreateAsync(position);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    _flashMessage.Danger("There is already a position with this description");
                }

                return View(position);
            }
            return View(position);
        }

        // GET: Positions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PositionNotFound");
            }

            var country = await _context.GetByIdAsync(id.Value);

            if (country == null)
            {
                return new NotFoundViewResult("PositionNotFound");
            }
            return View(country);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Position position)
        {
            if (id != position.Id)
            {
                return new NotFoundViewResult("PositionNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateAsync(position);

                    TempData["success"] = $"Position updated";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.ExistAsync(position.Id))
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
            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var position = await _context.GetByIdAsync(id);

            try
            {
                await _context.DeleteAsync(position);

                TempData["success"] = $"Position removed";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.ErrorTitle = "This Position is in use";
                ViewBag.ErrorMessage = "Consider deleting all Players appended and try again.";
                return View("Error");
            }

        }

        public IActionResult PositionNotFound()
        {
            return View();
        }
    }
}
