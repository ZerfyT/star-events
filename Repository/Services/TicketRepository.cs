using Microsoft.EntityFrameworkCore;
using star_events.Data;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Repository.Services;

public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    public TicketRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override IEnumerable<Ticket> GetAll()
    {
        return _context.Tickets
            .Include(t => t.Booking)
                .ThenInclude(b => b.User)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Event)
            .Include(t => t.TicketType)
            .ToList();
    }

    public override Ticket GetById(object id)
    {
        return _context.Tickets
            .Include(t => t.Booking)
                .ThenInclude(b => b.User)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Event)
                    .ThenInclude(e => e.Location)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Event)
                    .ThenInclude(e => e.Category)
            .Include(t => t.TicketType)
            .FirstOrDefault(t => t.TicketID == (int)id);
    }

    public IEnumerable<Ticket> GetTicketsByBookingId(int bookingId)
    {
        return _context.Tickets
            .Include(t => t.Booking)
                .ThenInclude(b => b.User)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Event)
            .Include(t => t.TicketType)
            .Where(t => t.BookingID == bookingId)
            .ToList();
    }

    public IEnumerable<Ticket> GetTicketsByEventId(int eventId)
    {
        return _context.Tickets
            .Include(t => t.Booking)
                .ThenInclude(b => b.User)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Event)
            .Include(t => t.TicketType)
            .Where(t => t.Booking.EventID == eventId)
            .ToList();
    }

    public IEnumerable<Ticket> GetScannedTickets()
    {
        return _context.Tickets
            .Include(t => t.Booking)
                .ThenInclude(b => b.User)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Event)
            .Include(t => t.TicketType)
            .Where(t => t.IsScanned == true)
            .ToList();
    }

    public IEnumerable<Ticket> GetUnscannedTickets()
    {
        return _context.Tickets
            .Include(t => t.Booking)
                .ThenInclude(b => b.User)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Event)
            .Include(t => t.TicketType)
            .Where(t => t.IsScanned == false)
            .ToList();
    }

    public Ticket? GetTicketByQRCode(string qrCodeValue)
    {
        return _context.Tickets
            .Include(t => t.Booking)
                .ThenInclude(b => b.User)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Event)
                    .ThenInclude(e => e.Location)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Event)
                    .ThenInclude(e => e.Category)
            .Include(t => t.TicketType)
            .FirstOrDefault(t => t.QRCodeValue == qrCodeValue);
    }

    public IEnumerable<Ticket> GetTicketsByUser(string userId)
    {
        return _context.Tickets
            .Include(t => t.Booking)
                .ThenInclude(b => b.User)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Event)
            .Include(t => t.TicketType)
            .Where(t => t.Booking.UserID == userId)
            .ToList();
    }
}
