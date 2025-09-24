using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Controllers;

public class HomeController : Controller
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger<HomeController> _logger;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPromotionRepository _promotionRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, IEventRepository eventRepository,
        ILocationRepository locationRepository, ICategoryRepository categoryRepository,
        IPromotionRepository promotionRepository, IBookingRepository bookingRepository,
        ITicketRepository ticketRepository, IPaymentRepository paymentRepository,
        ITicketTypeRepository ticketTypeRepository, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _eventRepository = eventRepository;
        _locationRepository = locationRepository;
        _categoryRepository = categoryRepository;
        _promotionRepository = promotionRepository;
        _bookingRepository = bookingRepository;
        _ticketRepository = ticketRepository;
        _paymentRepository = paymentRepository;
        _ticketTypeRepository = ticketTypeRepository;
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
            if (eventItem == null) return NotFound();

            // Get ticket types for this event
            var ticketTypes = _ticketTypeRepository.GetAll()
                .Where(tt => tt.EventID == id)
                .ToList();

            ViewBag.TicketTypes = ticketTypes;

            return View("Event/Index", eventItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading event details for ID: {EventId}", id);
            return NotFound();
        }
    }

    [Authorize]
    // Customer Dashboard
    public async Task<IActionResult> Dashboard()
    {
        try
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

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
    public async Task<IActionResult> SelectSeats(int id, string selectedTickets)
    {
        try
        {
            var eventItem = _eventRepository.GetById(id);
            if (eventItem == null) return NotFound();

            // Parse selected tickets from query string
            var tickets = new List<SelectedTicketType>();
            if (!string.IsNullOrEmpty(selectedTickets))
            {
                var ticketData = System.Web.HttpUtility.UrlDecode(selectedTickets);
                tickets = System.Text.Json.JsonSerializer.Deserialize<List<SelectedTicketType>>(ticketData) ?? new List<SelectedTicketType>();
            }

            ViewBag.SelectedTickets = tickets;
            ViewBag.TotalAmount = tickets.Sum(t => t.Price * t.Quantity);

            return View("Booking/Index", eventItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading seat selection for event ID: {EventId}", id);
            return NotFound();
        }
    }

    // Process Booking and Navigate to Payment
    [HttpPost]
    public IActionResult ProcessBooking([FromForm] BookingFormModel model)
    {
        try
        {
            // Check if model is null
            if (model == null)
            {
                _logger.LogError("ProcessBooking called with null model");
                return Json(new { success = false, message = "Invalid booking data received" });
            }

            _logger.LogInformation("ProcessBooking called with EventId: {EventId}, TotalAmount: {TotalAmount}", 
                model.EventId, model.TotalAmount);
            _logger.LogInformation("SelectedSeatsJson: {SelectedSeatsJson}", model.SelectedSeatsJson ?? "NULL");
            _logger.LogInformation("SelectedTicketsJson: {SelectedTicketsJson}", model.SelectedTicketsJson ?? "NULL");

            // Validate required fields
            if (model.EventId <= 0)
            {
                _logger.LogError("Invalid EventId: {EventId}", model.EventId);
                return Json(new { success = false, message = "Invalid event selection" });
            }

            if (model.TotalAmount <= 0)
            {
                _logger.LogError("Invalid TotalAmount: {TotalAmount}", model.TotalAmount);
                return Json(new { success = false, message = "Invalid total amount" });
            }

            // Deserialize the JSON strings from form data
            var selectedSeats = new List<SelectedSeat>();
            var selectedTickets = new List<SelectedTicketType>();

            if (!string.IsNullOrEmpty(model.SelectedSeatsJson))
            {
                try
                {
                    selectedSeats = System.Text.Json.JsonSerializer.Deserialize<List<SelectedSeat>>(model.SelectedSeatsJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<SelectedSeat>();
                    _logger.LogInformation("Deserialized {Count} selected seats", selectedSeats.Count);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Error deserializing SelectedSeatsJson: {Json}", model.SelectedSeatsJson);
                    throw;
                }
            }

            if (!string.IsNullOrEmpty(model.SelectedTicketsJson))
            {
                try
                {
                    selectedTickets = System.Text.Json.JsonSerializer.Deserialize<List<SelectedTicketType>>(model.SelectedTicketsJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<SelectedTicketType>();
                    _logger.LogInformation("Deserialized {Count} selected tickets", selectedTickets.Count);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Error deserializing SelectedTicketsJson: {Json}", model.SelectedTicketsJson);
                    throw;
                }
            }

            // Validate that we have at least some booking data
            if (selectedSeats.Count == 0 && selectedTickets.Count == 0)
            {
                _logger.LogError("No seats or tickets selected");
                return Json(new { success = false, message = "Please select at least one seat or ticket" });
            }

            // Store booking data in session for payment page
            var bookingData = new BookingSessionData
            {
                EventId = model.EventId,
                SelectedSeats = selectedSeats,
                SelectedTickets = selectedTickets,
                TotalAmount = model.TotalAmount
            };

            // Store in session
            HttpContext.Session.SetString("BookingData", System.Text.Json.JsonSerializer.Serialize(bookingData));
            _logger.LogInformation("Booking data stored in session successfully");

            // Redirect to payment page
            return RedirectToAction("Payment");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing booking: {Error}", ex.Message);
            _logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
            return Json(new { success = false, message = "Error processing booking: " + ex.Message });
        }
    }

    // Alternative Process Booking method for debugging
    [HttpPost]
    public IActionResult ProcessBookingRaw(IFormCollection form)
    {
        try
        {
            _logger.LogInformation("ProcessBookingRaw called with {Count} form fields", form.Count);
            
            foreach (var key in form.Keys)
            {
                _logger.LogInformation("Form field {Key}: {Value}", key, form[key]);
            }

            var eventIdStr = form["EventId"].FirstOrDefault();
            var selectedSeatsJson = form["SelectedSeatsJson"].FirstOrDefault();
            var selectedTicketsJson = form["SelectedTicketsJson"].FirstOrDefault();
            var totalAmountStr = form["TotalAmount"].FirstOrDefault();

            if (!int.TryParse(eventIdStr, out int eventId) || eventId <= 0)
            {
                return Json(new { success = false, message = "Invalid event ID" });
            }

            if (!decimal.TryParse(totalAmountStr, out decimal totalAmount) || totalAmount <= 0)
            {
                return Json(new { success = false, message = "Invalid total amount" });
            }

            var selectedSeats = new List<SelectedSeat>();
            var selectedTickets = new List<SelectedTicketType>();

            if (!string.IsNullOrEmpty(selectedSeatsJson))
            {
                selectedSeats = System.Text.Json.JsonSerializer.Deserialize<List<SelectedSeat>>(selectedSeatsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<SelectedSeat>();
            }

            if (!string.IsNullOrEmpty(selectedTicketsJson))
            {
                selectedTickets = System.Text.Json.JsonSerializer.Deserialize<List<SelectedTicketType>>(selectedTicketsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<SelectedTicketType>();
            }

            var bookingData = new BookingSessionData
            {
                EventId = eventId,
                SelectedSeats = selectedSeats,
                SelectedTickets = selectedTickets,
                TotalAmount = totalAmount
            };

            HttpContext.Session.SetString("BookingData", System.Text.Json.JsonSerializer.Serialize(bookingData));
            return RedirectToAction("Payment");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ProcessBookingRaw: {Error}", ex.Message);
            return Json(new { success = false, message = "Error processing booking: " + ex.Message });
        }
    }

    // Payment Page
    [Authorize]
    public IActionResult Payment(int eventId, string selectedSeats, string selectedTickets, decimal totalAmount)
    {
        try
        {
            _logger.LogInformation("Payment page called with EventId: {EventId}, TotalAmount: {TotalAmount}", eventId, totalAmount);

            // Validate required parameters
            if (eventId <= 0)
            {
                _logger.LogError("Invalid EventId: {EventId}", eventId);
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(selectedSeats))
            {
                _logger.LogError("No selected seats provided");
                return RedirectToAction("Index");
            }

            if (totalAmount <= 0)
            {
                _logger.LogError("Invalid TotalAmount: {TotalAmount}", totalAmount);
                return RedirectToAction("Index");
            }

            // Deserialize the booking data from URL parameters
            var selectedSeatsList = new List<SelectedSeat>();
            var selectedTicketsList = new List<SelectedTicketType>();

            if (!string.IsNullOrEmpty(selectedSeats))
            {
                selectedSeatsList = System.Text.Json.JsonSerializer.Deserialize<List<SelectedSeat>>(selectedSeats, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<SelectedSeat>();
            }

            if (!string.IsNullOrEmpty(selectedTickets))
            {
                selectedTicketsList = System.Text.Json.JsonSerializer.Deserialize<List<SelectedTicketType>>(selectedTickets, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<SelectedTicketType>();
            }

            // Create booking data object
            var bookingData = new BookingSessionData
            {
                EventId = eventId,
                SelectedSeats = selectedSeatsList,
                SelectedTickets = selectedTicketsList,
                TotalAmount = totalAmount
            };

            ViewBag.BookingData = bookingData;
            _logger.LogInformation("Payment page loaded successfully with {SeatCount} seats and {TicketCount} tickets", 
                selectedSeatsList.Count, selectedTicketsList.Count);

            return View("Payment/Pay");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading payment page: {Error}", ex.Message);
            return RedirectToAction("Index");
        }
    }

    // User Profile Page
    public async Task<IActionResult> Profile()
    {
        try
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

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
                return Json(new { success = false, message = "User not authenticated" });

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "User not found" });

            // Update user properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Json(new { success = true, message = "Profile updated successfully" });

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { success = false, message = errors });
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
                return Json(new { success = false, message = "User not authenticated" });

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "User not found" });

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded) return Json(new { success = true, message = "Password changed successfully" });

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { success = false, message = errors });
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
            if (!User.Identity.IsAuthenticated) return Json(new { loyaltyPoints = 0 });

            var user = await _userManager.GetUserAsync(User);
            if (user != null) return Json(new { loyaltyPoints = user.LoyaltyPoints });

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
                return Json(new { success = false, message = "Please enter a promo code." });

            // Get all active promotions
            var promotions = _promotionRepository.GetAll()
                .Where(p => p.IsActive && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now)
                .ToList();

            var promotion = promotions.FirstOrDefault(p => p.PromoCode.ToUpper() == request.PromoCode.ToUpper());

            if (promotion == null) return Json(new { success = false, message = "Invalid or expired promo code." });

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

            return Json(new
            {
                success = true,
                message = $"Promo code applied! You saved LKR {discountAmount:F0}.",
                discountAmount,
                promoCode = promotion
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating promo code");
            return Json(new { success = false, message = "Error validating promo code. Please try again." });
        }
    }

    // Get User Dashboard Data
    [HttpGet]
    public async Task<IActionResult> GetDashboardData()
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { success = false, message = "User not authenticated" });

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "User not found" });

            // Get user's bookings with related data
            var userBookings = _bookingRepository.GetBookingsByUser(user.Id);
            _logger.LogInformation("Found {Count} bookings for user {UserId}", userBookings.Count(), user.Id);

            var bookings = new List<object>();
            
            try
            {
                bookings = userBookings
                    .OrderByDescending(b => b.BookingDateTime)
                    .Take(20) // Limit to last 20 bookings
                    .Select(b => new
                    {
                        bookingId = b.BookingID,
                        eventTitle = b.Event?.Title ?? "Unknown Event",
                        eventDate = b.Event?.StartDateTime ?? DateTime.MinValue,
                        eventLocation = b.Event?.Location != null ? $"{b.Event.Location.Name}, {b.Event.Location.City}" : "TBD",
                        eventImage = (b.Event?.AllImagePaths != null && b.Event.AllImagePaths.Any()) ? b.Event.AllImagePaths[0] : "/Images/default-event.jpg",
                        categoryName = b.Event?.Category?.Name ?? "General",
                        bookingDateTime = b.BookingDateTime,
                        totalAmount = b.TotalAmount,
                        status = b.Status,
                        ticketCount = b.Tickets?.Count ?? 0,
                        tickets = (b.Tickets ?? new List<Ticket>()).Select(t => new
                        {
                            ticketId = t.TicketID,
                            ticketType = t.TicketType?.Name ?? "Unknown",
                            price = t.TicketType?.Price ?? 0,
                            isScanned = t.IsScanned,
                            qrCode = t.QRCodeValue
                        }).ToList()
                    }).ToList<object>();
            }
            catch (Exception selectEx)
            {
                _logger.LogError(selectEx, "Error in Select operation for bookings");
                
                // Fallback: return basic booking data without related entities
                bookings = userBookings
                    .OrderByDescending(b => b.BookingDateTime)
                    .Take(20)
                    .Select(b => new
                    {
                        bookingId = b.BookingID,
                        eventTitle = "Event Data Unavailable",
                        eventDate = DateTime.MinValue,
                        eventLocation = "Location Unknown",
                        eventImage = "/Images/default-event.jpg",
                        categoryName = "General",
                        bookingDateTime = b.BookingDateTime,
                        totalAmount = b.TotalAmount,
                        status = b.Status,
                        ticketCount = 0,
                        tickets = new List<object>()
                    }).ToList<object>();
            }

            return Json(new { success = true, bookings });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard data");
            return Json(new { success = false, message = "Error loading dashboard data" });
        }
    }

    // Process Payment (Enhanced with Promo Codes and Loyalty Points)
    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromForm] EnhancedPaymentModel model, [FromForm] string eventId, [FromForm] string selectedSeats, [FromForm] string selectedTickets, [FromForm] string totalAmount)
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { success = false, message = "User not authenticated" });

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "User not found" });

            // Validate loyalty points usage
            if (model.LoyaltyPointsUsed > user.LoyaltyPoints)
                return Json(new { success = false, message = "Insufficient loyalty points." });

            // Get booking data from form parameters (passed from payment page)
            if (!int.TryParse(eventId, out int eventIdInt) || eventIdInt <= 0)
                return Json(new { success = false, message = "Invalid event ID." });

            if (string.IsNullOrEmpty(selectedSeats))
                return Json(new { success = false, message = "No selected seats found." });

            if (!decimal.TryParse(totalAmount, out decimal totalAmountDecimal) || totalAmountDecimal <= 0)
                return Json(new { success = false, message = "Invalid total amount." });

            // Deserialize selected seats
            var selectedSeatsList = new List<SelectedSeat>();
            if (!string.IsNullOrEmpty(selectedSeats))
            {
                selectedSeatsList = System.Text.Json.JsonSerializer.Deserialize<List<SelectedSeat>>(selectedSeats, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<SelectedSeat>();
            }

            if (selectedSeatsList.Count == 0)
                return Json(new { success = false, message = "No seats selected." });

            // Simulate payment processing
            var random = new Random();
            var isSuccess = random.Next(1, 10) > 2; // 80% success rate

            if (isSuccess)
            {
                // Create booking
                var booking = new Booking
                {
                    UserID = user.Id,
                    EventID = eventIdInt,
                    BookingDateTime = DateTime.Now,
                    TotalAmount = model.Amount,
                    Status = "Confirmed"
                };

                // Apply promotion if promo code was used
                if (!string.IsNullOrEmpty(model.PromoCode))
                {
                    var promotion = _promotionRepository.GetAll()
                        .FirstOrDefault(p => p.PromoCode.ToUpper() == model.PromoCode.ToUpper() && p.IsActive);
                    if (promotion != null)
                    {
                        booking.PromotionID = promotion.PromotionID;
                    }
                }

                _bookingRepository.Insert(booking);
                _bookingRepository.Save();

                // Create payment record
                var payment = new Payment
                {
                    BookingID = booking.BookingID,
                    PaymentGatewayTransactionID = Guid.NewGuid().ToString(),
                    AmountPaid = model.Amount,
                    PaymentMethod = "Credit Card",
                    PaymentStatus = "Completed",
                    PaymentDateTime = DateTime.Now
                };

                _paymentRepository.Insert(payment);
                _paymentRepository.Save();

                // Create tickets for each selected seat
                var createdTickets = new List<Ticket>();
                foreach (var seat in selectedSeatsList)
                {
                    var ticket = new Ticket
                    {
                        BookingID = booking.BookingID,
                        TicketTypeID = seat.TicketTypeId,
                        QRCodeValue = GenerateQRCodeData(booking.BookingID, seat.SeatId),
                        IsScanned = false
                    };

                    _ticketRepository.Insert(ticket);
                    createdTickets.Add(ticket);
                }

                _ticketRepository.Save();

                // Calculate loyalty points earned (1% of original amount, minimum 10 points)
                var loyaltyPointsEarned = Math.Max(10, (int)(model.OriginalAmount * 0.01m));

                // Update user loyalty points
                user.LoyaltyPoints = user.LoyaltyPoints - model.LoyaltyPointsUsed + loyaltyPointsEarned;
                await _userManager.UpdateAsync(user);

                // Clear session storage after successful payment
                // HttpContext.Session.Remove("BookingData");

                // Return success with ticket information
                return Json(new
                {
                    success = true,
                    message = "Payment processed successfully!",
                    bookingId = booking.BookingID,
                    ticketIds = createdTickets.Select(t => t.TicketID).ToList(),
                    qrCodes = createdTickets.Select(t => t.QRCodeValue).ToList(),
                    loyaltyPointsEarned
                });
            }

            return Json(new
            {
                success = false,
                message = "Payment failed. Please try again or use a different payment method."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment");
            return Json(new
            {
                success = false,
                message = "An error occurred while processing your payment. Please try again."
            });
        }
    }

    // QR Code Summary Page
    public IActionResult QRSummary(int? ticketId, string? qr)
    {
        if (ticketId.HasValue) ViewBag.TicketId = ticketId.Value;
        if (!string.IsNullOrEmpty(qr)) ViewBag.QRCode = qr;

        return View("QR/Summary");
    }

    // QR Code Scanner Page (for security guards)
    public IActionResult QRScanner()
    {
        return View("QR/Scan");
    }

    // QR Code Test Page
    public IActionResult QRTest()
    {
        return View("QR/Test");
    }

    private string GenerateQRCodeData(int bookingId, string seatId)
    {
        // Generate QR code data with ticket information
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var qrData = $"TICKET:{bookingId}|SEAT:{seatId}|TIME:{timestamp}|STATUS:CONFIRMED";
        return qrData;
    }

    private List<SelectedSeat> GetSelectedSeatsFromSession()
    {
        // For now, return sample data. In a real implementation, this would read from session storage
        // or be passed as part of the form data from the frontend
        return new List<SelectedSeat>
        {
            new() { SeatId = "A1", EventId = 1, TicketTypeId = 1, Price = 5000 },
            new() { SeatId = "A2", EventId = 1, TicketTypeId = 1, Price = 5000 }
        };
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
    public string SelectedSeats { get; set; }
    public string EventId { get; set; }
}

// Selected Seat Model
public class SelectedSeat
{
    public string SeatId { get; set; }
    public int EventId { get; set; }
    public int TicketTypeId { get; set; }
    public decimal Price { get; set; }
}

// Selected Ticket Type Model
public class SelectedTicketType
{
    public int TicketTypeId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

// Booking Form Model
public class BookingFormModel
{
    public int EventId { get; set; }
    public string SelectedSeatsJson { get; set; }
    public string SelectedTicketsJson { get; set; }
    public decimal TotalAmount { get; set; }
}

// Booking Session Data Model
public class BookingSessionData
{
    public int EventId { get; set; }
    public List<SelectedSeat> SelectedSeats { get; set; } = new List<SelectedSeat>();
    public List<SelectedTicketType> SelectedTickets { get; set; } = new List<SelectedTicketType>();
    public decimal TotalAmount { get; set; }
}