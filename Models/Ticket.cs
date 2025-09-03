using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }

        [Required]
        public int BookingID { get; set; }

        [Required]
        public int TicketTypeID { get; set; }

        [StringLength(255)]
        public string QRCodeValue { get; set; }

        public DateTime? ScannedAt { get; set; } // Nullable

        public Booking Booking { get; set; } // Navigation property
        public TicketType TicketType { get; set; } // Navigation property
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}