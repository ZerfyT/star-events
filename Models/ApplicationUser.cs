using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class ApplicationUser : IdentityUser <int>
    {
        //[Required]
        //public int RoleID { get; set; } // Foreign key to Roles table

        [PersonalData]
        [Required]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        public string LastName { get; set; }

        [PersonalData]
        [Required]
        public string ContactNo { get; set; }

        [PersonalData]
        public string Address { get; set; }

        //[Required]
        //[EmailAddress]
        //[StringLength(256)]
        //public override string Email { get; set; }

        [PersonalData]
        [Required]
        public int LoyaltyPoints { get; set; }

        // Navigation properties for relationships
        public Role Role { get; set; } // 1:1 to Role (via RoleID)
        public ICollection<Event> OrganizedEvents { get; set; } // Events organized by this user
        public ICollection<Booking> Bookings { get; set; } // Bookings made by this user
    }
}