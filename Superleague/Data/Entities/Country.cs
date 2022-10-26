using System.ComponentModel.DataAnnotations;

namespace Superleague.Data.Entities
{
    public class Country : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
