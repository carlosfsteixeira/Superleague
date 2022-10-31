using Superleague.Data.Entities;

namespace Superleague.Data
{
    public class GlobalStatsRepository : Repository<GlobalStats>, IGlobalStatsRepository
    {
        public GlobalStatsRepository(DataContext context) : base(context)
        {
        }
    }
}
