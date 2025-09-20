using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace star_events.Models
{
    public class TicketType
    {
        [Key]
        public int TicketTypeID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int TotalQuantity { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        // Navigation properties
        [ForeignKey("EventID")]
        public virtual Event? Event { get; set; } 
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}