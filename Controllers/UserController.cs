using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using star_events.Models;
using star_events.Models.ViewModels;
using star_events.Repository.Interfaces;

namespace star_events.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserController(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    // GET: ApplicationUser
    public async Task<IActionResult> Index()
    {
        var users = _userRepository.GetAll();
        var userViewModels = users.Select(user => new UserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ContactNo = user.ContactNo,
            LoyaltyPoints = user.LoyaltyPoints,
            Roles = _userRepository.GetRoles(user)
        }).ToList();
        
        return View(userViewModels);
    }

    // GET: ApplicationUser/Details/5
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null) return NotFound();

        var applicationUser = _userRepository.GetById(id);
        if (applicationUser == null) return NotFound();

        return View(applicationUser);
    }

    // GET: ApplicationUser/Create
    public IActionResult Create()
    {
        var roles = _roleRepository.GetAll();
        var model = new CreateUserViewModel
        {
            AllRoles = roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Name }).ToList()
        };
        return View(model);
    }

    // POST: ApplicationUser/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelStateEntry in ModelState)
            {
                var key = modelStateEntry.Key;
                var errors = modelStateEntry.Value.Errors;
                
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation Error for '{key}': {error.ErrorMessage}");
                }
            }
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            ContactNo =  model.ContactNo,
        };
        
        var result = _userRepository.Create(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            Console.WriteLine(result);
            return View(model);
        }
        
        if (model.SelectedRoles != null && model.SelectedRoles.Any())
        {
            _userRepository.AddToRoles(user, model.SelectedRoles);
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: ApplicationUser/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();

        var applicationUser = _userRepository.GetById(id);
        if (applicationUser == null) return NotFound();
        return View(applicationUser);
    }

    // POST: ApplicationUser/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name,Description")] ApplicationUser applicationUser)
    {
        // if (id != applicationUser.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _userRepository.Update(applicationUser);
            return RedirectToAction(nameof(Index));
        }

        return View(applicationUser);
    }

    // GET: ApplicationUser/Delete/5
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null) return NotFound();

        var applicationUser = _userRepository.GetById(id);
        if (applicationUser == null) return NotFound();

        return View(applicationUser);
    }

    // POST: ApplicationUser/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var applicationUser = _userRepository.GetById(id);
        if (applicationUser != null)
        {
            _userRepository.Delete(applicationUser);
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    // public IActionResult ManageUserRoles(ManageUserRolesViewModel model)
    // {
    //     var user = _userRepository.GetById(model.UserId);
    //     if (user == null) return NotFound();
    //
    //     var currentRoles = _userRepository.GetRoles(user);
    //     var result = _userRepository.RemoveFromRoles(user, currentRoles);
    //
    //     if (!result.Succeeded)
    //     {
    //         ModelState.AddModelError("", "Cannot remove user's existing roles");
    //         return View(model);
    //     }
    //
    //     result = _userRepository.AddToRoles(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));
    //
    //     if (!result.Succeeded)
    //     {
    //         ModelState.AddModelError("", "Cannot add selected roles to user");
    //         return View(model);
    //     }
    //
    //     return RedirectToAction("UserList");
    // }


    // =================================================================
    // ROLE MANAGEMENT
    // =================================================================

    public IActionResult RoleList()
    {
        var roles = _roleRepository.GetAll();
        return View(roles);
    }

    [HttpGet]
    public IActionResult CreateRole()
    {
        return View();
    }

    // [HttpPost]
    // public IActionResult CreateRole(CreateRoleViewModel model)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         if (_roleRepository.GetByName(model.RoleName) == null)
    //         {
    //             _roleRepository.Create(new IdentityRole(model.RoleName));
    //         }
    //         return RedirectToAction("RoleList");
    //     }
    //     return View(model);
    // }
}