using Microsoft.AspNetCore.Identity;
using star_events.Models;

namespace star_events.Repository.Interfaces;

public interface IUserRepository
{
    ApplicationUser GetById(string id);
    IEnumerable<ApplicationUser> GetAll();
    IdentityResult Create(ApplicationUser user, string password);
    IdentityResult Update(ApplicationUser user);
    IdentityResult Delete(ApplicationUser user);

    // Role-related methods for a user
    IEnumerable<string> GetRoles(ApplicationUser user);
    IdentityResult AddToRoles(ApplicationUser user, IEnumerable<string> roles);
    IdentityResult RemoveFromRoles(ApplicationUser user, IEnumerable<string> roles);
}