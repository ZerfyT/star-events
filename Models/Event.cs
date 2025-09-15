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

        [Display(Name = "Organizer")]
        public string? OrganizerID { get; set; } // Maps to ApplicationUser.Id

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

        // JSON array to store all image paths (URLs and uploaded files)
        public string? ImagePaths { get; set; }

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

        [NotMapped]
        public string EventType => Category?.Name ?? "Unknown";

        [NotMapped]
        public string OrganizerName => Organizer?.FirstName + " " + Organizer?.LastName ?? "Unknown Organizer";

        [NotMapped]
        public string Venue => Location?.Name ?? "Unknown Venue";

        [NotMapped]
        public string Venue_Type => Location?.Address ?? "Unknown Address";

        [NotMapped]
        public DateTime StartDate => StartDateTime;

        [NotMapped]
        public DateTime EndDate => EndDateTime;


        [NotMapped]
        public int Allocated_Duration => (int)(EndDateTime - StartDateTime).TotalMinutes;

        [NotMapped]
        public List<string> AllImagePaths
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePaths))
                    return new List<string>();
                
                try
                {
                    return System.Text.Json.JsonSerializer.Deserialize<List<string>>(ImagePaths) ?? new List<string>();
                }
                catch
                {
                    return new List<string>();
                }
            }
            set
            {
                ImagePaths = value != null && value.Any() 
                    ? System.Text.Json.JsonSerializer.Serialize(value) 
                    : null;
            }
        }
    }
}