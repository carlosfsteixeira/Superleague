using Microsoft.CodeAnalysis.Differencing;
using Superleague.Data.Entities;

namespace Superleague.Data
{
    public class MatchRepository : Repository<Match>, IMatchRepository
    {
        public MatchRepository(DataContext context) : base(context)
        {

        }
    }
}
