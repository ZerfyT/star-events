using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public int UserID { get; set; }
        public int? PromotionID { get; set; } // Nullable  //FK to Promotion table

        [Required]
        public DateTime BookingDateTime { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }


        // Navigation properties
        public ApplicationUser User { get; set; }
        public Promotion Promotion { get; set; } // Existing
        public ICollection<Ticket> Tickets { get; set; } // Existing
        public ICollection<Payment> Payments { get; set; } // Existing
    }
}