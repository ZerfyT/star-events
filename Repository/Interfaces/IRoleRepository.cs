using Microsoft.AspNetCore.Identity;

namespace star_events.Repository.Interfaces;

public interface IRoleRepository
{
    IdentityRole GetById(string id);
    IdentityRole GetByName(string name);
    IEnumerable<IdentityRole> GetAll();
    IdentityResult Create(IdentityRole role);
    IdentityResult Update(IdentityRole role);
    IdentityResult Delete(IdentityRole role);
}