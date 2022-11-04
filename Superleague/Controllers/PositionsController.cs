using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;

namespace Superleague.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PositionsController : Controller
    {
        private readonly IPositionRepository _context;

        public PositionsController(IPositionRepository context)
        {
            _context = context;
        }

        // GET: Positions
        public IActionResult Index()
        {
            return View(_context.GetAll().OrderBy(e => e.Description));
        }

        // GET: Positions API
        public IActionResult GetAll()
        {
            var positions = _context.GetAll().OrderBy(e => e.Description);

            return Json(new { data = positions });
        }

        // GET: Positions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.GetByIdAsync(id.Value);

            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Positions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (ModelState.IsValid)
            {
                await _context.CreateAsync(position);

                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        // GET: Positions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.GetByIdAsync(id.Value);

            if (country == null)
            {
                return NotFound();
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
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateAsync(position);
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

        // GET: Positions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.GetByIdAsync(id.Value);

            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
    }
}
