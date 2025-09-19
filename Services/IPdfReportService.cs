using star_events.Models;

namespace star_events.Services
{
    public interface IPdfReportService
    {
        byte[] GenerateRevenueReportPdf(List<Payment> payments, decimal totalRevenue, 
            List<PaymentMethodSummary> paymentMethods, DateTime startDate, DateTime endDate, 
            string? eventTitle = null);
        
        byte[] GenerateEventsReportPdf(List<Event> events, int totalEvents, int activeEvents, 
            int completedEvents, int cancelledEvents, DateTime startDate, DateTime endDate);
        
        byte[] GenerateUsersReportPdf(List<ApplicationUser> users, int totalUsers, 
            int adminUsers, int eventOrganizerUsers, int customerUsers, 
            DateTime startDate, DateTime endDate, string? selectedRole = null);
        
        byte[] GenerateTicketSalesReportPdf(List<Ticket> tickets, int totalTickets, 
            decimal totalRevenue, int scannedTickets, int unscannedTickets, 
            DateTime startDate, DateTime endDate, string? eventTitle = null);
    }

    public class PaymentMethodSummary
    {
        public string Method { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
