using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Superleague.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageURL { get; set; }

        [Display(Name = "Team")]
        public int? TeamId { get; set; }
        [ForeignKey("TeamId")]

        [ValidateNever]
        public Team Team { get; set; }

        public string Role { get; set; }

    }
}
