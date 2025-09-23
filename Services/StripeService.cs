using Microsoft.Extensions.Options;
using star_events.Models;
using star_events.Models.Configuration;
using Stripe;

namespace star_events.Services
{
    public class StripeService : IStripeService
    {
        private readonly StripeSettings _stripeSettings;
        private readonly ILogger<StripeService> _logger;

        public StripeService(IOptions<StripeSettings> stripeSettings, ILogger<StripeService> logger)
        {
            _stripeSettings = stripeSettings.Value;
            _logger = logger;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public async Task<PaymentIntent> CreatePaymentIntentAsync(decimal amount, string currency = "lkr", string? customerId = null, Dictionary<string, string>? metadata = null)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100), // Convert to cents
                    Currency = currency.ToLower(),
                    Customer = customerId,
                    Metadata = metadata ?? new Dictionary<string, string>(),
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    }
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                _logger.LogInformation("Payment intent created: {PaymentIntentId} for amount {Amount}", paymentIntent.Id, amount);
                return paymentIntent;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Error creating payment intent for amount {Amount}", amount);
                throw new Exception($"Failed to create payment intent: {ex.Message}", ex);
            }
        }

        public async Task<PaymentIntent> ConfirmPaymentIntentAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                var options = new PaymentIntentConfirmOptions();
                var paymentIntent = await service.ConfirmAsync(paymentIntentId, options);

                _logger.LogInformation("Payment intent confirmed: {PaymentIntentId}", paymentIntentId);
                return paymentIntent;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Error confirming payment intent {PaymentIntentId}", paymentIntentId);
                throw new Exception($"Failed to confirm payment intent: {ex.Message}", ex);
            }
        }

        public async Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId);
                return paymentIntent;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Error getting payment intent {PaymentIntentId}", paymentIntentId);
                throw new Exception($"Failed to get payment intent: {ex.Message}", ex);
            }
        }

        public async Task<Customer> CreateCustomerAsync(ApplicationUser user)
        {
            try
            {
                var options = new CustomerCreateOptions
                {
                    Email = user.Email,
                    Name = $"{user.FirstName} {user.LastName}",
                    Phone = user.PhoneNumber,
                    Metadata = new Dictionary<string, string>
                    {
                        ["user_id"] = user.Id,
                        ["loyalty_points"] = user.LoyaltyPoints.ToString()
                    }
                };

                var service = new CustomerService();
                var customer = await service.CreateAsync(options);

                _logger.LogInformation("Stripe customer created: {CustomerId} for user {UserId}", customer.Id, user.Id);
                return customer;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Error creating customer for user {UserId}", user.Id);
                throw new Exception($"Failed to create customer: {ex.Message}", ex);
            }
        }

        public async Task<Customer?> GetCustomerAsync(string customerId)
        {
            try
            {
                var service = new CustomerService();
                var customer = await service.GetAsync(customerId);
                return customer;
            }
            catch (StripeException ex) when (ex.StripeError?.Type == "invalid_request_error")
            {
                _logger.LogWarning("Customer not found: {CustomerId}", customerId);
                return null;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Error getting customer {CustomerId}", customerId);
                throw new Exception($"Failed to get customer: {ex.Message}", ex);
            }
        }

        public async Task<Refund> CreateRefundAsync(string paymentIntentId, decimal? amount = null)
        {
            try
            {
                var options = new RefundCreateOptions
                {
                    PaymentIntent = paymentIntentId,
                };

                if (amount.HasValue)
                {
                    options.Amount = (long)(amount.Value * 100);
                }

                var service = new RefundService();
                var refund = await service.CreateAsync(options);

                _logger.LogInformation("Refund created: {RefundId} for payment intent {PaymentIntentId}", refund.Id, paymentIntentId);
                return refund;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Error creating refund for payment intent {PaymentIntentId}", paymentIntentId);
                throw new Exception($"Failed to create refund: {ex.Message}", ex);
            }
        }
    }
}