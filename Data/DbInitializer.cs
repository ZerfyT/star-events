using Microsoft.AspNetCore.Identity;
using star_events.Models;

namespace star_events.Data;

public class DbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public DbInitializer(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
        _context.Database.EnsureCreated();

        SeedRoles();
        SeedUsers();

        SeedEventCategories();
        SeedVenues();
        SeedPromotions();
        SeedEvents();
        SeedBookings();
        SeedPayments();
        SeedTickets();
    }

    private void SeedRoles()
    {
        // Check if the roles already exist
        if (_roleManager.Roles.Any()) return;

        string[] roleNames = { "Admin", "EventOrganizer", "Customer" };
        foreach (var roleName in roleNames)
            if (!_roleManager.RoleExistsAsync(roleName).Result)
                _roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
    }

    private void SeedUsers()
    {
        // Check if the users already exist
        if (_userManager.Users.Any()) return;

        // Create Admin User
        var adminUser = new ApplicationUser
        {
            UserName = "admin@starevents.lk",
            Email = "admin@starevents.lk",
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true
        };

        var result = _userManager.CreateAsync(adminUser, "Admin@123").Result;

        if (result.Succeeded)
            // Assign the new user to the "Admin" role
            _userManager.AddToRoleAsync(adminUser, "Admin").Wait();

        // Create Event Organizer User
        var organizerUser = new ApplicationUser
        {
            UserName = "organizer@starevents.lk",
            Email = "organizer@starevents.lk",
            FirstName = "Event",
            LastName = "Organizer",
            EmailConfirmed = true
        };

        var orgResult = _userManager.CreateAsync(organizerUser, "Organizer@123").Result;
        if (orgResult.Succeeded)
            // Assign the new user to the "EventOrganizer" role
            _userManager.AddToRoleAsync(organizerUser, "EventOrganizer").Wait();
    }

    private void SeedEventCategories()
    {
        if (_context.Categories.Any()) return;

        var categories = new[]
        {
            new Category { Name = "Live Concert", Description = "Live musical performances by artists and bands." },
            new Category
                { Name = "Theatre & Drama", Description = "Stage plays, dramas, and theatrical performances." },
            new Category
                { Name = "Cultural Festival", Description = "Events celebrating culture, heritage, and traditions." },
            new Category { Name = "Stand-up Comedy", Description = "Live comedy shows featuring stand-up comedians." },
            new Category
                { Name = "Workshop & Seminar", Description = "Educational workshops and professional seminars." }
        };

        _context.Categories.AddRange(categories);
        _context.SaveChanges();
    }

    private void SeedVenues()
    {
        if (_context.Locations.Any()) return;

        var venues = new[]
        {
            new Location
            {
                Name = "Nelum Pokuna Theatre", Address = "110 Ananda Coomaraswamy Mawatha", City = "Colombo",
                Capacity = 1288
            },
            new Location
                { Name = "BMICH Main Hall", Address = "Bauddhaloka Mawatha", City = "Colombo", Capacity = 1600 },
            new Location
            {
                Name = "Galle Face Green", Address = "Galle Road, Colombo", City = "Colombo", Capacity = 25000
            },
            new Location
            {
                Name = "Lionel Wendt Art Centre", Address = "18 Guildford Crescent", City = "Colombo", Capacity = 622
            }
        };

        _context.Locations.AddRange(venues);
        _context.SaveChanges();
    }

    private void SeedPromotions()
    {
        if (_context.Promotions.Any()) return;

        var promotions = new[]
        {
            new Promotion
            {
                PromoCode = "EARLYBIRD15",
                DiscountType = "Percentage",
                DiscountValue = 15,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                IsActive = true
            },
            new Promotion
            {
                PromoCode = "SAVE500",
                DiscountType = "FixedAmount",
                DiscountValue = 500,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(2),
                IsActive = true
            }
        };

        _context.Promotions.AddRange(promotions);
        _context.SaveChanges();
    }

    private void SeedEvents()
    {
        if (_context.Events.Any()) return;

        // Get dependencies
        var organizer = _userManager.FindByEmailAsync("organizer@starevents.lk").Result;
        var concertCategory = _context.Categories.FirstOrDefault(c => c.Name == "Live Concert");
        var theatreCategory = _context.Categories.FirstOrDefault(c => c.Name == "Theatre & Drama");
        var nelumPokuna = _context.Locations.FirstOrDefault(v => v.Name == "Nelum Pokuna Theatre");
        var lionelWendt = _context.Locations.FirstOrDefault(v => v.Name == "Lionel Wendt Art Centre");

        // Ensure dependencies exist before proceeding
        if (organizer == null || concertCategory == null || theatreCategory == null || nelumPokuna == null ||
            lionelWendt == null) return; // or throw an exception

        var events = new[]
        {
            new Event
            {
                Title = "Symphony of Strings - Live in Colombo",
                Description =
                    "A magical evening featuring the nation's top classical and contemporary artists. Experience a fusion of sounds that will captivate your soul.",
                StartDateTime = DateTime.Now.AddDays(45).Date.AddHours(19), // 7 PM
                EndDateTime = DateTime.Now.AddDays(45).Date.AddHours(22), // 10 PM
                OrganizerID = organizer.Id,
                LocationID = nelumPokuna.LocationID,
                CategoryID = concertCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/334155/FFFFFF?text=Symphony+of+Strings",
                Status = "Upcoming"
            },
            new Event
            {
                Title = "Colombo International Theatre Festival",
                Description =
                    "Showcasing the best of local and international drama. A week-long festival of captivating performances, workshops, and artist talks.",
                StartDateTime = DateTime.Now.AddDays(70).Date.AddHours(18), // 6 PM
                EndDateTime = DateTime.Now.AddDays(70).Date.AddHours(21), // 9 PM
                OrganizerID = organizer.Id,
                LocationID = lionelWendt.LocationID,
                CategoryID = theatreCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/78350F/FFFFFF?text=Theatre+Festival",
                Status = "Upcoming"
            }
        };

        _context.Events.AddRange(events);
        _context.SaveChanges();

        // Seed Ticket Types for the created events
        var symphonyEvent = _context.Events.FirstOrDefault(e => e.Title.Contains("Symphony"));
        if (symphonyEvent != null)
        {
            var symphonyTickets = new[]
            {
                new TicketType
                {
                    EventID = symphonyEvent.EventID, Name = "VIP Box", Price = 20, TotalQuantity = 100,
                    AvailableQuantity = 100
                },
                new TicketType
                {
                    EventID = symphonyEvent.EventID, Name = "Balcony", Price = 13, TotalQuantity = 400,
                    AvailableQuantity = 400
                },
                new TicketType
                {
                    EventID = symphonyEvent.EventID, Name = "Orchestra Stalls", Price = (decimal)8.99, TotalQuantity = 788,
                    AvailableQuantity = 788
                }
            };
            _context.TicketTypes.AddRange(symphonyTickets);
        }

        var theatreEvent = _context.Events.FirstOrDefault(e => e.Title.Contains("Theatre Festival"));
        if (theatreEvent != null)
        {
            var theatreTickets = new[]
            {
                new TicketType
                {
                    EventID = theatreEvent.EventID, Name = "Premium Seating", Price = 11, TotalQuantity = 200,
                    AvailableQuantity = 200
                },
                new TicketType
                {
                    EventID = theatreEvent.EventID, Name = "General Admission", Price = 4, TotalQuantity = 422,
                    AvailableQuantity = 422
                }
            };
            _context.TicketTypes.AddRange(theatreTickets);
        }

        _context.SaveChanges();
    }

    private void SeedBookings()
    {
        if (_context.Bookings.Any()) return;

        // Get dependencies
        var adminUser = _userManager.FindByEmailAsync("admin@starevents.lk").Result;
        var organizerUser = _userManager.FindByEmailAsync("organizer@starevents.lk").Result;
        var earlyBirdPromo = _context.Promotions.FirstOrDefault(p => p.PromoCode == "EARLYBIRD15");
        var save500Promo = _context.Promotions.FirstOrDefault(p => p.PromoCode == "SAVE500");

        // Create a sample customer user for bookings
        var customerUser = new ApplicationUser
        {
            UserName = "customer02@starevents.lk",
            Email = "customer02@starevents.lk",
            FirstName = "John",
            LastName = "Doe",
            EmailConfirmed = true
        };

        var customerResult = _userManager.CreateAsync(customerUser, "Customer@123").Result;
        if (customerResult.Succeeded)
            _userManager.AddToRoleAsync(customerUser, "Customer").Wait();

        // Ensure dependencies exist
        if (adminUser == null || organizerUser == null || customerUser == null) return;

        var bookings = new[]
        {
            new Booking
            {
                UserID = customerUser.Id,
                PromotionID = earlyBirdPromo?.PromotionID,
                BookingDateTime = DateTime.Now.AddDays(-5),
                TotalAmount = 17, // VIP Box (20) with 15% discount = 17
                Status = "Confirmed"
            },
            new Booking
            {
                UserID = customerUser.Id,
                PromotionID = save500Promo?.PromotionID,
                BookingDateTime = DateTime.Now.AddDays(-3),
                TotalAmount = 6, // Premium Seating (11) with 4 discount = 7, but using 6 for realistic amount
                Status = "Confirmed"
            },
            new Booking
            {
                UserID = adminUser.Id,
                PromotionID = null,
                BookingDateTime = DateTime.Now.AddDays(-1),
                TotalAmount = 13, // Balcony ticket
                Status = "Pending"
            }
        };

        _context.Bookings.AddRange(bookings);
        _context.SaveChanges();
    }

    private void SeedPayments()
    {
        if (_context.Payments.Any()) return;

        // Get the bookings we just created
        var bookings = _context.Bookings.ToList();
        if (!bookings.Any()) return;

        var payments = new[]
        {
            new Payment
            {
                BookingID = bookings[0].BookingID,
                PaymentGatewayTransactionID = "TXN_" + Guid.NewGuid().ToString("N")[..8].ToUpper(),
                AmountPaid = 17,
                PaymentMethod = "Credit Card",
                PaymentStatus = "Completed",
                PaymentDateTime = bookings[0].BookingDateTime.AddMinutes(5)
            },
            new Payment
            {
                BookingID = bookings[1].BookingID,
                PaymentGatewayTransactionID = "TXN_" + Guid.NewGuid().ToString("N")[..8].ToUpper(),
                AmountPaid = 6,
                PaymentMethod = "Debit Card",
                PaymentStatus = "Completed",
                PaymentDateTime = bookings[1].BookingDateTime.AddMinutes(3)
            },
            new Payment
            {
                BookingID = bookings[2].BookingID,
                PaymentGatewayTransactionID = "TXN_" + Guid.NewGuid().ToString("N")[..8].ToUpper(),
                AmountPaid = 13,
                PaymentMethod = "Bank Transfer",
                PaymentStatus = "Pending",
                PaymentDateTime = bookings[2].BookingDateTime.AddMinutes(10)
            }
        };

        _context.Payments.AddRange(payments);
        _context.SaveChanges();
    }

    private void SeedTickets()
    {
        if (_context.Tickets.Any()) return;

        // Get the bookings and ticket types
        var bookings = _context.Bookings.ToList();
        var ticketTypes = _context.TicketTypes.ToList();
        
        if (!bookings.Any() || !ticketTypes.Any()) return;

        var tickets = new List<Ticket>();

        // Create tickets for each booking
        foreach (var booking in bookings)
        {
            // Find appropriate ticket type based on booking amount
            TicketType? selectedTicketType = null;
            
            if (booking.TotalAmount >= 15) // VIP Box (20)
            {
                selectedTicketType = ticketTypes.FirstOrDefault(tt => tt.Name == "VIP Box");
            }
            else if (booking.TotalAmount >= 10) // Premium or Balcony
            {
                selectedTicketType = ticketTypes.FirstOrDefault(tt => tt.Name == "Premium Seating") ??
                                   ticketTypes.FirstOrDefault(tt => tt.Name == "Balcony");
            }
            else // General or Orchestra
            {
                selectedTicketType = ticketTypes.FirstOrDefault(tt => tt.Name == "General Admission") ??
                                   ticketTypes.FirstOrDefault(tt => tt.Name == "Orchestra Stalls");
            }

            if (selectedTicketType != null)
            {
                // Create 1-3 tickets per booking
                var ticketCount = booking.TotalAmount >= 15 ? 1 : 
                                 booking.TotalAmount >= 10 ? 2 : 3;

                for (int i = 0; i < ticketCount; i++)
                {
                    var ticket = new Ticket
                    {
                        BookingID = booking.BookingID,
                        TicketTypeID = selectedTicketType.TicketTypeID,
                        QRCodeValue = "QR_" + Guid.NewGuid().ToString("N")[..12].ToUpper(),
                        IsScanned = booking.Status == "Confirmed" && i == 0, // First ticket scanned for confirmed bookings
                        ScannedAt = booking.Status == "Confirmed" && i == 0 ? 
                                   booking.BookingDateTime.AddDays(1) : null
                    };
                    tickets.Add(ticket);
                }
            }
        }

        _context.Tickets.AddRange(tickets);
        _context.SaveChanges();
    }
}