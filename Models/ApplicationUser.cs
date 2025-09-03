using Microsoft.AspNetCore.Identity;

namespace star_events.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Navigation properties for relationships
        public ICollection<Event> OrganizedEvents { get; set; } // Events organized by this user
        public ICollection<Booking> Bookings { get; set; } // Bookings made by this user
    }
}