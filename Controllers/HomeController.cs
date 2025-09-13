using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using star_events.Data;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventRepository _eventRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IEventRepository eventRepository, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get all events from repository
                var events = _eventRepository.GetAll();

                // Get current user data if authenticated
                if (User.Identity.IsAuthenticated)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);
                        if (user != null)
                        {
                            ViewBag.CurrentClientName = $"{user.FirstName} {user.LastName}";
                            ViewBag.CurrentClientFName = user.FirstName;
                            
                            _logger.LogInformation($"Current user: {user.FirstName} {user.LastName} ({user.Email})");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error retrieving user data");
                        ViewBag.CurrentClientName = null;
                        ViewBag.CurrentClientFName = null;
                    }
                }

                return View(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading events for home page");
                return View(new List<Event>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
