using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Helpers;
using Vereyon.Web;

namespace Superleague.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoundsController : Controller
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IFlashMessage _flashMessage;

        public RoundsController(IRoundRepository roundRepository, IFlashMessage flashMessage)
        {
            _roundRepository = roundRepository;
            _flashMessage = flashMessage;
        }

        // GET: Rounds
        public IActionResult Index()
        {
            return View(_roundRepository.GetAll().OrderBy(e => e.Description));
        }

        // GET: Rounds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rounds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Round round)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _roundRepository.CreateAsync(round);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    _flashMessage.Danger("There is already a round with this description");
                }

                return View(round);
            }
            return View(round);
        }

        // POST: Rounds/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var round = await _roundRepository.GetByIdAsync(id);

            try
            {
                await _roundRepository.DeleteAsync(round);

                TempData["success"] = "Round removed";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = "This Round is in use";
                ViewBag.ErrorMessage = "Consider deleting all Matches appended and try again.";
                return View("Error");
            }
        }

        public IActionResult RoundNotFound()
        {
            return View();
        }
    }
}
