using Microsoft.EntityFrameworkCore;
using star_events.Data;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Repository.Services;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override IEnumerable<Payment> GetAll()
    {
        return _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.User)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
            .ToList();
    }

    public override Payment GetById(object id)
    {
        return _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.User)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
                    .ThenInclude(e => e.Location)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
                    .ThenInclude(e => e.Category)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Promotion)
            .FirstOrDefault(p => p.PaymentID == (int)id);
    }

    public IEnumerable<Payment> GetPaymentsByBookingId(int bookingId)
    {
        return _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.User)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
            .Where(p => p.BookingID == bookingId)
            .ToList();
    }

    public IEnumerable<Payment> GetPaymentsByUser(string userId)
    {
        return _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.User)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
            .Where(p => p.Booking.UserID == userId)
            .ToList();
    }

    public IEnumerable<Payment> GetPaymentsByStatus(string paymentStatus)
    {
        return _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.User)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
            .Where(p => p.PaymentStatus == paymentStatus)
            .ToList();
    }

    public IEnumerable<Payment> GetPaymentsByDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.User)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
            .Where(p => p.PaymentDateTime >= startDate && p.PaymentDateTime <= endDate)
            .ToList();
    }

    public Payment? GetPaymentByTransactionId(string transactionId)
    {
        return _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.User)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
                    .ThenInclude(e => e.Location)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
                    .ThenInclude(e => e.Category)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Promotion)
            .FirstOrDefault(p => p.PaymentGatewayTransactionID == transactionId);
    }

    public IEnumerable<Payment> GetSuccessfulPayments()
    {
        return _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.User)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
            .Where(p => p.PaymentStatus == "Completed" || p.PaymentStatus == "Success")
            .ToList();
    }

    public IEnumerable<Payment> GetFailedPayments()
    {
        return _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.User)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Event)
            .Where(p => p.PaymentStatus == "Failed" || p.PaymentStatus == "Cancelled")
            .ToList();
    }
}
