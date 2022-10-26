using Superleague.Data.Entities;
using System.Linq;

namespace Superleague.Data
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        public TeamRepository(DataContext context) : base(context)
        {
        }


    }
}
