using Microsoft.AspNetCore.Identity;
using Superleague.Data.Entities;
using Superleague.Helpers;
using System;
using System.Threading.Tasks;

namespace Superleague.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Club");


            var user = await _userHelper.GetUserByEmailAsync("carlost2410@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Carlos",
                    LastName = "Teixeira",
                    Email = "carlost2410@gmail.com",
                    UserName = "carlost2410@gmail.com",
                    Role = "Admin",
                    ImageURL = @"images\archive\user.png"
                };

                var result = await _userHelper.AddUserAsync(user, "Cinel123.");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }
        }
    }
}
