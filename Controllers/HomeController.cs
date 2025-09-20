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
        private readonly ILocationRepository _locationRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IEventRepository eventRepository, ILocationRepository locationRepository, ICategoryRepository categoryRepository, IPromotionRepository promotionRepository, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _locationRepository = locationRepository;
            _categoryRepository = categoryRepository;
            _promotionRepository = promotionRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get all events from repository
                var events = _eventRepository.GetAll();
                
                // Get locations and categories for filters
                var locations = _locationRepository.GetAll();
                var categories = _categoryRepository.GetAll();

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

                // Pass filter data to view
                ViewBag.Locations = locations;
                ViewBag.Categories = categories;

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

        // Event Details Page
        public async Task<IActionResult> Event(int id)
        {
            try
            {
                var eventItem = _eventRepository.GetById(id);
                if (eventItem == null)
                {
                    return NotFound();
                }

                return View("Event/Index", eventItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading event details for ID: {EventId}", id);
                return NotFound();
            }
        }

        // Customer Dashboard
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    ViewBag.CurrentClientName = $"{user.FirstName} {user.LastName}";
                    ViewBag.CurrentClientFName = user.FirstName;
                }

                return View("Dashboard/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard");
                return View("Dashboard/Index");
            }
        }

        // Customer Seat Selection Page
        public async Task<IActionResult> SelectSeats(int id)
        {
            try
            {
                var eventItem = _eventRepository.GetById(id);
                if (eventItem == null)
                {
                    return NotFound();
                }

                return View("Booking/Index", eventItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading seat selection for event ID: {EventId}", id);
                return NotFound();
            }
        }

        // Payment Page
        public IActionResult Payment()
        {
            return View("Payment/Pay");
        }

        // User Profile Page
        public async Task<IActionResult> Profile()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                ViewBag.CurrentClientName = $"{user.FirstName} {user.LastName}";
                ViewBag.CurrentClientFName = user.FirstName;

                return View("Profile/Index", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user profile");
                return RedirectToAction("Index");
            }
        }

        // Update User Profile
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileModel model)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                // Update user properties
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Profile updated successfully" });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Json(new { success = false, message = errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile");
                return Json(new { success = false, message = "An error occurred while updating your profile" });
            }
        }

        // Change Password
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordModel model)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Password changed successfully" });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Json(new { success = false, message = errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return Json(new { success = false, message = "An error occurred while changing your password" });
            }
        }

        // Get User Loyalty Points
        [HttpGet]
        public async Task<IActionResult> GetUserLoyaltyPoints()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { loyaltyPoints = 0 });
                }

                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    return Json(new { loyaltyPoints = user.LoyaltyPoints });
                }

                return Json(new { loyaltyPoints = 0 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user loyalty points");
                return Json(new { loyaltyPoints = 0 });
            }
        }

        // Validate Promo Code
        [HttpPost]
        public IActionResult ValidatePromoCode([FromBody] PromoCodeRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PromoCode))
                {
                    return Json(new { success = false, message = "Please enter a promo code." });
                }

                // Get all active promotions
                var promotions = _promotionRepository.GetAll()
                    .Where(p => p.IsActive && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now)
                    .ToList();

                var promotion = promotions.FirstOrDefault(p => p.PromoCode.ToUpper() == request.PromoCode.ToUpper());

                if (promotion == null)
                {
                    return Json(new { success = false, message = "Invalid or expired promo code." });
                }

                // Calculate discount
                decimal discountAmount = 0;
                if (promotion.DiscountType == "Percentage")
                {
                    discountAmount = request.Amount * (promotion.DiscountValue / 100);
                    // Cap percentage discount at 50%
                    discountAmount = Math.Min(discountAmount, request.Amount * 0.5m);
                }
                else if (promotion.DiscountType == "Fixed")
                {
                    discountAmount = Math.Min(promotion.DiscountValue, request.Amount);
                }

                return Json(new { 
                    success = true, 
                    message = $"Promo code applied! You saved LKR {discountAmount:F0}.",
                    discountAmount = discountAmount,
                    promoCode = promotion
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating promo code");
                return Json(new { success = false, message = "Error validating promo code. Please try again." });
            }
        }

        // Process Payment (Enhanced with Promo Codes and Loyalty Points)
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromForm] EnhancedPaymentModel model)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                // Validate loyalty points usage
                if (model.LoyaltyPointsUsed > user.LoyaltyPoints)
                {
                    return Json(new { success = false, message = "Insufficient loyalty points." });
                }

                // Simulate payment processing
                var random = new Random();
                var isSuccess = random.Next(1, 10) > 2; // 80% success rate

                if (isSuccess)
                {
                    // Calculate loyalty points earned (1% of original amount, minimum 10 points)
                    var loyaltyPointsEarned = Math.Max(10, (int)(model.OriginalAmount * 0.01m));

                    // Update user loyalty points
                    user.LoyaltyPoints = user.LoyaltyPoints - model.LoyaltyPointsUsed + loyaltyPointsEarned;
                    await _userManager.UpdateAsync(user);

                    // Generate QR code data
                    var qrData = GenerateQRCodeData();
                    
                    return Json(new { 
                        success = true, 
                        message = "Payment processed successfully!",
                        qrCode = qrData,
                        loyaltyPointsEarned = loyaltyPointsEarned
                    });
                }
                else
                {
                    return Json(new { 
                        success = false, 
                        message = "Payment failed. Please try again or use a different payment method."
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return Json(new { 
                    success = false, 
                    message = "An error occurred while processing your payment. Please try again."
                });
            }
        }

        private string GenerateQRCodeData()
        {
            // Generate a simple QR code data string
            var bookingId = Guid.NewGuid().ToString("N")[..8].ToUpper();
            var eventId = "EVT001";
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            
            return $"BOOKING:{bookingId}|EVENT:{eventId}|TIME:{timestamp}|STATUS:CONFIRMED";
        }
    }

    // Payment Model for form binding
    public class PaymentModel
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
    }

    // Profile Update Model
    public class UpdateProfileModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    // Change Password Model
    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    // Promo Code Request Model
    public class PromoCodeRequest
    {
        public string PromoCode { get; set; }
        public decimal Amount { get; set; }
    }

    // Enhanced Payment Model
    public class EnhancedPaymentModel
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal LoyaltyDiscountAmount { get; set; }
        public int LoyaltyPointsUsed { get; set; }
        public string PromoCode { get; set; }
    }
}
