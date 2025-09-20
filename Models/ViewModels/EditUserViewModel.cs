using System.ComponentModel.DataAnnotations;

namespace star_events.Models.ViewModels;

public class EditUserViewModel
{
    public string UserId { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }
}