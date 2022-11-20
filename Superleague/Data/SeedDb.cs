using Microsoft.AspNetCore.Identity;
using Superleague.Data.Entities;
using Superleague.Helpers;
using System;
using System.Linq;
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

            if(!_context.Positions.Any())
            {
                AddPosition("Goalkeeper");
                AddPosition("Defender");
                AddPosition("Midfielder");
                AddPosition("Winger");
                AddPosition("Striker");
            }

            if (!_context.Functions.Any())
            {
                AddFunction("Manager");
                AddFunction("President");
                AddFunction("Assistant Manager");
                AddFunction("Goalkeeper Manager");
                AddFunction("Scout");
                AddFunction("Sporting Director");
                AddFunction("Chief Executive Officer");
                AddFunction("Nutricionist");
                AddFunction("Fitness Coach");
                AddFunction("Technical Coach");
                AddFunction("Individual Coach");
                AddFunction("Rehab Coach");
                AddFunction("Physiotherapist");
                AddFunction("Masseur");
                AddFunction("Kit Manager");
                AddFunction("Club Representative");
                AddFunction("Development Coach");
                AddFunction("Academy Manager");
                AddFunction("Conditioning Coach");
                AddFunction("Head of Scouting");
            }

            if (!_context.Countries.Any())
            {
                AddCountry("Albania");
                AddCountry("Algeria");
                AddCountry("Andorra");
                AddCountry("Angola");
                AddCountry("Argentina");
                AddCountry("Armenia");
                AddCountry("Australia");
                AddCountry("Austria");
                AddCountry("Azerbaijan");
                AddCountry("Belarus");
                AddCountry("Belgium");
                AddCountry("Bosnia and Herzegovina");
                AddCountry("Brazil");
                AddCountry("Bulgaria");
                AddCountry("Cameroon");
                AddCountry("Canada");
                AddCountry("Chade");
                AddCountry("Chile");
                AddCountry("China");
                AddCountry("Colombia");
                AddCountry("Congo");
                AddCountry("Costa Rica");
                AddCountry("Croatia");
                AddCountry("Cuba");
                AddCountry("Cyprus");
                AddCountry("Czech Republic");
                AddCountry("Denmark");
                AddCountry("Ecuador");
                AddCountry("Egypt");
                AddCountry("Estonia");
                AddCountry("England");
                AddCountry("Finland");
                AddCountry("France");
                AddCountry("Gabon");
                AddCountry("Gambia");
                AddCountry("Georgia");
                AddCountry("Germany");
                AddCountry("Ghana");
                AddCountry("Greece");
                AddCountry("Iceland");
                AddCountry("India");
                AddCountry("Ireland");
                AddCountry("Israel");
                AddCountry("Italy");
                AddCountry("Japan");
                AddCountry("Kenya");
                AddCountry("Latvia");
                AddCountry("Lebanon");
                AddCountry("Liechtenstein");
                AddCountry("Lithuania");
                AddCountry("Luxembourg");
                AddCountry("Mali");
                AddCountry("Mexico");
                AddCountry("Montenegro");
                AddCountry("Morocco");
                AddCountry("Netherlands");
                AddCountry("Nigeria");
                AddCountry("Norway");
                AddCountry("Paraguay");
                AddCountry("Peru");
                AddCountry("Poland");
                AddCountry("Portugal");
                AddCountry("Puerto Rico");
                AddCountry("Romania");
                AddCountry("Serbia");
                AddCountry("Scotland");
                AddCountry("Slovakia");
                AddCountry("Slovenia");
                AddCountry("Senegal");
                AddCountry("South Africa");
                AddCountry("Spain");
                AddCountry("Sweden");
                AddCountry("Switzerland");
                AddCountry("Turkey");
                AddCountry("Tunisia");
                AddCountry("Ukraine");
                AddCountry("United Arab Emirates");
                AddCountry("United States of America");
                AddCountry("Uruguay");
                AddCountry("Zambia");
                AddCountry("Zimbabwe");
            }

            if (!_context.Rounds.Any())
            {
                AddRound("Round 1", false, false);
                AddRound("Round 2", false, false);
                AddRound("Round 3", false, false);
                AddRound("Round 4", false, false);
                AddRound("Round 5", false, false);
                AddRound("Round 6", false, false);
                AddRound("Round 7", false, false);
            }
        }

        private void AddPosition(string position)
        {
            _context.Positions.Add(new Position
            {
                Description = position,
            });
            _context.SaveChanges();
        }

        private void AddFunction(string function)
        {
            _context.Functions.Add(new Function
            {
                Description = function,
            });
            _context.SaveChanges();
        }

        private void AddCountry(string name)
        {
            _context.Countries.Add(new Country
            {
                Name = name,
            });
            _context.SaveChanges();
        }

        private void AddRound(string description, bool complete, bool closed)
        {
            _context.Rounds.Add(new Round
            {
                Description = description,
                Complete = complete,
                Closed = closed
            });
            _context.SaveChanges();
        }

    }
}
