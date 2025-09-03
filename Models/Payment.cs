using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required]
        public int BookingID { get; set; }

        [Required]
        [StringLength(100)]
        public string PaymentGatewayTransactionID { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        [StringLength(20)]
        public string PaymentStatus { get; set; }

        [Required]
        public DateTime PaymentDateTime { get; set; }

        public Booking Booking { get; set; } // Navigation property
    }
}