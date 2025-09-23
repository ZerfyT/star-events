using star_events.Models;

namespace star_events.Models.ViewModels
{
    public class TransactionHistoryViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StatusFilter { get; set; }
        public List<Payment> Payments { get; set; } = new();
        public bool IsAdmin { get; set; }
        public bool IsEventOrganizer { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalAmount { get; set; }
        public int CompletedTransactions { get; set; }
        public int PendingTransactions { get; set; }
        public int FailedTransactions { get; set; }
    }
}
