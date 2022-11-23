using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superleague.Data;

namespace Superleague.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IResultRepository _resultRepository;

        public ResultsController(IResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }

        [HttpGet]
        public IActionResult GetResults()
        {
            return Ok(_resultRepository.GetAll().Include("HomeTeam").Include("AwayTeam").Include("Round").Include("Match"));
        }
    }
}
