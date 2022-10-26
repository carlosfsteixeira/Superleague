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

namespace Superleague.Controllers
{
    public class StaffsController : Controller
    {
        private readonly IStaffRepository _context;

        public StaffsController(IStaffRepository context)
        {
            _context = context;
        }

        // GET: Staffs
        public IActionResult Index()
        {
            return View(_context.GetAll().OrderBy(e => e.Name).Include(p => p.Country).Include(p => p.Function).Include(p => p.Team));
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            //ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Description");
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name");

            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff)
        {
            if (ModelState.IsValid)
            {
                await _context.CreateAsync(staff);

                return RedirectToAction(nameof(Index));
            }

            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", staff.CountryId);
            //ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Description", staff.FunctionId);
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", staff.TeamId);

            return View(staff);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.GetByIdAsync(id.Value);

            if (staff == null)
            {
                return NotFound();
            }

            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", staff.CountryId);
            //ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Description", staff.FunctionId);
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", staff.TeamId);

            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateAsync(staff);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.ExistAsync(staff.Id))
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

            return View(staff);
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.GetByIdAsync(id.Value);

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
            var staff = await _context.GetByIdAsync(id);

            await _context.DeleteAsync(staff);

            return RedirectToAction(nameof(Index));
        }
    }
}
