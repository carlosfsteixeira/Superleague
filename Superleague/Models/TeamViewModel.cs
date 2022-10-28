using Microsoft.AspNetCore.Http;
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

        public IEnumerable<SelectListItem> CountryList { get; set; }

        public IEnumerable<Player> PlayerList { get; set; }

        public IEnumerable<Staff> StaffList { get; set; }

        //public Statistics Statistics { get; set; }


    }
}
