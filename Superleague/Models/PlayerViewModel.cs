using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Superleague.Data.Entities;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Superleague.Models
{
    public class PlayerViewModel
    {
        [Display(Name="Image")]
        public IFormFile ImageFile { get; set; }

        public Player Player { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> PositionList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> TeamList { get; set; }

        [ValidateNever]
        public int TeamId { get; set; }

        //public Statistics Statistics { get; set; }


    }
}
