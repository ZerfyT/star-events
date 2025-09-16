using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using star_events.Data;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        public BookingController(IBookingRepository bookingRepository, IUserRepository userRepository, ApplicationDbContext context)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            var viewModel = new BookingIndexViewModel
            {
                IsAdmin = userRoles.Contains("Admin"),
                IsEventOrganizer = userRoles.Contains("EventOrganizer")
            };

            // Get all bookings with related data
            var bookings = _bookingRepository.GetBookingsWithDetails()
                .OrderByDescending(b => b.BookingDateTime)
                .ToList();

            viewModel.Bookings = bookings;
            viewModel.TotalBookings = bookings.Count;
            viewModel.TotalRevenue = bookings.Sum(b => b.TotalAmount);
            viewModel.ConfirmedBookings = bookings.Count(b => b.Status == "Confirmed");
            viewModel.PendingBookings = bookings.Count(b => b.Status == "Pending");
            viewModel.CancelledBookings = bookings.Count(b => b.Status == "Cancelled");

            return View(viewModel);
        }

        // GET: Booking/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = _userRepository.GetRoles(_userRepository.GetById(currentUserId));

            var booking = _bookingRepository.GetBookingWithDetails(id.Value);
            if (booking == null)
            {
                return NotFound();
            }

            var viewModel = new BookingDetailsViewModel
            {
                Booking = booking,
                IsAdmin = userRoles.Contains("Admin"),
                IsEventOrganizer = userRoles.Contains("EventOrganizer")
            };

            return View(viewModel);
        }

    }

    public class BookingIndexViewModel
    {
        public bool IsAdmin { get; set; }
        public bool IsEventOrganizer { get; set; }
        public List<Booking> Bookings { get; set; } = new();
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ConfirmedBookings { get; set; }
        public int PendingBookings { get; set; }
        public int CancelledBookings { get; set; }
    }
}
