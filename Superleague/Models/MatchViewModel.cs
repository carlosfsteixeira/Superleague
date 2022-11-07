using Microsoft.AspNetCore.Mvc.Rendering;
using Superleague.Data.Entities;
using System.Collections.Generic;
using Match = Superleague.Data.Entities.Match;

namespace Superleague.Models
{
    public class MatchViewModel
    {
        public Match Match { get; set; }

        public IEnumerable<Round> Rounds { get; set; }

        public IEnumerable<Team> TeamsList { get; set; }

        public IEnumerable<SelectListItem> TeamNamesList { get; set; }

        public IEnumerable<SelectListItem> RoundsList { get; set; }
    }
}
