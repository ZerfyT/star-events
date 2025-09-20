using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace star_events.Models;

public class Booking
{
    [Key] public int BookingID { get; set; }

    [Required] public string UserID { get; set; }

    [Required] public int EventID { get; set; }

    public int? PromotionID { get; set; }

    [Required]
    [Display(Name = "Booking Date & Time")]
    public DateTime BookingDateTime { get; set; }

    [Required]
    [Display(Name = "Total Amount")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
    public decimal TotalAmount { get; set; }

    [Required]
    [StringLength(20)]
    [Display(Name = "Booking Status")]
    public string Status { get; set; }

    // Navigation properties
    [ForeignKey("UserID")] public virtual ApplicationUser User { get; set; }

    [ForeignKey("EventID")] public virtual Event Event { get; set; }

    [ForeignKey("PromotionID")] public virtual Promotion? Promotion { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}