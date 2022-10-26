using System.ComponentModel.DataAnnotations;

namespace Superleague.Data.Entities
{
    public class Position : IEntity 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
    }
}
