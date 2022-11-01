using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Superleague.Data.Entities
{
    public class Result : IEntity
    {
        public int Id { get; set; }

        [Range(0, 99)]
        [Required]
        public int HomeGoals { get; set; }

        [Range(0, 99)]
        [Required]
        public int AwayGoals { get; set; }

        public int? HomeTeamId { get; set; }
        [ForeignKey("HomeTeamId")]
        [ValidateNever]

        public Team HomeTeam { get; set; }

        public int? AwayTeamId { get; set; }
        [ForeignKey("AwayTeamId")]
        [ValidateNever]

        public Team AwayTeam { get; set; }

        public int? RoundId { get; set; }
        [ForeignKey("RoundId")]
        [ValidateNever]

        public Round Round { get; set; }

        [Range(0, 99)]
        [Required]
        public int HomeYellowCards { get; set; }

        [Range(0, 99)]
        [Required]
        public int AwayYellowCards { get; set; }

        [Range(0, 99)]
        [Required]
        public int HomeRedCards { get; set; }

        [Range(0, 99)]
        [Required]
        public int AwayRedCards { get; set; }

        public int? MatchId { get; set; }
        [ForeignKey("MatchId")]
        [ValidateNever]

        public Match Match { get; set; }
    }
}
