using Superleague.Data.Entities;

namespace Superleague.Data
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(DataContext context) : base(context)
        {

        }
    }
}
