using Superleague.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Superleague.Models
{
    public class UserProfileViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(30), MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(30), MinLength(2)]
        public string LastName { get; set; }

        [Required]
        public string ImageURL { get; set; }
    }
}
