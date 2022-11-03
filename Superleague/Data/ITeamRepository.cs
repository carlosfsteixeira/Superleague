using Superleague.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Superleague.Data
{
    public interface ITeamRepository : IRepository<Team>
    {
        //Task<IQueryable<Team>> GetCountriesAsync();
    }
}