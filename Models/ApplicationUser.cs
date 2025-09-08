using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Required]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        public string LastName { get; set; }

        [PersonalData]
        public string? ContactNo { get; set; }

        [PersonalData]
        public string? Address { get; set; }

        [PersonalData]
        [Required]
        [DefaultValue(0)]
        public int LoyaltyPoints { get; set; }

        // Navigation properties for relationships
        public ICollection<Event> OrganizedEvents { get; set; } // Events organized by this user
        public ICollection<Booking> Bookings { get; set; } // Bookings made by this user
    }
}