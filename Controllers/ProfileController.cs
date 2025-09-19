using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using star_events.Models;
using star_events.Models.ViewModels;
using star_events.Services;

namespace star_events.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<ProfileController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    // GET: Profile
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var model = new ProfileViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            ContactNo = user.ContactNo,
            Address = user.Address,
            LoyaltyPoints = user.LoyaltyPoints
        };

        return View(model);
    }

    // POST: Profile/Update
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        // Update user properties
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.ContactNo = model.ContactNo;
        user.Address = model.Address;
        user.LoyaltyPoints = model.LoyaltyPoints;

        // Update email if changed
        if (user.Email != model.Email)
        {
            user.Email = model.Email;
            user.UserName = model.Email;
        }

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Your profile has been updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View("Index", model);
    }

    // GET: Profile/ChangePassword
    public IActionResult ChangePassword()
    {
        return View();
    }

    // POST: Profile/ChangePassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            // Sign out and sign in again to refresh the authentication cookie
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
            
            TempData["SuccessMessage"] = "Your password has been changed successfully!";
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }
}
