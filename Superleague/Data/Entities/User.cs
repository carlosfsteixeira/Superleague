using Microsoft.AspNetCore.Identity;

namespace Superleague.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
