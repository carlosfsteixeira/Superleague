using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Superleague.Data.Entities;
using System.Net.NetworkInformation;

namespace Superleague.Models
{
    public class StatisticsViewModel
    {
        [ValidateNever]
        public Statistics Statistics { get; set; }

        [ValidateNever]
        public GlobalStats GlobalStats { get; set; }


        [ValidateNever]
        public Result Results { get; set; }
    }
}

