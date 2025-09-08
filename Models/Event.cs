using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        //[Required]
        //public int LocationID { get; set; }

        //[Required]
        //public int CategoryID { get; set; }

        //[Required]
        //public int OrganizerID { get; set; } // Maps to ApplicationUser.ID

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
        public Location Location { get; set; } // Existing
        public Category Category { get; set; } // New: Links to Categories table
        public ApplicationUser Organizer { get; set; } // New: Links to Users table via OrganizerID
        public ICollection<TicketType> TicketTypes { get; set; } // Existing


    }
}