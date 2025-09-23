namespace star_events.Models.ViewModels
{
    public class PaymentIndexViewModel
    {
        public bool IsAdmin { get; set; }
        public bool IsEventOrganizer { get; set; }
        public List<Payment> Payments { get; set; } = new();
        public int TotalPayments { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingPayments { get; set; }
        public int FailedPayments { get; set; }
    }
}
