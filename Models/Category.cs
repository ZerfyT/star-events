using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        // public ICollection<Event> Events { get; set; } // One-to-many with Events
    }
}