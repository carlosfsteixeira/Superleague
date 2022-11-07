using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;
using Superleague.Helpers;
using Superleague.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Superleague.Controllers
{
    public class ResultsController : Controller
    {
        private readonly IResultRepository _resultRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly ITeamRepository _teamRepository;

        public ResultsController(IResultRepository resultRepository, IMatchRepository matchRepository, IRoundRepository roundRepository, ITeamRepository teamRepository)
        {
            _resultRepository = resultRepository;
            _matchRepository = matchRepository;
            _roundRepository = roundRepository;
            _teamRepository = teamRepository;
        }

        // GET: Results
        public IActionResult Index()
        {
            var results = _resultRepository.GetAll().Include(r => r.Match).Include(e => e.Round).Include(e => e.HomeTeam).Include(e => e.AwayTeam);

            return View(results);
        }

        // GET: Results/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ResultNotFound");
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
                return new NotFoundViewResult("ResultNotFound");
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
            var getMatchResult = _resultRepository.GetAll().Where(u => u.MatchId == id).FirstOrDefault();

            if (getMatchResult == null)
            {
                model.Match = _matchRepository.GetAll().Include(p => p.HomeTeam).Include(p => p.AwayTeam).Include(p => p.Round).Where(u => u.Id == id).FirstOrDefault();
                model.Result.MatchId = id;
                model.Result.HomeTeamId = model.Match.HomeTeamId;
                model.Result.AwayTeamId = model.Match.AwayTeamId;
                model.Result.RoundId = model.Match.RoundId;
                model.Match.HasResult = true;

                //model.Result.Match = model.Match;

                if (ModelState.IsValid)
                {
                    await _resultRepository.CreateAsync(model.Result);
                    await _matchRepository.UpdateAsync(model.Match);

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
                return new NotFoundViewResult("ResultNotFound");
            }

            ResultViewModel resultViewModel = new()
            {
                Result = new(),

                Match = new(),
            };

            resultViewModel.Result = _resultRepository.GetAll().Include(r => r.Match).Include(e => e.Round).Include(e => e.HomeTeam).Include(e => e.AwayTeam).Where(u => u.Id == id).FirstOrDefaultAsync().Result;

            if (resultViewModel == null)
            {
                return new NotFoundViewResult("ResultNotFound");
            }

            return View(resultViewModel);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ResultViewModel model, int id)
        {
            model.Result = _resultRepository.GetAll().Include(r => r.Match).Include(e => e.Round).Include(e => e.HomeTeam).Include(e => e.AwayTeam).Where(u => u.Id == id).FirstOrDefaultAsync().Result;

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
                        return new NotFoundViewResult("ResultNotFound");
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
                return new NotFoundViewResult("ResultNotFound");
            }

            var result = await _resultRepository.GetByIdAsync(id.Value);

            if (result == null)
            {
                return new NotFoundViewResult("ResultNotFound");
            }

            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _resultRepository.GetByIdAsync(id);

            var match = await _matchRepository.GetAll().Where(a => a.Id == result.MatchId).FirstAsync();

            await _resultRepository.DeleteAsync(result);

            match.HasResult = false;

            await _matchRepository.UpdateAsync(match);

            TempData["success"] = $"Result removed";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ResultNotFound()
        {
            return View();
        }
    }
}
