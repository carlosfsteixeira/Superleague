using Superleague.Data.Entities;

namespace Superleague.Data
{
    public class StatisticsRepository : Repository<Statistics>, IStatisticsRepository
    {
        public StatisticsRepository(DataContext context) : base(context)
        {

        }
    }
}
