using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Superleague.Data.Entities;

namespace Superleague.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Function> Functions { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Statistics> Statistics { get; set; }

        public DbSet<Result> Results { get; set; }

        public DbSet<Round> Rounds { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<GlobalStats> GlobalStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Match>()
                .HasOne(x => x.HomeTeam)
                .WithMany(x => x.HomeMatches)
                .HasForeignKey(x => x.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Match>()
                .HasOne(x => x.AwayTeam)
                .WithMany(x => x.AwayMatches)
                .HasForeignKey(x => x.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
