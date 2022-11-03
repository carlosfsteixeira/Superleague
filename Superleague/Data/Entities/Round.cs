﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Superleague.Data.Entities
{
    public class Round : IEntity
    {
        [Required]
        [Display(Name = "Round")]
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
