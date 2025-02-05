﻿using System.ComponentModel.DataAnnotations;

namespace Superleague.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public int TeamId { get; set; }
    }
}
