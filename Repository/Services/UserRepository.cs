using Microsoft.AspNetCore.Identity;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Repository.Services;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public ApplicationUser GetById(string id)
    {
        return _userManager.FindByIdAsync(id).Result;
    }

    public IEnumerable<ApplicationUser> GetAll()
    {
        return _userManager.Users.ToList();
    }

    public IdentityResult Create(ApplicationUser user, string password)
    {
        return _userManager.CreateAsync(user, password).Result;
    }

    public IdentityResult Update(ApplicationUser user)
    {
        return _userManager.UpdateAsync(user).Result;
    }

    public IdentityResult Delete(ApplicationUser user)
    {
        return _userManager.DeleteAsync(user).Result;
    }

    public IEnumerable<string> GetRoles(ApplicationUser user)
    {
        return _userManager.GetRolesAsync(user).Result;
    }

    public IdentityResult AddToRoles(ApplicationUser user, IEnumerable<string> roles)
    {
        return _userManager.AddToRolesAsync(user, roles).Result;
    }

    public IdentityResult RemoveFromRoles(ApplicationUser user, IEnumerable<string> roles)
    {
        return _userManager.RemoveFromRolesAsync(user, roles).Result;
    }
}