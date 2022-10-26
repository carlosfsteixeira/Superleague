using Superleague.Data.Entities;

namespace Superleague.Data
{
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(DataContext context) : base(context)
        {

        }
    }
}
