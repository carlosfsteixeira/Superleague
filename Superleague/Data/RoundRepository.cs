using Superleague.Data.Entities;

namespace Superleague.Data
{
    public class RoundRepository : Repository<Round>, IRoundRepository
    {
        public RoundRepository(DataContext context) : base(context)
        {

        }
    }
}
