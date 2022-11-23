using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Superleague.Data;

namespace Superleague.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalStatsController : ControllerBase
    {
        private readonly IGlobalStatsRepository _globalStatsRepository;

        public GlobalStatsController(IGlobalStatsRepository globalStatsRepository)
        {
            _globalStatsRepository = globalStatsRepository;
        }

        [HttpGet]
        public IActionResult GetStats()
        {
            return Ok(_globalStatsRepository.GetAll());
        }
    }
}
