using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        public int Capacity { get; set; }

        // Navigation properties
        // public ICollection<Event> Events { get; set; } // 1:N with Events
    }
}