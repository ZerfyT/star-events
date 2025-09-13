using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace star_events.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [Display(Name = "Venue")]
        public int LocationID { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Organizer")]
        public string? OrganizerId { get; set; } // Maps to ApplicationUser.Id

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string ImageURL { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        // Navigation properties
        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }
        
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        
        [ForeignKey("OrganizerId")]
        public virtual ApplicationUser? Organizer { get; set; }
        
        public virtual ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();

        // Computed properties for view compatibility
        [NotMapped]
        public string Event_Name => Title;

        [NotMapped]
        public string Event_Type => Category?.Name ?? "Unknown";

        [NotMapped]
        public string Organizer_Name => Organizer?.FirstName + " " + Organizer?.LastName ?? "Unknown Organizer";

        [NotMapped]
        public string Venue => Location?.Name ?? "Unknown Venue";

        [NotMapped]
        public string Venue_Type => Location?.Address ?? "Unknown Address";

        [NotMapped]
        public DateTime StartDate => StartDateTime;

        [NotMapped]
        public DateTime EndDate => EndDateTime;

        [NotMapped]
        public string PosterPath1 => ImageURL;

        [NotMapped]
        public int Allocated_Duration => (int)(EndDateTime - StartDateTime).TotalMinutes;
    }
}