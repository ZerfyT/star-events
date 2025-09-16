using star_events.Models;

namespace star_events.Repository.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        IEnumerable<Booking> GetBookingsWithDetails();
        IEnumerable<Booking> GetBookingsByUser(string userId);
        IEnumerable<Booking> GetBookingsByStatus(string status);
        Booking GetBookingWithDetails(int id);
    }
}
