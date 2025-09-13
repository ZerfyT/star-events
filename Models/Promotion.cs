using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace star_events.Models
{
    public class Promotion
    {
        [Key]
        public int PromotionID { get; set; }
        
        // [Required]
        public int? EventID { get; set; }

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

        // Navigation properties
        [ForeignKey("EventID")]
        public virtual Event? Event { get; set; }
        
        // public virtual ICollection<Booking> Bookings { get; set; } // One-to-many
    }
}