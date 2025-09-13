using System.ComponentModel;
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

        [Required]
        [Display(Name = "Organizer")]
        public string OrganizerID { get; set; } // Maps to ApplicationUser.ID

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

        [StringLength(255)]
        public string ImageURL { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }


        // Navigation properties
        [ForeignKey("LocationID")]
        public virtual Location? Location { get; set; } // Existing
        
        [ForeignKey("CategoryID")]
        public virtual Category? Category { get; set; } // New: Links to Categories table
        
        [ForeignKey("OrganizerID")]
        public virtual ApplicationUser? Organizer { get; set; } // New: Links to Users table via OrganizerID
        
        // public virtual ICollection<TicketType> TicketTypes { get; set; } // Existing

    }
}