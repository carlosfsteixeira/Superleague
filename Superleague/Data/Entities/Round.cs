using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Superleague.Data.Entities
{
    public class Round : IEntity
    {
        [Key]
        [Display(Name = "Round")]
        public int Id { get; set; }


        [Required]
        public string Description { get; set; }

        public bool Complete { get; set; }
    }
}
