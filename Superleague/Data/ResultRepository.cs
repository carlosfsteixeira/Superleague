using Superleague.Data.Entities;

namespace Superleague.Data
{
    public class ResultRepository : Repository<Result>, IResultRepository
    {
        public ResultRepository(DataContext context) : base(context)
        {

        }
    }
}
