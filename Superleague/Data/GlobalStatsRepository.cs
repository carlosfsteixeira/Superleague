using Microsoft.EntityFrameworkCore;
using Superleague.Data.Entities;
using System.Threading.Tasks;

namespace Superleague.Data
{
    public class GlobalStatsRepository : Repository<GlobalStats>, IGlobalStatsRepository
    {
        private readonly DataContext _context;

        public GlobalStatsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task RefreshGlobalStatisticsTableAsync()
        {
            var globalStats = await _context.GlobalStatistics.ToListAsync();

            _context.GlobalStatistics.RemoveRange(globalStats);
        }

    }
}
