using Microsoft.EntityFrameworkCore;
using star_events.Data;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Repository.Services;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override IEnumerable<Booking> GetAll()
    {
        return _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Promotion)
            .Include(b => b.Tickets)
            .ThenInclude(t => t.TicketType)
            .ThenInclude(tt => tt.Event)
            .Include(b => b.Payments)
            .ToList();
    }

    public override Booking GetById(object id)
    {
        return _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Promotion)
            .Include(b => b.Tickets)
            .ThenInclude(t => t.TicketType)
            .ThenInclude(tt => tt.Event)
            .Include(b => b.Payments)
            .FirstOrDefault(b => b.BookingID == (int)id);
    }

    public IEnumerable<Booking> GetBookingsWithDetails()
    {
        return _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Promotion)
            .Include(b => b.Tickets)
            .ThenInclude(t => t.TicketType)
            .ThenInclude(tt => tt.Event)
            .Include(b => b.Payments)
            .ToList();
    }

    public IEnumerable<Booking> GetBookingsByUser(string userId)
    {
        return _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Promotion)
            .Include(b => b.Tickets)
            .ThenInclude(t => t.TicketType)
            .ThenInclude(tt => tt.Event)
            .Include(b => b.Payments)
            .Where(b => b.UserID == userId)
            .ToList();
    }

    public IEnumerable<Booking> GetBookingsByStatus(string status)
    {
        return _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Promotion)
            .Include(b => b.Tickets)
            .ThenInclude(t => t.TicketType)
            .ThenInclude(tt => tt.Event)
            .Include(b => b.Payments)
            .Where(b => b.Status == status)
            .ToList();
    }

    public Booking GetBookingWithDetails(int id)
    {
        return _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Promotion)
            .Include(b => b.Tickets)
            .ThenInclude(t => t.TicketType)
            .ThenInclude(tt => tt.Event)
            .Include(b => b.Payments)
            .FirstOrDefault(b => b.BookingID == id);
    }
}