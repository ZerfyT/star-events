using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Required]
        public required string FirstName { get; set; }

        [PersonalData]
        [Required]
        public required string LastName { get; set; }

        [PersonalData]
        public string? ContactNo { get; set; }

        // [PersonalData]
        // public string? NIC { get; set; }

        [PersonalData]
        public string? Address { get; set; }

        [PersonalData]
        [DefaultValue(0)]
        public int LoyaltyPoints { get; set; }

        // Navigation properties for relationships
        public virtual ICollection<Event> OrganizedEvents { get; set; } = new List<Event>(); // Events organized by this user
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>(); // Bookings made by this user
    }
}