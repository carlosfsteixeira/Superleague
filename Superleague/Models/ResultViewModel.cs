using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Superleague.Data.Entities;
using System.Collections.Generic;

namespace Superleague.Models
{
    public class ResultViewModel
    {
        public Result Result { get; set; }

        [ValidateNever]
        public int MatchId { get; set; }

        [ValidateNever]
        public Match Match { get; set; }

        [ValidateNever]
        public IEnumerable<Match> MatchList { get; set; }
    }
}
