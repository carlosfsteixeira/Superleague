using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Superleague.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageURL { get; set; }

        [ForeignKey("TeamId")]
        public virtual Team? Team { get; set; }

        public string Role { get; set; }

    }
}
