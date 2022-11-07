using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace Superleague.Data.Entities
{
    public class Match : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Match Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyy HH:mm}")]
        public DateTime MatchDate { get; set; }

        [Required]
        [Display(Name = "Round")]
        public int? RoundId { get; set; }
        [ForeignKey("RoundId")]

        [ValidateNever]
        public Round Round { get; set; }

        [Required]
        [Display(Name = "Home Team")]
        public int? HomeTeamId { get; set; }
        [ForeignKey("HomeTeamId")]

        [ValidateNever]
        public Team HomeTeam { get; set; }

        [Required]
        [Display(Name = "Away Team")]
        public int? AwayTeamId { get; set; }
        [ForeignKey("AwayTeamId")]

        [ValidateNever]
        public Team AwayTeam { get; set; }

        [ValidateNever]
        public bool Played { get; set; }

        public bool HasResult { get; set; }
    }
}
