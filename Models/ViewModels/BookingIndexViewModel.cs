namespace star_events.Models.ViewModels
{
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
