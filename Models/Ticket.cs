using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace star_events.Models;

public class Ticket
{
    [Key] public int TicketID { get; set; }

    [Required] public int BookingID { get; set; }

    [Required] public int TicketTypeID { get; set; }

    [StringLength(255)]
    [Display(Name = "QR Code")]
    public string? QRCodeValue { get; set; }

    [Display(Name = "Is Scanned")] public bool IsScanned { get; set; } = false; // Default to false

    [Display(Name = "Scanned At")] public DateTime? ScannedAt { get; set; } // Nullable

    // Navigation properties
    [ForeignKey("BookingID")] public virtual Booking Booking { get; set; }

    [ForeignKey("TicketTypeID")] public virtual TicketType TicketType { get; set; }
}