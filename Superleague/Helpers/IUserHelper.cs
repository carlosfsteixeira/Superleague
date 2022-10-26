using Microsoft.AspNetCore.Identity;
using Superleague.Data.Entities;
using System.Threading.Tasks;

namespace Superleague.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password); 
    }
}
