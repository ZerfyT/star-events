using System.ComponentModel.DataAnnotations;

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

        public Event Event { get; set; } // Navigation property

        // Navigation property to Tickets
        public ICollection<Ticket> Tickets { get; set; }
    }
}