using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Superleague.Data.Entities;
using System.Net.NetworkInformation;

namespace Superleague.Models
{
    public class StatisticsViewModel
    {
        public Statistics Statistics { get; set; }

        public GlobalStats GlobalStats { get; set; }

        public Result Results { get; set; }
    }
}

