using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Controllers;

[Authorize]
public class PaymentController : Controller
{
    private readonly IGenericRepository<Booking> _bookingRepository;
    private readonly IGenericRepository<Event> _eventRepository;
    private readonly IGenericRepository<Payment> _paymentRepository;
    private readonly IGenericRepository<Ticket> _ticketRepository;
    private readonly IUserRepository _userRepository;

    public PaymentController(
        IUserRepository userRepository,
        IGenericRepository<Payment> paymentRepository,
        IGenericRepository<Booking> bookingRepository,
        IGenericRepository<Ticket> ticketRepository,
        IGenericRepository<Event> eventRepository)
    {
        _userRepository = userRepository;
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository;
        _ticketRepository = ticketRepository;
        _eventRepository = eventRepository;
    }

    // GET: Payment
    public async Task<IActionResult> Index()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

        var viewModel = new PaymentIndexViewModel
        {
            IsAdmin = userRoles.Contains("Admin"),
            IsEventOrganizer = userRoles.Contains("EventOrganizer")
        };

        // Get all payments with related data
        var payments = _paymentRepository.GetAll()
            .OrderByDescending(p => p.PaymentDateTime)
            .ToList();

        viewModel.Payments = payments;
        viewModel.TotalPayments = payments.Count;
        viewModel.TotalRevenue = payments.Where(p => p.PaymentStatus == "Completed").Sum(p => p.AmountPaid);
        viewModel.PendingPayments = payments.Count(p => p.PaymentStatus == "Pending");
        viewModel.FailedPayments = payments.Count(p => p.PaymentStatus == "Failed");

        return View(viewModel);
    }

    // GET: Payment/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

        var payment = _paymentRepository.GetById(id.Value);
        if (payment == null)
            return NotFound();

        var viewModel = new PaymentDetailsViewModel
        {
            Payment = payment,
            IsAdmin = userRoles.Contains("Admin"),
            IsEventOrganizer = userRoles.Contains("EventOrganizer")
        };

        return View(viewModel);
    }

    // GET: Payment/BookingHistory
    public async Task<IActionResult> BookingHistory(DateTime? startDate, DateTime? endDate, string status = "")
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

        var query = _bookingRepository.GetAll()
            .OrderByDescending(b => b.BookingDateTime);

        // Apply date filters
        // if (startDate.HasValue)
        //     query = query.Where(b => b.BookingDateTime >= startDate.Value);
        //
        // if (endDate.HasValue)
        //     query = query.Where(b => b.BookingDateTime <= endDate.Value);
        //
        // // Apply status filter
        // if (!string.IsNullOrEmpty(status))
        //     query = query.Where(b => b.Status == status);

        var bookings = query.ToList();

        var viewModel = new BookingHistoryViewModel
        {
            StartDate = startDate ?? DateTime.Now.AddMonths(-1),
            EndDate = endDate ?? DateTime.Now,
            StatusFilter = status,
            Bookings = bookings,
            IsAdmin = userRoles.Contains("Admin"),
            IsEventOrganizer = userRoles.Contains("EventOrganizer"),
            TotalBookings = bookings.Count,
            TotalValue = bookings.Sum(b => b.TotalAmount),
            CompletedBookings = bookings.Count(b => b.Status == "Completed"),
            PendingBookings = bookings.Count(b => b.Status == "Pending"),
            CancelledBookings = bookings.Count(b => b.Status == "Cancelled")
        };

        return View(viewModel);
    }

    // GET: Payment/BookingDetails/5
    public async Task<IActionResult> BookingDetails(int? id)
    {
        if (id == null)
            return NotFound();

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

        var booking = _bookingRepository.GetById(id.Value);
        if (booking == null)
            return NotFound();

        // Get related tickets and payments
        var tickets = _ticketRepository.GetAll()
            .Where(t => t.Booking.BookingID == id.Value)
            .ToList();

        var payments = _paymentRepository.GetAll()
            .Where(p => p.Booking.BookingID == id.Value)
            .ToList();

        var viewModel = new BookingDetailsViewModel
        {
            Booking = booking,
            Tickets = tickets,
            Payments = payments,
            IsAdmin = userRoles.Contains("Admin"),
            IsEventOrganizer = userRoles.Contains("EventOrganizer")
        };

        return View(viewModel);
    }

    // GET: Payment/TransactionHistory
    public async Task<IActionResult> TransactionHistory(DateTime? startDate, DateTime? endDate, string status = "")
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

        var query = _paymentRepository.GetAll()
            .OrderByDescending(p => p.PaymentDateTime);

        // Apply date filters
        // if (startDate.HasValue)
        //     query = query.Where(p => p.PaymentDateTime >= startDate.Value);
        //
        // if (endDate.HasValue)
        //     query = query.Where(p => p.PaymentDateTime <= endDate.Value);
        //
        // // Apply status filter
        // if (!string.IsNullOrEmpty(status))
        //     query = query.Where(p => p.PaymentStatus == status);

        var payments = query.ToList();

        var viewModel = new TransactionHistoryViewModel
        {
            StartDate = startDate ?? DateTime.Now.AddMonths(-1),
            EndDate = endDate ?? DateTime.Now,
            StatusFilter = status,
            Payments = payments,
            IsAdmin = userRoles.Contains("Admin"),
            IsEventOrganizer = userRoles.Contains("EventOrganizer"),
            TotalTransactions = payments.Count,
            TotalAmount = payments.Where(p => p.PaymentStatus == "Completed").Sum(p => p.AmountPaid),
            CompletedTransactions = payments.Count(p => p.PaymentStatus == "Completed"),
            PendingTransactions = payments.Count(p => p.PaymentStatus == "Pending"),
            FailedTransactions = payments.Count(p => p.PaymentStatus == "Failed")
        };

        return View(viewModel);
    }

    // POST: Payment/UpdateStatus
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

        if (!userRoles.Contains("Admin") && !userRoles.Contains("EventOrganizer")) return Forbid();

        var payment = _paymentRepository.GetById(id);
        if (payment == null)
            return NotFound();

        payment.PaymentStatus = status;
        _paymentRepository.Update(payment);
        _paymentRepository.Save();

        return Json(new { success = true, message = "Payment status updated successfully" });
    }

    // POST: Payment/Export
    [HttpPost]
    public async Task<IActionResult> Export(string exportType, DateTime? startDate, DateTime? endDate,
        string status = "")
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

        if (!userRoles.Contains("Admin") && !userRoles.Contains("EventOrganizer")) return Forbid();

        var csvContent = GenerateExportData(exportType, startDate, endDate, status);
        var fileName = $"{exportType}_Export_{DateTime.Now:yyyyMMdd}.csv";

        return File(Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
    }

    private string GenerateExportData(string exportType, DateTime? startDate, DateTime? endDate, string status)
    {
        switch (exportType.ToLower())
        {
            case "payments":
                return GeneratePaymentsCSV(startDate, endDate, status);
            case "bookings":
                return GenerateBookingsCSV(startDate, endDate, status);
            default:
                return "Invalid export type";
        }
    }

    private string GeneratePaymentsCSV(DateTime? startDate, DateTime? endDate, string status)
    {
        var query = _paymentRepository.GetAll();

        if (startDate.HasValue)
            query = query.Where(p => p.PaymentDateTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.PaymentDateTime <= endDate.Value);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.PaymentStatus == status);

        var payments = query.ToList();

        var csv = "Payment ID,Amount,Payment Method,Status,Date,Transaction ID,Booking ID\n";
        foreach (var payment in payments)
            csv +=
                $"{payment.PaymentID},{payment.AmountPaid},{payment.PaymentMethod},{payment.PaymentStatus},{payment.PaymentDateTime:yyyy-MM-dd HH:mm},{payment.PaymentGatewayTransactionID},{payment.Booking?.BookingID}\n";
        return csv;
    }

    private string GenerateBookingsCSV(DateTime? startDate, DateTime? endDate, string status)
    {
        var query = _bookingRepository.GetAll();

        if (startDate.HasValue)
            query = query.Where(b => b.BookingDateTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(b => b.BookingDateTime <= endDate.Value);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(b => b.Status == status);

        var bookings = query.ToList();

        var csv = "Booking ID,Total Amount,Status,Booking Date,User Email\n";
        foreach (var booking in bookings)
            csv +=
                $"{booking.BookingID},{booking.TotalAmount},{booking.Status},{booking.BookingDateTime:yyyy-MM-dd HH:mm},{booking.User?.Email}\n";
        return csv;
    }
}

// View Models
public class PaymentIndexViewModel
{
    public bool IsAdmin { get; set; }
    public bool IsEventOrganizer { get; set; }
    public List<Payment> Payments { get; set; } = new();
    public int TotalPayments { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingPayments { get; set; }
    public int FailedPayments { get; set; }
}

public class PaymentDetailsViewModel
{
    public Payment Payment { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsEventOrganizer { get; set; }
}

public class BookingHistoryViewModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StatusFilter { get; set; }
    public List<Booking> Bookings { get; set; } = new();
    public bool IsAdmin { get; set; }
    public bool IsEventOrganizer { get; set; }
    public int TotalBookings { get; set; }
    public decimal TotalValue { get; set; }
    public int CompletedBookings { get; set; }
    public int PendingBookings { get; set; }
    public int CancelledBookings { get; set; }
}

public class BookingDetailsViewModel
{
    public Booking Booking { get; set; }
    public List<Ticket> Tickets { get; set; } = new();
    public List<Payment> Payments { get; set; } = new();
    public bool IsAdmin { get; set; }
    public bool IsEventOrganizer { get; set; }
}

public class TransactionHistoryViewModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StatusFilter { get; set; }
    public List<Payment> Payments { get; set; } = new();
    public bool IsAdmin { get; set; }
    public bool IsEventOrganizer { get; set; }
    public int TotalTransactions { get; set; }
    public decimal TotalAmount { get; set; }
    public int CompletedTransactions { get; set; }
    public int PendingTransactions { get; set; }
    public int FailedTransactions { get; set; }
}