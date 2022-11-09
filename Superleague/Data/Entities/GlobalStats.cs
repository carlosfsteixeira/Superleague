using System.ComponentModel.DataAnnotations;

namespace Superleague.Data.Entities
{
    public class GlobalStats : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int TotalMatches { get; set; }

        public int TotalGoals { get; set; }

        public double GoalAverage { get; set; }

        public double HomeWins { get; set; }

        public double AwayWins { get; set; }

        public double Draws { get; set; }

        public int TotalYellowCards { get; set; }

        public int TotalRedCards { get; set; }

        public string BestAttack { get; set; }

        public string WorstAttack { get; set; }

        public string BestDefence { get; set; }

        public string WorstDefence { get; set; }

        public string MostWins { get; set; }

        public string LessWins { get; set; }

        public string MostDefeats { get; set; }

        public string LessDefeats { get; set; }

        public string MostDraws { get; set; }

        public string LessDraws { get; set; }
    }
}
