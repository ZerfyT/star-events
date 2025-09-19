using System.ComponentModel.DataAnnotations;

namespace star_events.Models.ViewModels;

public class ProfileViewModel
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
    
    [Display(Name = "Contact Number")]
    public string? ContactNo { get; set; }
    
    [Display(Name = "Address")]
    public string? Address { get; set; }
    
    [Display(Name = "Loyalty Points")]
    public int LoyaltyPoints { get; set; }
}

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; } = string.Empty;
    
    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
