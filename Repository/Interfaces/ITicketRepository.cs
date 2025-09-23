using star_events.Models;

namespace star_events.Repository.Interfaces;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    IEnumerable<Ticket> GetTicketsByBookingId(int bookingId);
    IEnumerable<Ticket> GetTicketsByEventId(int eventId);
    IEnumerable<Ticket> GetScannedTickets();
    IEnumerable<Ticket> GetUnscannedTickets();
    Ticket? GetTicketByQRCode(string qrCodeValue);
    IEnumerable<Ticket> GetTicketsByUser(string userId);
}
