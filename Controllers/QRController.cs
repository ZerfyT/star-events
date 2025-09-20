using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using star_events.Data;
using star_events.Models;
using star_events.Repository.Interfaces;

namespace star_events.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRController : ControllerBase
    {
        private readonly ILogger<QRController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IBookingRepository _bookingRepository;

        public QRController(ILogger<QRController> logger, ApplicationDbContext context, IBookingRepository bookingRepository)
        {
            _logger = logger;
            _context = context;
            _bookingRepository = bookingRepository;
        }

        // GET: api/qr/ticket/{ticketId}
        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetTicketDetails(int ticketId)
        {
            try
            {
                var ticket = await _context.Tickets
                    .Include(t => t.Booking)
                        .ThenInclude(b => b.Event)
                            .ThenInclude(e => e.Location)
                    .Include(t => t.Booking)
                        .ThenInclude(b => b.Event)
                            .ThenInclude(e => e.Category)
                    .Include(t => t.Booking)
                        .ThenInclude(b => b.User)
                    .Include(t => t.TicketType)
                    .Include(t => t.Booking)
                        .ThenInclude(b => b.Payments)
                    .FirstOrDefaultAsync(t => t.TicketID == ticketId);

                if (ticket == null)
                {
                    return NotFound(new { success = false, message = "Ticket not found" });
                }

                var ticketDetails = new
                {
                    success = true,
                    ticket = new
                    {
                        ticketId = ticket.TicketID,
                        qrCodeValue = ticket.QRCodeValue,
                        isScanned = ticket.IsScanned,
                        scannedAt = ticket.ScannedAt,
                        booking = new
                        {
                            bookingId = ticket.Booking.BookingID,
                            bookingDateTime = ticket.Booking.BookingDateTime,
                            totalAmount = ticket.Booking.TotalAmount,
                            status = ticket.Booking.Status,
                            user = new
                            {
                                userId = ticket.Booking.User.Id,
                                firstName = ticket.Booking.User.FirstName,
                                lastName = ticket.Booking.User.LastName,
                                email = ticket.Booking.User.Email
                            },
                            @event = new
                            {
                                eventId = ticket.Booking.Event.EventID,
                                title = ticket.Booking.Event.Title,
                                imagePath = ticket.Booking.Event.AllImagePaths.FirstOrDefault(),
                                description = ticket.Booking.Event.Description,
                                startDateTime = ticket.Booking.Event.StartDateTime,
                                endDateTime = ticket.Booking.Event.EndDateTime,
                                location = new
                                {
                                    name = ticket.Booking.Event.Location?.Name,
                                    address = ticket.Booking.Event.Location?.Address
                                },
                                category = new
                                {
                                    name = ticket.Booking.Event.Category?.Name
                                }
                            },
                            payments = ticket.Booking.Payments.Select(p => new
                            {
                                paymentId = p.PaymentID,
                                amount = p.AmountPaid,
                                paymentMethod = p.PaymentMethod,
                                paymentDate = p.PaymentDateTime,
                                status = p.PaymentStatus
                            }).ToList()
                        },
                        ticketType = new
                        {
                            ticketTypeId = ticket.TicketType.TicketTypeID,
                            name = ticket.TicketType.Name,
                            price = ticket.TicketType.Price,
                            totalQuantity = ticket.TicketType.TotalQuantity,
                            availableQuantity = ticket.TicketType.AvailableQuantity
                        }
                    }
                };

                return Ok(ticketDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ticket details for ID: {TicketId}", ticketId);
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        // POST: api/qr/scan/{ticketId}
        [HttpPost("scan/{ticketId}")]
        public async Task<IActionResult> ScanTicket(int ticketId)
        {
            try
            {
                var ticket = await _context.Tickets
                    .Include(t => t.Booking)
                        .ThenInclude(b => b.Event)
                    .Include(t => t.TicketType)
                    .FirstOrDefaultAsync(t => t.TicketID == ticketId);

                if (ticket == null)
                {
                    return NotFound(new { success = false, message = "Ticket not found" });
                }

                if (ticket.IsScanned)
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "Ticket has already been scanned",
                        scannedAt = ticket.ScannedAt
                    });
                }

                // Check if event has already started or ended
                var now = DateTime.Now;
                if (now > ticket.Booking.Event.EndDateTime)
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "Event has already ended" 
                    });
                }

                // Mark ticket as scanned
                ticket.IsScanned = true;
                ticket.ScannedAt = now;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Ticket {TicketId} scanned successfully at {ScanTime}", ticketId, now);

                return Ok(new { 
                    success = true, 
                    message = "Ticket scanned successfully",
                    scannedAt = now,
                    eventTitle = ticket.Booking.Event.Title,
                    ticketType = ticket.TicketType?.Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scanning ticket ID: {TicketId}", ticketId);
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        // GET: api/qr/validate/{qrCodeValue}
        [HttpGet("validate/{qrCodeValue}")]
        public async Task<IActionResult> ValidateQRCode(string qrCodeValue)
        {
            try
            {
                var ticket = await _context.Tickets
                    .Include(t => t.Booking)
                        .ThenInclude(b => b.Event)
                    .Include(t => t.TicketType)
                    .FirstOrDefaultAsync(t => t.QRCodeValue == qrCodeValue);

                if (ticket == null)
                {
                    return NotFound(new { success = false, message = "Invalid QR code" });
                }

                var validationResult = new
                {
                    success = true,
                    isValid = true,
                    ticket = new
                    {
                        ticketId = ticket.TicketID,
                        isScanned = ticket.IsScanned,
                        scannedAt = ticket.ScannedAt,
                        eventTitle = ticket.Booking.Event.Title,
                        ticketType = ticket.TicketType?.Name,
                        eventStartTime = ticket.Booking.Event.StartDateTime,
                        eventEndTime = ticket.Booking.Event.EndDateTime
                    }
                };

                return Ok(validationResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating QR code: {QRCodeValue}", qrCodeValue);
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
    }
}