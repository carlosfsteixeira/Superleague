using System.ComponentModel.DataAnnotations;

namespace Superleague.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
