using star_events.Models;

namespace star_events.Models.ViewModels
{
    public class BookingDetailsViewModel
    {
        public Booking Booking { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsEventOrganizer {  get; set; }
    }
}
