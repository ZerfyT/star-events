using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace star_events.Models
{
    public class Role : IdentityRole<int> // Use int as TKey to match RoleID INT PK
    {
        [Required]
        [StringLength(50)]
        public override string Name { get; set; } // Override to match ERD RoleName

        // Navigation properties
        public ICollection<ApplicationUser> Users { get; set; } // 1:N with Users
    }
}