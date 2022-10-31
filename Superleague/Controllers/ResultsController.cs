using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Data.Entities;
using Superleague.Models;

namespace Superleague.Controllers
{
    public class ResultsController : Controller
    {
        private readonly IResultRepository _resultRepository;
        private readonly IMatchRepository _matchRepository;

        public ResultsController(IResultRepository resultRepository, IMatchRepository matchRepository)
        {
            _resultRepository = resultRepository;
            _matchRepository = matchRepository;
        }

        // GET: Results
        public IActionResult Index()
        {
            var dataContext = _resultRepository.GetAll().Include(r => r.Match);

            return View(dataContext);
        }

        // GET: Results/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ResultViewModel resultViewModel = new()
            {
                Result = new(),

                Match = new(),

            };

            resultViewModel.Result = await _resultRepository.GetByIdAsync(id.Value);

            resultViewModel.Result.Match = await _matchRepository.GetByIdAsync(resultViewModel.Result.MatchId.Value);

            if (resultViewModel == null)
            {
                return NotFound();
            }

            return View(resultViewModel);
        }

        // GET: Results/Create
        public IActionResult Create(int id)
        {
            ResultViewModel model = new()
            {
                Result = new(),

                Match = _matchRepository.GetAll().Include(p => p.HomeTeam).Include(p => p.AwayTeam).Include(p => p.Round).Where(u => u.Id == id).FirstOrDefault(),
            };

            return View(model);
        }

        // POST: Results/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResultViewModel model, int id)
        {
            var results = _resultRepository.GetAll().Where(u => u.Id == id);

            if (results == null)
            {
                model.Result.MatchId = id;
                model.Result.Match = await _matchRepository.GetAll().Include(p => p.HomeTeam).Include(p => p.AwayTeam).Include(p => p.Round).Where(u => u.Id == id).FirstOrDefaultAsync();
                model.Result.HomeTeam = model.Result.Match.HomeTeam;
                model.Result.AwayTeam = model.Result.Match.AwayTeam;
                model.Result.Round = model.Result.Match.Round;

                if (ModelState.IsValid)
                {
                    await _resultRepository.CreateAsync(model.Result);

                    TempData["success"] = $"Result added";

                    return RedirectToAction(nameof(Index));
                }
            }        
            
            return View(model);
        }

        // GET: Results/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ResultViewModel resultViewModel = new()
            {
                Result = new(),
            };

            resultViewModel.Result = await _resultRepository.GetByIdAsync(id.Value);

            resultViewModel.Result.Match = await _matchRepository.GetAll().Include(p => p.HomeTeam).Include(p => p.AwayTeam).Include(p => p.Round).Where(u => u.Id == id).FirstOrDefaultAsync();

            if (resultViewModel == null)
            {
                return NotFound();
            }

            return View(resultViewModel);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ResultViewModel model)
        {
            model.Result.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    await _resultRepository.UpdateAsync(model.Result);

                    TempData["success"] = $"Result updated";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _resultRepository.ExistAsync(model.Result.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(model);
        }

        // GET: Results/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _resultRepository.GetByIdAsync(id.Value);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _resultRepository.GetByIdAsync(id);

            await _resultRepository.DeleteAsync(result);

            TempData["success"] = $"Result removed";

            return RedirectToAction(nameof(Index));
        }


    }
}
