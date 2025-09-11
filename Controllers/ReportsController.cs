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

        public ReportsController(
            IUserRepository userRepository,
            IGenericRepository<Event> eventRepository,
            IGenericRepository<Booking> bookingRepository,
            IGenericRepository<Payment> paymentRepository,
            IGenericRepository<Ticket> ticketRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
            _ticketRepository = ticketRepository;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            var viewModel = new ReportsViewModel
            {
                IsAdmin = userRoles.Contains("Admin"),
                IsEventOrganizer = userRoles.Contains("EventOrganizer")
            };

            // Get basic statistics
            viewModel.TotalEvents = _eventRepository.GetAll().Count();
            viewModel.TotalBookings = _bookingRepository.GetAll().Count();
            viewModel.TotalRevenue = _paymentRepository.GetAll()
                .Where(p => p.PaymentStatus == "Completed")
                .Sum(p => p.AmountPaid);
            viewModel.TotalUsers = _userRepository.GetAll().Count();

            // Get recent activities
            viewModel.RecentBookings = _bookingRepository.GetAll()
                .OrderByDescending(b => b.BookingDateTime)
                .Take(10)
                .ToList();

            viewModel.RecentPayments = _paymentRepository.GetAll()
                .OrderByDescending(p => p.PaymentDateTime)
                .Take(10)
                .ToList();

            return View(viewModel);
        }

        // GET: Reports/Revenue
        public async Task<IActionResult> Revenue(DateTime? startDate, DateTime? endDate)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            if (!userRoles.Contains("Admin") && !userRoles.Contains("EventOrganizer"))
            {
                return Forbid();
            }

            var query = _paymentRepository.GetAll()
                .Where(p => p.PaymentStatus == "Completed");

            if (startDate.HasValue)
                query = query.Where(p => p.PaymentDateTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.PaymentDateTime <= endDate.Value);

            var payments = query.ToList();

            var viewModel = new RevenueReportViewModel
            {
                StartDate = startDate ?? DateTime.Now.AddMonths(-1),
                EndDate = endDate ?? DateTime.Now,
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
        public async Task<IActionResult> Events(DateTime? startDate, DateTime? endDate)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            if (!userRoles.Contains("Admin") && !userRoles.Contains("EventOrganizer"))
            {
                return Forbid();
            }

            var query = _eventRepository.GetAll();

            if (startDate.HasValue)
                query = query.Where(e => e.StartDateTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.StartDateTime <= endDate.Value);

            var events = query.ToList();

            var viewModel = new EventsReportViewModel
            {
                StartDate = startDate ?? DateTime.Now.AddMonths(-1),
                EndDate = endDate ?? DateTime.Now,
                Events = events,
                TotalEvents = events.Count,
                ActiveEvents = events.Count(e => e.Status == "Active"),
                CompletedEvents = events.Count(e => e.Status == "Completed"),
                CancelledEvents = events.Count(e => e.Status == "Cancelled")
            };

            return View(viewModel);
        }

        // GET: Reports/Bookings
        public async Task<IActionResult> Bookings(DateTime? startDate, DateTime? endDate)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            if (!userRoles.Contains("Admin") && !userRoles.Contains("EventOrganizer"))
            {
                return Forbid();
            }

            var query = _bookingRepository.GetAll();

            if (startDate.HasValue)
                query = query.Where(b => b.BookingDateTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(b => b.BookingDateTime <= endDate.Value);

            var bookings = query.ToList();

            var viewModel = new BookingsReportViewModel
            {
                StartDate = startDate ?? DateTime.Now.AddMonths(-1),
                EndDate = endDate ?? DateTime.Now,
                Bookings = bookings,
                TotalBookings = bookings.Count,
                CompletedBookings = bookings.Count(b => b.Status == "Completed"),
                PendingBookings = bookings.Count(b => b.Status == "Pending"),
                CancelledBookings = bookings.Count(b => b.Status == "Cancelled"),
                TotalBookingValue = bookings.Sum(b => b.TotalAmount)
            };

            return View(viewModel);
        }

        // POST: Reports/Export
        [HttpPost]
        public async Task<IActionResult> Export(string reportType, string format, DateTime? startDate, DateTime? endDate)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            if (!userRoles.Contains("Admin") && !userRoles.Contains("EventOrganizer"))
            {
                return Forbid();
            }

            // For now, return a simple CSV export
            // In a real application, you would implement proper PDF/Excel generation
            var csvContent = GenerateCSVReport(reportType, startDate, endDate);
            var fileName = $"{reportType}_Report_{DateTime.Now:yyyyMMdd}.csv";

            return File(System.Text.Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
        }

        private string GenerateCSVReport(string reportType, DateTime? startDate, DateTime? endDate)
        {
            switch (reportType.ToLower())
            {
                case "revenue":
                    return GenerateRevenueCSV(startDate, endDate);
                case "events":
                    return GenerateEventsCSV(startDate, endDate);
                case "bookings":
                    return GenerateBookingsCSV(startDate, endDate);
                default:
                    return "Invalid report type";
            }
        }

        private string GenerateRevenueCSV(DateTime? startDate, DateTime? endDate)
        {
            var payments = _paymentRepository.GetAll()
                .Where(p => p.PaymentStatus == "Completed");

            if (startDate.HasValue)
                payments = payments.Where(p => p.PaymentDateTime >= startDate.Value);

            if (endDate.HasValue)
                payments = payments.Where(p => p.PaymentDateTime <= endDate.Value);

            var csv = "Payment ID,Amount,Payment Method,Status,Date,Transaction ID\n";
            foreach (var payment in payments)
            {
                csv += $"{payment.PaymentID},{payment.AmountPaid},{payment.PaymentMethod},{payment.PaymentStatus},{payment.PaymentDateTime:yyyy-MM-dd HH:mm},{payment.PaymentGatewayTransactionID}\n";
            }
            return csv;
        }

        private string GenerateEventsCSV(DateTime? startDate, DateTime? endDate)
        {
            var events = _eventRepository.GetAll();

            if (startDate.HasValue)
                events = events.Where(e => e.StartDateTime >= startDate.Value);

            if (endDate.HasValue)
                events = events.Where(e => e.StartDateTime <= endDate.Value);

            var csv = "Event ID,Title,Start Date,End Date,Status,Description\n";
            foreach (var evt in events)
            {
                csv += $"{evt.EventID},{evt.Title},{evt.StartDateTime:yyyy-MM-dd HH:mm},{evt.EndDateTime:yyyy-MM-dd HH:mm},{evt.Status},\"{evt.Description}\"\n";
            }
            return csv;
        }

        private string GenerateBookingsCSV(DateTime? startDate, DateTime? endDate)
        {
            var bookings = _bookingRepository.GetAll();

            if (startDate.HasValue)
                bookings = bookings.Where(b => b.BookingDateTime >= startDate.Value);

            if (endDate.HasValue)
                bookings = bookings.Where(b => b.BookingDateTime <= endDate.Value);

            var csv = "Booking ID,Total Amount,Status,Booking Date\n";
            foreach (var booking in bookings)
            {
                csv += $"{booking.BookingID},{booking.TotalAmount},{booking.Status},{booking.BookingDateTime:yyyy-MM-dd HH:mm}\n";
            }
            return csv;
        }
    }

    // View Models
    public class ReportsViewModel
    {
        public bool IsAdmin { get; set; }
        public bool IsEventOrganizer { get; set; }
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
        public List<Payment> Payments { get; set; } = new();
        public decimal TotalRevenue { get; set; }
        public List<PaymentMethodSummary> PaymentMethodBreakdown { get; set; } = new();
    }

    public class EventsReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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
        public List<Booking> Bookings { get; set; } = new();
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int PendingBookings { get; set; }
        public int CancelledBookings { get; set; }
        public decimal TotalBookingValue { get; set; }
    }

    public class PaymentMethodSummary
    {
        public string Method { get; set; }
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
