using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;

namespace Superleague.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerRepository _context;

        public PlayersController(IPlayerRepository context)
        {
            _context = context;
        }

        // GET: Players
        public IActionResult Index()
        {
            return View(_context.GetAll().OrderBy(e => e.Name).Include(p => p.Country).Include(p => p.Position).Include(p => p.Team));
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            //ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Description");
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name");

            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {
            if (ModelState.IsValid)
            {
                await _context.CreateAsync(player);

                return RedirectToAction(nameof(Index));
            }

            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", player.CountryId);
            //ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Description", player.PositionId);
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", player.TeamId);

            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.GetByIdAsync(id.Value);

            if (player == null)
            {
                return NotFound();
            }

            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", player.CountryId);
            //ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Description", player.PositionId);
            //ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", player.TeamId);

            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateAsync(player);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.ExistAsync(player.Id))
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

            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.GetByIdAsync(id.Value);

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
            var player = await _context.GetByIdAsync(id);

            await _context.DeleteAsync(player);

            return RedirectToAction(nameof(Index));
        }
    }
}
