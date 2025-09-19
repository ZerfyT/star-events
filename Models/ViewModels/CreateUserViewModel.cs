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
    public string FirstName { get; set; }
        
    [Required]
    public string LastName { get; set; }
    
    public string? ContactNo { get; set; }
    public string? Address { get; set; }
    
    [ValidateNever]
    public List<SelectListItem> AllRoles { get; set; }
        
    [Display(Name = "Role")]
    public string? SelectedRole { get; set; }
}