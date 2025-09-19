using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using star_events.Models;
using star_events.Repository.Interfaces;
using System.Security.Claims;

namespace star_events.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketType> _ticketTypeRepository;

        public ReportsController(
            IUserRepository userRepository,
            IGenericRepository<Event> eventRepository,
            IGenericRepository<Booking> bookingRepository,
            IGenericRepository<Payment> paymentRepository,
            IGenericRepository<Ticket> ticketRepository,
            IGenericRepository<TicketType> ticketTypeRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
            _ticketRepository = ticketRepository;
            _ticketTypeRepository = ticketTypeRepository;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _userRepository.GetById(currentUserId);
            var userRoles = _userRepository.GetRoles(currentUser);

            var viewModel = new ReportsViewModel
            {
                IsAdmin = userRoles.Contains("Admin"),
                IsEventOrganizer = userRoles.Contains("EventOrganizer"),
                CurrentUserId = currentUserId
            };

            // Get basic statistics based on role
            if (userRoles.Contains("Admin"))
            {
                // Admin can see all data
                viewModel.TotalEvents = _eventRepository.GetAll().Count();
                viewModel.TotalBookings = _bookingRepository.GetAll().Count();
                viewModel.TotalRevenue = _paymentRepository.GetAll()
                    .Where(p => p.PaymentStatus == "Completed")
                    .Sum(p => p.AmountPaid);
                viewModel.TotalUsers = _userRepository.GetAll().Count();

                // Get recent activities for all users
                viewModel.RecentBookings = _bookingRepository.GetAll()
                    .OrderByDescending(b => b.BookingDateTime)
                    .Take(10)
                    .ToList();

                viewModel.RecentPayments = _paymentRepository.GetAll()
                    .OrderByDescending(p => p.PaymentDateTime)
                    .Take(10)
                    .ToList();
            }
            else if (userRoles.Contains("EventOrganizer"))
            {
                // EventOrganizer can only see their own events and related data
                var organizerEvents = _eventRepository.GetAll()
                    .Where(e => e.OrganizerID == currentUserId)
                    .ToList();

                viewModel.TotalEvents = organizerEvents.Count;
                
                // Get bookings for organizer's events
                var organizerEventIds = organizerEvents.Select(e => e.EventID).ToList();
                var organizerBookings = _bookingRepository.GetAll()
                    .Where(b => b.Tickets.Any(t => organizerEventIds.Contains(t.TicketType.EventID)))
                    .ToList();

                viewModel.TotalBookings = organizerBookings.Count;
                viewModel.TotalRevenue = organizerBookings
                    .Where(b => b.Status == "Completed")
                    .Sum(b => b.TotalAmount);
                viewModel.TotalUsers = organizerBookings.Select(b => b.UserID).Distinct().Count();

                // Get recent activities for organizer's events
                viewModel.RecentBookings = organizerBookings
                    .OrderByDescending(b => b.BookingDateTime)
                    .Take(10)
                    .ToList();

                viewModel.RecentPayments = _paymentRepository.GetAll()
                    .Where(p => organizerBookings.Select(b => b.BookingID).Contains(p.BookingID))
                    .OrderByDescending(p => p.PaymentDateTime)
                    .Take(10)
                    .ToList();
            }

            return View(viewModel);
        }

        // GET: Reports/Revenue
        public async Task<IActionResult> Revenue(DateTime? startDate, DateTime? endDate, int? eventId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            if (!userRoles.Contains("Admin") && !userRoles.Contains("EventOrganizer"))
            {
                return Forbid();
            }

            var query = _paymentRepository.GetAll()
                .Where(p => p.PaymentStatus == "Completed");

            // Apply role-based filtering
            if (userRoles.Contains("EventOrganizer"))
            {
                // EventOrganizer can only see payments for their events
                var organizerEventIds = _eventRepository.GetAll()
                    .Where(e => e.OrganizerID == currentUserId)
                    .Select(e => e.EventID)
                    .ToList();

                query = query.Where(p => p.Booking.Tickets.Any(t => organizerEventIds.Contains(t.TicketType.EventID)));
            }

            // Apply date filters
            if (startDate.HasValue)
                query = query.Where(p => p.PaymentDateTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.PaymentDateTime <= endDate.Value);

            // Apply event filter
            if (eventId.HasValue)
            {
                query = query.Where(p => p.Booking.Tickets.Any(t => t.TicketType.EventID == eventId.Value));
            }

            var payments = query.ToList();

            // Get available events for filter dropdown
            var availableEvents = userRoles.Contains("Admin") 
                ? _eventRepository.GetAll().ToList()
                : _eventRepository.GetAll().Where(e => e.OrganizerID == currentUserId).ToList();

            var viewModel = new RevenueReportViewModel
            {
                StartDate = startDate ?? DateTime.Now.AddMonths(-1),
                EndDate = endDate ?? DateTime.Now,
                SelectedEventId = eventId,
                AvailableEvents = availableEvents,
                Payments = payments,
                TotalRevenue = payments.Sum(p => p.AmountPaid),
                PaymentMethodBreakdown = payments.GroupBy(p => p.PaymentMethod)
                    .Select(g => new PaymentMethodSummary
                    {
                        Method = g.Key,
                        Count = g.Count(),
                        TotalAmount = g.Sum(p => p.AmountPaid)
                    }).ToList()
            };

            return View(viewModel);
        }

        // GET: Reports/Events
        public async Task<IActionResult> Events(DateTime? startDate, DateTime? endDate, int? eventId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            if (!userRoles.Contains("Admin") && !userRoles.Contains("EventOrganizer"))
            {
                return Forbid();
            }

            var query = _eventRepository.GetAll();

            // Apply role-based filtering
            if (userRoles.Contains("EventOrganizer"))
            {
                // EventOrganizer can only see their own events
                query = query.Where(e => e.OrganizerID == currentUserId);
            }

            // Apply date filters
            if (startDate.HasValue)
                query = query.Where(e => e.StartDateTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.StartDateTime <= endDate.Value);

            // Apply specific event filter
            if (eventId.HasValue)
            {
                query = query.Where(e => e.EventID == eventId.Value);
            }

            var events = query.ToList();

            // Get available events for filter dropdown
            var availableEvents = userRoles.Contains("Admin") 
                ? _eventRepository.GetAll().ToList()
                : _eventRepository.GetAll().Where(e => e.OrganizerID == currentUserId).ToList();

            var viewModel = new EventsReportViewModel
            {
                StartDate = startDate ?? DateTime.Now.AddMonths(-1),
                EndDate = endDate ?? DateTime.Now,
                SelectedEventId = eventId,
                AvailableEvents = availableEvents,
                Events = events,
                TotalEvents = events.Count,
                ActiveEvents = events.Count(e => e.Status == "Active"),
                CompletedEvents = events.Count(e => e.Status == "Completed"),
                CancelledEvents = events.Count(e => e.Status == "Cancelled")
            };

            return View(viewModel);
        }
        
        // GET: Reports/Users (Admin only)
        public async Task<IActionResult> Users(DateTime? startDate, DateTime? endDate, string? role)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            if (!userRoles.Contains("Admin"))
            {
                return Forbid();
            }

            var query = _userRepository.GetAll().AsQueryable();

            // Note: IdentityUser doesn't have CreatedAt by default, so we'll skip date filtering for now
            // In a real application, you would add a CreatedAt property to ApplicationUser

            // Apply role filter
            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(u => _userRepository.GetRoles(u).Contains(role));
            }

            var users = query.ToList();

            var viewModel = new UsersReportViewModel
            {
                StartDate = startDate ?? DateTime.Now.AddMonths(-1),
                EndDate = endDate ?? DateTime.Now,
                SelectedRole = role,
                Users = users,
                TotalUsers = users.Count,
                AdminUsers = users.Count(u => _userRepository.GetRoles(u).Contains("Admin")),
                EventOrganizerUsers = users.Count(u => _userRepository.GetRoles(u).Contains("EventOrganizer")),
                CustomerUsers = users.Count(u => _userRepository.GetRoles(u).Contains("Customer"))
            };

            // Pass user roles to view
            ViewBag.UserRoles = users.ToDictionary(u => u.Id, u => _userRepository.GetRoles(u).ToList());

            return View(viewModel);
        }

        // GET: Reports/TicketSales (EventOrganizer only)
        public async Task<IActionResult> TicketSales(DateTime? startDate, DateTime? endDate, int? eventId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            if (!userRoles.Contains("EventOrganizer"))
            {
                return Forbid();
            }

            // Get organizer's events
            var organizerEventIds = _eventRepository.GetAll()
                .Where(e => e.OrganizerID == currentUserId)
                .Select(e => e.EventID)
                .ToList();

            var query = _ticketRepository.GetAll()
                .Where(t => organizerEventIds.Contains(t.TicketType.EventID));

            // Apply date filters
            if (startDate.HasValue)
                query = query.Where(t => t.Booking.BookingDateTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.Booking.BookingDateTime <= endDate.Value);

            // Apply event filter
            if (eventId.HasValue)
            {
                query = query.Where(t => t.TicketType.EventID == eventId.Value);
            }

            var tickets = query.ToList();

            // Get available events for filter dropdown
            var availableEvents = _eventRepository.GetAll()
                .Where(e => e.OrganizerID == currentUserId)
                .ToList();

            var viewModel = new TicketSalesReportViewModel
            {
                StartDate = startDate ?? DateTime.Now.AddMonths(-1),
                EndDate = endDate ?? DateTime.Now,
                SelectedEventId = eventId,
                AvailableEvents = availableEvents,
                Tickets = tickets,
                TotalTicketsSold = tickets.Count,
                TotalRevenue = tickets.Sum(t => t.TicketType.Price),
                ScannedTickets = tickets.Count(t => t.IsScanned),
                UnscannedTickets = tickets.Count(t => !t.IsScanned)
            };

            return View(viewModel);
        }

        // POST: Reports/Export
        [HttpPost]
        public async Task<IActionResult> Export(string reportType, string format, DateTime? startDate, DateTime? endDate, int? eventId, string? role)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            if (!userRoles.Contains("Admin") && !userRoles.Contains("EventOrganizer"))
            {
                return Forbid();
            }

            // For now, return a simple CSV export
            // In a real application, you would implement proper PDF/Excel generation
            var csvContent = GenerateCSVReport(reportType, startDate, endDate, eventId, role, currentUserId, userRoles);
            var fileName = $"{reportType}_Report_{DateTime.Now:yyyyMMdd}.csv";

            return File(System.Text.Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
        }

        private string GenerateCSVReport(string reportType, DateTime? startDate, DateTime? endDate, int? eventId, string? role, string currentUserId, IEnumerable<string> userRoles)
        {
            switch (reportType.ToLower())
            {
                case "revenue":
                    return GenerateRevenueCSV(startDate, endDate, eventId, currentUserId, userRoles);
                case "events":
                    return GenerateEventsCSV(startDate, endDate, eventId, currentUserId, userRoles);
                case "bookings":
                    return GenerateBookingsCSV(startDate, endDate, eventId, currentUserId, userRoles);
                case "users":
                    return GenerateUsersCSV(startDate, endDate, role);
                case "ticketsales":
                    return GenerateTicketSalesCSV(startDate, endDate, eventId, currentUserId);
                default:
                    return "Invalid report type";
            }
        }

        private string GenerateRevenueCSV(DateTime? startDate, DateTime? endDate, int? eventId, string currentUserId, IEnumerable<string> userRoles)
        {
            var payments = _paymentRepository.GetAll()
                .Where(p => p.PaymentStatus == "Completed");

            // Apply role-based filtering
            if (userRoles.Contains("EventOrganizer"))
            {
                var organizerEventIds = _eventRepository.GetAll()
                    .Where(e => e.OrganizerID == currentUserId)
                    .Select(e => e.EventID)
                    .ToList();

                payments = payments.Where(p => p.Booking.Tickets.Any(t => organizerEventIds.Contains(t.TicketType.EventID)));
            }

            if (startDate.HasValue)
                payments = payments.Where(p => p.PaymentDateTime >= startDate.Value);

            if (endDate.HasValue)
                payments = payments.Where(p => p.PaymentDateTime <= endDate.Value);

            if (eventId.HasValue)
                payments = payments.Where(p => p.Booking.Tickets.Any(t => t.TicketType.EventID == eventId.Value));

            var csv = "Payment ID,Amount,Payment Method,Status,Date,Transaction ID,Event Title\n";
            foreach (var payment in payments)
            {
                var eventTitle = payment.Booking.Tickets.FirstOrDefault()?.TicketType?.Event?.Title ?? "N/A";
                csv += $"{payment.PaymentID},{payment.AmountPaid},{payment.PaymentMethod},{payment.PaymentStatus},{payment.PaymentDateTime:yyyy-MM-dd HH:mm},{payment.PaymentGatewayTransactionID},{eventTitle}\n";
            }
            return csv;
        }

        private string GenerateEventsCSV(DateTime? startDate, DateTime? endDate, int? eventId, string currentUserId, IEnumerable<string> userRoles)
        {
            var events = _eventRepository.GetAll();

            // Apply role-based filtering
            if (userRoles.Contains("EventOrganizer"))
            {
                events = events.Where(e => e.OrganizerID == currentUserId);
            }

            if (startDate.HasValue)
                events = events.Where(e => e.StartDateTime >= startDate.Value);

            if (endDate.HasValue)
                events = events.Where(e => e.StartDateTime <= endDate.Value);

            if (eventId.HasValue)
                events = events.Where(e => e.EventID == eventId.Value);

            var csv = "Event ID,Title,Start Date,End Date,Status,Description,Organizer,Location\n";
            foreach (var evt in events)
            {
                csv += $"{evt.EventID},{evt.Title},{evt.StartDateTime:yyyy-MM-dd HH:mm},{evt.EndDateTime:yyyy-MM-dd HH:mm},{evt.Status},\"{evt.Description}\",{evt.OrganizerName},{evt.Venue}\n";
            }
            return csv;
        }

        private string GenerateBookingsCSV(DateTime? startDate, DateTime? endDate, int? eventId, string currentUserId, IEnumerable<string> userRoles)
        {
            var bookings = _bookingRepository.GetAll();

            // Apply role-based filtering
            if (userRoles.Contains("EventOrganizer"))
            {
                var organizerEventIds = _eventRepository.GetAll()
                    .Where(e => e.OrganizerID == currentUserId)
                    .Select(e => e.EventID)
                    .ToList();

                bookings = bookings.Where(b => b.Tickets.Any(t => organizerEventIds.Contains(t.TicketType.EventID)));
            }

            if (startDate.HasValue)
                bookings = bookings.Where(b => b.BookingDateTime >= startDate.Value);

            if (endDate.HasValue)
                bookings = bookings.Where(b => b.BookingDateTime <= endDate.Value);

            if (eventId.HasValue)
                bookings = bookings.Where(b => b.Tickets.Any(t => t.TicketType.EventID == eventId.Value));

            var csv = "Booking ID,Total Amount,Status,Booking Date,Customer Email,Event Title\n";
            foreach (var booking in bookings)
            {
                var eventTitle = booking.Tickets.FirstOrDefault()?.TicketType?.Event?.Title ?? "N/A";
                csv += $"{booking.BookingID},{booking.TotalAmount},{booking.Status},{booking.BookingDateTime:yyyy-MM-dd HH:mm},{booking.User?.Email},{eventTitle}\n";
            }
            return csv;
        }

        private string GenerateUsersCSV(DateTime? startDate, DateTime? endDate, string? role)
        {
            var users = _userRepository.GetAll().AsQueryable();

            // Note: IdentityUser doesn't have CreatedAt by default, so we'll skip date filtering for now
            // In a real application, you would add a CreatedAt property to ApplicationUser

            if (!string.IsNullOrEmpty(role))
            {
                users = users.Where(u => _userRepository.GetRoles(u).Contains(role));
            }

            var csv = "User ID,Email,First Name,Last Name,Roles\n";
            foreach (var user in users)
            {
                var userRoles = string.Join(";", _userRepository.GetRoles(user));
                csv += $"{user.Id},{user.Email},{user.FirstName},{user.LastName},{userRoles}\n";
            }
            return csv;
        }

        private string GenerateTicketSalesCSV(DateTime? startDate, DateTime? endDate, int? eventId, string currentUserId)
        {
            var organizerEventIds = _eventRepository.GetAll()
                .Where(e => e.OrganizerID == currentUserId)
                .Select(e => e.EventID)
                .ToList();

            var tickets = _ticketRepository.GetAll()
                .Where(t => organizerEventIds.Contains(t.TicketType.EventID));

            if (startDate.HasValue)
                tickets = tickets.Where(t => t.Booking.BookingDateTime >= startDate.Value);

            if (endDate.HasValue)
                tickets = tickets.Where(t => t.Booking.BookingDateTime <= endDate.Value);

            if (eventId.HasValue)
                tickets = tickets.Where(t => t.TicketType.EventID == eventId.Value);

            var csv = "Ticket ID,Ticket Type,Price,Event Title,Booking Date,Scanned,Scanned Date,Customer Email\n";
            foreach (var ticket in tickets)
            {
                csv += $"{ticket.TicketID},{ticket.TicketType.Name},{ticket.TicketType.Price},{ticket.TicketType.Event.Title},{ticket.Booking.BookingDateTime:yyyy-MM-dd HH:mm},{ticket.IsScanned},{ticket.ScannedAt?.ToString("yyyy-MM-dd HH:mm") ?? "N/A"},{ticket.Booking.User?.Email}\n";
            }
            return csv;
        }
    }

    // View Models
    public class ReportsViewModel
    {
        public bool IsAdmin { get; set; }
        public bool IsEventOrganizer { get; set; }
        public string CurrentUserId { get; set; }
        public int TotalEvents { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalUsers { get; set; }
        public List<Booking> RecentBookings { get; set; } = new();
        public List<Payment> RecentPayments { get; set; } = new();
    }

    public class RevenueReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? SelectedEventId { get; set; }
        public List<Event> AvailableEvents { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();
        public decimal TotalRevenue { get; set; }
        public List<PaymentMethodSummary> PaymentMethodBreakdown { get; set; } = new();
    }

    public class EventsReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? SelectedEventId { get; set; }
        public List<Event> AvailableEvents { get; set; } = new();
        public List<Event> Events { get; set; } = new();
        public int TotalEvents { get; set; }
        public int ActiveEvents { get; set; }
        public int CompletedEvents { get; set; }
        public int CancelledEvents { get; set; }
    }

    public class BookingsReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? SelectedEventId { get; set; }
        public List<Event> AvailableEvents { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int PendingBookings { get; set; }
        public int CancelledBookings { get; set; }
        public decimal TotalBookingValue { get; set; }
    }

    public class UsersReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? SelectedRole { get; set; }
        public List<ApplicationUser> Users { get; set; } = new();
        public int TotalUsers { get; set; }
        public int AdminUsers { get; set; }
        public int EventOrganizerUsers { get; set; }
        public int CustomerUsers { get; set; }
    }

    public class TicketSalesReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? SelectedEventId { get; set; }
        public List<Event> AvailableEvents { get; set; } = new();
        public List<Ticket> Tickets { get; set; } = new();
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ScannedTickets { get; set; }
        public int UnscannedTickets { get; set; }
    }

    public class PaymentMethodSummary
    {
        public string Method { get; set; }
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
