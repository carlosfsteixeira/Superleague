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


            var user = await _userHelper.GetUserByEmailAsync("admin@superleague.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "admin",
                    LastName = "superleague",
                    Email = "admin@superleague.com",
                    UserName = "admin@superleague.com",
                    Role = "Admin",
                    ImageURL = @"images\archive\user.png",
                    EmailConfirmed = true,
                };

                var result = await _userHelper.AddUserAsync(user, "Cinel123.");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }
        }
    }
}
