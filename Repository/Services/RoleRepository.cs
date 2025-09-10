using Microsoft.AspNetCore.Identity;
using star_events.Repository.Interfaces;

namespace star_events.Repository.Services;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleRepository(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IdentityRole GetById(string id)
    {
        return _roleManager.FindByIdAsync(id).Result;
    }

    public IdentityRole GetByName(string name)
    {
        return _roleManager.FindByNameAsync(name).Result;
    }

    public IEnumerable<IdentityRole> GetAll()
    {
        return _roleManager.Roles.ToList();
    }

    public IdentityResult Create(IdentityRole role)
    {
        return _roleManager.CreateAsync(role).Result;
    }

    public IdentityResult Update(IdentityRole role)
    {
        return _roleManager.UpdateAsync(role).Result;
    }

    public IdentityResult Delete(IdentityRole role)
    {
        return _roleManager.DeleteAsync(role).Result;
    }
}