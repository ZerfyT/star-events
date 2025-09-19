using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using star_events.Models;
using star_events.Models.ViewModels;
using star_events.Repository.Interfaces;
using star_events.Services;

namespace star_events.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordService _passwordService;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IUserRepository userRepository, 
        IRoleRepository roleRepository,
        IPasswordService passwordService,
        IEmailService emailService,
        ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordService = passwordService;
        _emailService = emailService;
        _logger = logger;
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
            // Repopulate roles for the view
            var roles = _roleRepository.GetAll();
            model.AllRoles = roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Name }).ToList();
            
            // Show validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["ErrorMessage"] = "Please fix the following errors: " + string.Join(", ", errors);
            return View(model);
        }

        // Generate a random password
        var generatedPassword = _passwordService.GenerateRandomPassword();

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            ContactNo = model.ContactNo,
        };
        
        var result = _userRepository.Create(user, generatedPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            
            // Repopulate roles for the view
            var roles = _roleRepository.GetAll();
            model.AllRoles = roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Name }).ToList();
            return View(model);
        }
        
        // Add role if selected
        if (!string.IsNullOrEmpty(model.SelectedRole))
        {
            _userRepository.AddToRoles(user, new[] { model.SelectedRole });
        }

        // Send email with credentials
        try
        {
            await _emailService.SendUserCredentialsAsync(user.Email, user.FirstName, user.LastName, generatedPassword);
            TempData["SuccessMessage"] = $"User '{user.FirstName} {user.LastName}' created successfully! Login credentials have been sent to {user.Email}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to user {Email}", user.Email);
            TempData["WarningMessage"] = $"User '{user.FirstName} {user.LastName}' created successfully, but failed to send email. Please provide the password manually: {generatedPassword}";
        }
        
        return RedirectToAction(nameof(Index));
    }

    // GET: ApplicationUser/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();

        var applicationUser = _userRepository.GetById(id);
        if (applicationUser == null) return NotFound();

        // Get user roles
        var userRoles = _userRepository.GetRoles(applicationUser);
        var allRoles = _roleRepository.GetAll();

        var model = new ManageUserRolesViewModel
        {
            UserId = applicationUser.Id,
            UserName = applicationUser.UserName!,
            Roles = allRoles.Select(role => new UserRoleViewModel
            {
                RoleName = role.Name!,
                IsSelected = userRoles.Contains(role.Name!)
            }).ToList()
        };

        ViewBag.User = applicationUser;
        return View(model);
    }

    // POST: ApplicationUser/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, ApplicationUser applicationUser)
    {
        if (id != applicationUser.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _userRepository.Update(applicationUser);
            TempData["SuccessMessage"] = $"User '{applicationUser.FirstName} {applicationUser.LastName}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // Repopulate the model for the view
        var userRoles = _userRepository.GetRoles(applicationUser);
        var allRoles = _roleRepository.GetAll();

        var model = new ManageUserRolesViewModel
        {
            UserId = applicationUser.Id,
            UserName = applicationUser.UserName!,
            Roles = allRoles.Select(role => new UserRoleViewModel
            {
                RoleName = role.Name!,
                IsSelected = userRoles.Contains(role.Name!)
            }).ToList()
        };

        ViewBag.User = applicationUser;
        
        // Show validation errors
        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        TempData["ErrorMessage"] = "Please fix the following errors: " + string.Join(", ", errors);
        return View(model);
    }

    // POST: ApplicationUser/ManageRoles
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManageRoles(string userId, string selectedRole)
    {
        var user = _userRepository.GetById(userId);
        if (user == null) return NotFound();

        var currentRoles = _userRepository.GetRoles(user);
        var result = _userRepository.RemoveFromRoles(user, currentRoles);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot remove user's existing roles");
            
            // Repopulate the model for the view
            var userRoles = _userRepository.GetRoles(user);
            var allRoles = _roleRepository.GetAll();

            var model = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName!,
                Roles = allRoles.Select(role => new UserRoleViewModel
                {
                    RoleName = role.Name!,
                    IsSelected = userRoles.Contains(role.Name!)
                }).ToList()
            };

            ViewBag.User = user;
            return View("Edit", model);
        }

        // Add the selected role if one was chosen
        if (!string.IsNullOrEmpty(selectedRole))
        {
            result = _userRepository.AddToRoles(user, new[] { selectedRole });

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected role to user");
                
                // Repopulate the model for the view
                var userRoles = _userRepository.GetRoles(user);
                var allRoles = _roleRepository.GetAll();

                var model = new ManageUserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName!,
                    Roles = allRoles.Select(role => new UserRoleViewModel
                    {
                        RoleName = role.Name!,
                        IsSelected = userRoles.Contains(role.Name!)
                    }).ToList()
                };

                ViewBag.User = user;
                return View("Edit", model);
            }
        }

        TempData["SuccessMessage"] = $"User role updated successfully! User now has the '{selectedRole}' role.";
        return RedirectToAction(nameof(Index));
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
            var result = _userRepository.Delete(applicationUser);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User '{applicationUser.FirstName} {applicationUser.LastName}' deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to delete user '{applicationUser.FirstName} {applicationUser.LastName}'. Please try again.";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "User not found or already deleted.";
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