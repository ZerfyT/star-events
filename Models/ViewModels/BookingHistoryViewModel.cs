using star_events.Models;

namespace star_events.Models.ViewModels
{
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
}
