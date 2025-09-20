using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace star_events.Controllers;

[Authorize(Roles = "Admin,EventOrganizer")]
public class AdminController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public AdminController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }
}