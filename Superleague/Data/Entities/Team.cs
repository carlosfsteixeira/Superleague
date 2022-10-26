using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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

        [Display(Name = "Image")]
        [ValidateNever]
        public string ImageURL { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set; }
        [ForeignKey("CountryId")]

        [ValidateNever]
        public Country Country { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageURL))
                {
                    return null;
                }

                return $"https://localhost:44368{ImageURL.Substring(1)}";
            }
        }
    }
}
