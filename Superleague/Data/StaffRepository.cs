using Superleague.Data.Entities;

namespace Superleague.Data
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        public StaffRepository(DataContext context) : base(context)
        {

        }
    }
}
