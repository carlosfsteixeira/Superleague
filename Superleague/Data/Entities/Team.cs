using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Superleague.Data.Entities
{
    public class Team : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Venue { get; set; }

        [Required]
        [Display(Name = "Image")]
        [ValidateNever]
        public string ImageURL { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int? CountryId { get; set; }
        [ForeignKey("CountryId")]

        [ValidateNever]
        public Country Country { get; set; }

        public List<Match> HomeMatches { get; set; }
        public List<Match> AwayMatches { get; set; }
    }
}
