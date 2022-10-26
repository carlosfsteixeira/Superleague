using Superleague.Data.Entities;

namespace Superleague.Data
{
    public class FunctionRepository : Repository<Function>, IFunctionRepository
    {
        public FunctionRepository(DataContext context) : base(context)
        {

        }
    }
}
