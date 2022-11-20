using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Superleague.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superleague.Data
{
    public class MatchRepository : Repository<Match>, IMatchRepository
    {
        private readonly DataContext _context;

        public MatchRepository(DataContext context) : base(context)
        {
            _context = context;
        }



        public async Task<List<Match>> GetAllMatchesAsync()
        {
            return await _context.Matches.Include(m => m.AwayTeam).Include(m => m.HomeTeam).Include(m => m.Round).ToListAsync();
        }

        public async Task VerifyIfMatchPlayedAsync(List<Match> matches)
        {
            foreach (var match in matches)
            {
                if (match.MatchDate <= DateTime.Now)
                {
                    match.Played = true;
                    _context.Matches.Update(match);
                    await _context.SaveChangesAsync();
                }
            }
        }


    }
}
