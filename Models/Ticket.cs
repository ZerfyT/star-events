using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }

        //[Required]
        //public int BookingID { get; set; }  //FK to Bookings

        //[Required]
        //public int TicketTypeID { get; set; }   //FK to Tickets

        [StringLength(255)]
        public string QRCodeValue { get; set; }

        public bool IsScanned { get; set; } = false; // Default to false

        public DateTime? ScannedAt { get; set; } // Nullable

        public Booking Booking { get; set; } // Navigation property
        public TicketType TicketType { get; set; } // Navigation property
    }
}