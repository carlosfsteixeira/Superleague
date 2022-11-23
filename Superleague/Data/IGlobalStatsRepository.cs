using Superleague.Data.Entities;
using System.Threading.Tasks;

namespace Superleague.Data
{
    public interface IGlobalStatsRepository : IRepository<GlobalStats>
    {
        Task RefreshGlobalStatisticsTableAsync();
    }
}