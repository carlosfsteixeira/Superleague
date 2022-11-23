using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;

namespace Superleague.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchRepository _matchRepository;

        public MatchesController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }


        [HttpGet]
        public IActionResult GetMatches()
        {
            return Ok(_matchRepository.GetAll().Include("Round").Include("HomeTeam").Include("AwayTeam"));
        }
    }
}
