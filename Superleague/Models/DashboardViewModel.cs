using Superleague.Data.Entities;
using System.Collections.Generic;

namespace Superleague.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<Team> TeamsList { get; set; }
        
        public int TotalTeams { get; set; }

        public IEnumerable<User> UsersList { get; set; }

        public int TotalUsers { get; set; }

        public IEnumerable<Player> PlayersList { get; set; }

        public int TotalPlayers { get; set; }

        public IEnumerable<Staff> StaffsList { get; set; }

        public int TotalStaffs { get; set; }

        public IEnumerable<Round> Rounds { get; set; }


    }
}
