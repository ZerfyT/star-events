using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace star_events.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public int BookingID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Transaction ID")]
        public string PaymentGatewayTransactionID { get; set; }

        [Required]
        [Display(Name = "Amount Paid")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount paid must be greater than 0")]
        public decimal AmountPaid { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }

        [Required]
        [Display(Name = "Payment Date & Time")]
        public DateTime PaymentDateTime { get; set; }

        // Navigation property
        [ForeignKey("BookingID")]
        public virtual Booking Booking { get; set; } 
    }
}