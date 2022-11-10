using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Superleague.Data.Entities
{
    public class Statistics : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "W")]
        public int Wins { get; set; }

        [Display(Name = "D")]
        public int Draws { get; set; }

        [Display(Name = "L")]
        public int Losses { get; set; }

        [Display(Name = "PTS")]
        public int Points { get; set; }

        [Display(Name = "GS")]
        public int GoalsScored { get; set; }

        [Display(Name = "GC")]
        public int GoalsConceded { get; set; }

        [Display(Name = "TM")]
        public int TotalMatches { get; set; }

        [Display(Name = "P")]
        public int Position { get; set; }

        public int TotalYellows { get; set; }

        public int TotalReds { get; set; }

        public int GoalAverage { get; set; }

        public int? TeamId { get; set; }
        [ForeignKey("TeamId")]
        [ValidateNever]

        public Team Team { get; set; }
    }
}
