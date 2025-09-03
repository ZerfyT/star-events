using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class Promotion
    {
        [Key]
        public int PromotionID { get; set; }

        [Required]
        [StringLength(20)]
        public string PromoCode { get; set; }

        [Required]
        [StringLength(20)]
        public string DiscountType { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<Booking> Bookings { get; set; } // One-to-many
    }
}