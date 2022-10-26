using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Superleague.Data.Entities
{
    public class Player : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, 99)]
        [Display(Name = "Number")]
        public int Number { get; set; }

        [Display(Name = "Image")]
        [ValidateNever]
        public string ImageURL { get; set; }

        [Required]
        [Display(Name = "Position")]
        public int PositionId { get; set; }
        [ForeignKey("PositionId")]

        [ValidateNever]
        public Position Position { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int? CountryId { get; set; }
        [ForeignKey("CountryId")]

        [ValidateNever]
        public Country Country { get; set; }

        [Required]
        [Display(Name = "Team")]
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]

        [ValidateNever]
        public Team Team { get; set; }
    }
}
