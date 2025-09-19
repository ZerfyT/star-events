using System.ComponentModel.DataAnnotations;

namespace star_events.Models.ViewModels;

public class UserRoleViewModel
{
    public string RoleName { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}

public class ManageUserRolesViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<UserRoleViewModel> Roles { get; set; } = new List<UserRoleViewModel>();
}
