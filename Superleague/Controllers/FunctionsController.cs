using System;
using System.Collections.Generic;
using System.Data;
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
    public class FunctionsController : Controller
    {
        private readonly IFunctionRepository _context;

        public FunctionsController(IFunctionRepository context)
        {
            _context = context;
        }

        // GET: Functions
        public IActionResult Index()
        {
            return View(_context.GetAll().OrderBy(e => e.Description));
        }

        // GET: Functions API
        public IActionResult GetAll()
        {
            var functions = _context.GetAll().OrderBy(e => e.Description);

            return Json(new { data = functions });
        }

        // GET: Functions/Details/5
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

        // GET: Functions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Functions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Function function)
        {
            if (ModelState.IsValid)
            {
                await _context.CreateAsync(function);

                return RedirectToAction(nameof(Index));
            }
            return View(function);
        }

        // GET: Functions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _context.GetByIdAsync(id.Value);

            if (function == null)
            {
                return NotFound();
            }
            return View(function);
        }

        // POST: Functions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Function function)
        {
            if (id != function.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateAsync(function);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.ExistAsync(function.Id))
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
            return View(function);
        }

        // GET: Functions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _context.GetByIdAsync(id.Value);

            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // POST: Functions/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var function = await _context.GetByIdAsync(id);

            try
            {
                await _context.DeleteAsync(function);

                TempData["success"] = $"Function removed";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.ErrorTitle = "This Function is in use";
                ViewBag.ErrorMessage = "Consider deleting all Staff Members appended and try again.";
                return View("Error");
            }
        }
    }
}
