using star_events.Models;
using Stripe;

namespace star_events.Services
{
    public interface IStripeService
    {
        Task<PaymentIntent> CreatePaymentIntentAsync(decimal amount, string currency = "lkr", string? customerId = null, Dictionary<string, string>? metadata = null);
        Task<PaymentIntent> ConfirmPaymentIntentAsync(string paymentIntentId);
        Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId);
        Task<Customer> CreateCustomerAsync(ApplicationUser user);
        Task<Customer?> GetCustomerAsync(string customerId);
        Task<Refund> CreateRefundAsync(string paymentIntentId, decimal? amount = null);
    }
}

