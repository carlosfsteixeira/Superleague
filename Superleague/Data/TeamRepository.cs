using Microsoft.EntityFrameworkCore;
using Superleague.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Superleague.Data
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        private readonly DataContext _context;
        private readonly ICountryRepository _country;

        public TeamRepository(DataContext context, ICountryRepository country) : base(context)
        {
            _context = context;
            _country = country;
        }

        //public Task<IQueryable<Team>> GetCountriesAsync()
        //{
        //    return _context.Teams.Include(e => e.Country);
        //}
    }
}
