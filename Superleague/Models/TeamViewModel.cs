using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Superleague.Data.Entities;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Superleague.Models
{
    public class TeamViewModel
    {
        //[Display(Name="Image")]
        //public IFormFile ImageFile { get; set; }

        public Team Team { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }
        [ValidateNever]
        public IEnumerable<Player> PlayerList { get; set; }
        [ValidateNever]
        public IEnumerable<Staff> StaffList { get; set; }
        [ValidateNever]
        public Statistics Statistics { get; set; }

        public List<Result> Results { get; set; }

        public int TeamUserId { get; set; }


    }
}
