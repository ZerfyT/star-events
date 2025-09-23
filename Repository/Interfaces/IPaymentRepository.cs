using star_events.Models;

namespace star_events.Repository.Interfaces;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    IEnumerable<Payment> GetPaymentsByBookingId(int bookingId);
    IEnumerable<Payment> GetPaymentsByUser(string userId);
    IEnumerable<Payment> GetPaymentsByStatus(string paymentStatus);
    IEnumerable<Payment> GetPaymentsByDateRange(DateTime startDate, DateTime endDate);
    Payment? GetPaymentByTransactionId(string transactionId);
    IEnumerable<Payment> GetSuccessfulPayments();
    IEnumerable<Payment> GetFailedPayments();
}
