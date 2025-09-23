using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using star_events.Services;
using Stripe;

namespace star_events.Controllers
{
    [Authorize]
    public class StripePaymentController : Controller
    {
        private readonly IStripeService _stripeService;
        private readonly ILogger<StripePaymentController> _logger;

        public StripePaymentController(IStripeService stripeService, ILogger<StripePaymentController> logger)
        {
            _stripeService = stripeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
        {
            try
            {
                var metadata = new Dictionary<string, string>
                {
                    ["booking_details"] = request.BookingDetails ?? "",
                    ["event_id"] = request.EventId.ToString(),
                    ["user_id"] = User.Identity?.Name ?? ""
                };

                var paymentIntent = await _stripeService.CreatePaymentIntentAsync(
                    request.Amount,
                    request.Currency,
                    null, // customer ID if you have one
                    metadata
                );

                return Json(new
                {
                    success = true,
                    clientSecret = paymentIntent.ClientSecret,
                    paymentIntentId = paymentIntent.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment intent");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessCustomPayment([FromBody] CustomPaymentRequest request)
        {
            try
            {
                // Create payment method from custom form data
                var paymentMethodOptions = new PaymentMethodCreateOptions
                {
                    Type = "card",
                    Card = new PaymentMethodCardOptions
                    {
                        Number = request.CardNumber.Replace(" ", ""),
                        ExpMonth = long.Parse(request.ExpiryMonth),
                        ExpYear = long.Parse(request.ExpiryYear),
                        Cvc = request.Cvc
                    },
                    BillingDetails = new PaymentMethodBillingDetailsOptions
                    {
                        Name = request.CardHolderName
                    }
                };

                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = await paymentMethodService.CreateAsync(paymentMethodOptions);

                // Create and confirm payment intent
                var paymentIntentOptions = new PaymentIntentCreateOptions
                {
                    Amount = (long)(request.Amount * 100), // Convert to cents
                    Currency = "lkr",
                    PaymentMethod = paymentMethod.Id,
                    Confirm = true,
                    Description = $"Event booking - {request.EventId}",
                    Metadata = new Dictionary<string, string>
                    {
                        ["event_id"] = request.EventId.ToString(),
                        ["user_id"] = User.Identity?.Name ?? "",
                        ["booking_details"] = request.BookingDetails ?? ""
                    }
                };

                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.CreateAsync(paymentIntentOptions);

                if (paymentIntent.Status == "succeeded")
                {
                    // Payment successful - create booking, tickets, etc.
                    var bookingResult = await CreateBookingFromPayment(paymentIntent, request);

                    return Json(new
                    {
                        success = true,
                        message = "Payment successful!",
                        paymentIntentId = paymentIntent.Id,
                        bookingId = bookingResult.BookingId,
                        qrCodes = bookingResult.QrCodes
                    });
                }
                else if (paymentIntent.Status == "requires_action")
                {
                    // 3D Secure authentication required
                    return Json(new
                    {
                        success = false,
                        requiresAction = true,
                        clientSecret = paymentIntent.ClientSecret,
                        message = "Additional authentication required"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Payment failed"
                    });
                }
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe error during payment processing");
                return Json(new
                {
                    success = false,
                    message = GetUserFriendlyStripeError(ex)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return Json(new
                {
                    success = false,
                    message = "An error occurred while processing your payment"
                });
            }
        }

        private async Task<BookingResult> CreateBookingFromPayment(PaymentIntent paymentIntent, CustomPaymentRequest request)
        {
            // Implement your booking creation logic here
            // This should create the booking record, tickets, QR codes, etc.

            // Example implementation:
            var bookingId = Guid.NewGuid().ToString();
            var qrCodes = new List<string>();

            // Generate QR codes for each seat/ticket
            // var selectedSeats = JsonSerializer.Deserialize<List<SelectedSeat>>(request.BookingDetails);
            // foreach (var seat in selectedSeats)
            // {
            //     var qrCode = $"TICKET:{bookingId}|SEAT:{seat.Id}|EVENT:{request.EventId}|STATUS:CONFIRMED";
            //     qrCodes.Add(qrCode);
            // }

            return new BookingResult
            {
                BookingId = bookingId,
                QrCodes = qrCodes
            };
        }

        private string GetUserFriendlyStripeError(StripeException ex)
        {
            return ex.StripeError?.Code switch
            {
                "card_declined" => "Your card was declined. Please try a different payment method.",
                "expired_card" => "Your card has expired. Please use a different card.",
                "incorrect_cvc" => "The security code you entered is incorrect.",
                "processing_error" => "An error occurred while processing your card. Please try again.",
                "incorrect_number" => "The card number you entered is incorrect.",
                _ => "There was an issue with your payment. Please check your card details and try again."
            };
        }
    }

    // Request models
    public class CreatePaymentIntentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "lkr";
        public string? BookingDetails { get; set; }
        public int EventId { get; set; }
    }

    public class CustomPaymentRequest
    {
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryMonth { get; set; } = string.Empty;
        public string ExpiryYear { get; set; } = string.Empty;
        public string Cvc { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int EventId { get; set; }
        public string? BookingDetails { get; set; }
    }

    public class BookingResult
    {
        public string BookingId { get; set; } = string.Empty;
        public List<string> QrCodes { get; set; } = new();
    }
}