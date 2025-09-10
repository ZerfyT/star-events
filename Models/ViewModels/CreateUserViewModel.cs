using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace star_events.Models.ViewModels;

public class CreateUserViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required]
    public string FirstName { get; set; }
        
    [Required]
    public string LastName { get; set; }
    
    public string? ContactNo { get; set; }
    public string? Address { get; set; }
    
    [ValidateNever]
    public List<SelectListItem> AllRoles { get; set; }
        
    [Display(Name = "Roles")]
    public List<string> SelectedRoles { get; set; }
}