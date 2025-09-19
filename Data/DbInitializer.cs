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
        if (_userManager.Users.Any()) return;

        // Create Admin User
        var adminUser = new ApplicationUser
        {
            UserName = "admin@starevents.lk",
            Email = "admin@starevents.lk",
            FirstName = "Admin",
            LastName = "User",
            ContactNo = "+94 11 234 5678",
            EmailConfirmed = true
        };

        var result = _userManager.CreateAsync(adminUser, "Admin@123").Result;

        if (result.Succeeded)
            // Assign the new user to the "Admin" role
            _userManager.AddToRoleAsync(adminUser, "Admin").Wait();

        // Create Event Organizer Users
        var organizerUsers = new[]
        {
            new ApplicationUser
            {
                UserName = "organizer@starevents.lk",
                Email = "organizer@starevents.lk",
                FirstName = "Event",
                LastName = "Organizer",
                ContactNo = "+94 11 234 5679",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                UserName = "organizer2@starevents.lk",
                Email = "organizer2@starevents.lk",
                FirstName = "Sarah",
                LastName = "Johnson",
                ContactNo = "+94 11 234 5680",
                EmailConfirmed = true
            }
        };

        foreach (var organizerUser in organizerUsers)
        {
            var orgResult = _userManager.CreateAsync(organizerUser, "Organizer@123").Result;
            if (orgResult.Succeeded)
                _userManager.AddToRoleAsync(organizerUser, "EventOrganizer").Wait();
        }

        // Create Customer Users
        var customerUsers = new[]
        {
            new ApplicationUser
            {
                UserName = "customer@starevents.lk",
                Email = "customer@starevents.lk",
                FirstName = "John",
                LastName = "Doe",
                ContactNo = "+94 77 123 4567",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                UserName = "customer02@starevents.lk",
                Email = "customer02@starevents.lk",
                FirstName = "Jane",
                LastName = "Smith",
                ContactNo = "+94 77 123 4568",
                EmailConfirmed = true
            }
        };

        foreach (var customerUser in customerUsers)
        {
            var customerResult = _userManager.CreateAsync(customerUser, "Customer@123").Result;
            if (customerResult.Succeeded)
                _userManager.AddToRoleAsync(customerUser, "Customer").Wait();
        }
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

        var organizer = _context.Users.First(u => u.UserName == "organizer@starevents.lk");
        var concertCategory = _context.Categories.FirstOrDefault(c => c.Name == "Live Concert");
        var theatreCategory = _context.Categories.FirstOrDefault(c => c.Name == "Theatre & Drama");
        var culturalCategory = _context.Categories.FirstOrDefault(c => c.Name == "Cultural Festival");
        var comedyCategory = _context.Categories.FirstOrDefault(c => c.Name == "Stand-up Comedy");
        var workshopCategory = _context.Categories.FirstOrDefault(c => c.Name == "Workshop & Seminar");

        var nelumPokuna = _context.Locations.FirstOrDefault(v => v.Name == "Nelum Pokuna Theatre");
        var lionelWendt = _context.Locations.FirstOrDefault(v => v.Name == "Lionel Wendt Art Centre");
        var bmich = _context.Locations.FirstOrDefault(v => v.Name == "BMICH Main Hall");
        var galleFace = _context.Locations.FirstOrDefault(v => v.Name == "Galle Face Green");

        if (concertCategory == null || theatreCategory == null || culturalCategory == null ||
            comedyCategory == null || workshopCategory == null || nelumPokuna == null || lionelWendt == null ||
            bmich == null || galleFace == null) return;

        var events = new[]
        {
            // Past Events
            new Event
            {
                Title = "Symphony of Strings - Live in Colombo",
                Description =
                    "A magical evening featuring the nation's top classical and contemporary artists. Experience a fusion of sounds that will captivate your soul.",
                StartDateTime = DateTime.Now.AddDays(-30).Date.AddHours(19),
                EndDateTime = DateTime.Now.AddDays(-30).Date.AddHours(22),
                OrganizerID = organizer.Id,
                LocationID = nelumPokuna.LocationID,
                CategoryID = concertCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/334155/FFFFFF?text=Symphony+of+Strings",
                Status = "Completed"
            },
            new Event
            {
                Title = "Colombo International Theatre Festival",
                Description =
                    "Showcasing the best of local and international drama. A week-long festival of captivating performances, workshops, and artist talks.",
                StartDateTime = DateTime.Now.AddDays(-15).Date.AddHours(18),
                EndDateTime = DateTime.Now.AddDays(-15).Date.AddHours(21),
                OrganizerID = organizer.Id,
                LocationID = lionelWendt.LocationID,
                CategoryID = theatreCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/78350F/FFFFFF?text=Theatre+Festival",
                Status = "Completed"
            },
            new Event
            {
                Title = "Sri Lankan Cultural Heritage Festival",
                Description =
                    "Celebrate the rich cultural heritage of Sri Lanka with traditional dance, music, and art exhibitions.",
                StartDateTime = DateTime.Now.AddDays(-10).Date.AddHours(16),
                EndDateTime = DateTime.Now.AddDays(-10).Date.AddHours(20),
                OrganizerID = organizer.Id,
                LocationID = galleFace.LocationID,
                CategoryID = culturalCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/8B4513/FFFFFF?text=Cultural+Festival",
                Status = "Completed"
            },
            new Event
            {
                Title = "Comedy Night with Local Stars",
                Description =
                    "An evening of laughter with Sri Lanka's top comedians. Prepare for a night of non-stop entertainment.",
                StartDateTime = DateTime.Now.AddDays(-5).Date.AddHours(20),
                EndDateTime = DateTime.Now.AddDays(-5).Date.AddHours(23),
                OrganizerID = organizer.Id,
                LocationID = bmich.LocationID,
                CategoryID = comedyCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/FF6347/FFFFFF?text=Comedy+Night",
                Status = "Completed"
            },

            // Current/Active Events
            new Event
            {
                Title = "Digital Marketing Workshop",
                Description =
                    "Learn the latest digital marketing strategies and tools from industry experts. Perfect for entrepreneurs and marketing professionals.",
                StartDateTime = DateTime.Now.AddDays(2).Date.AddHours(9),
                EndDateTime = DateTime.Now.AddDays(2).Date.AddHours(17),
                OrganizerID = organizer.Id,
                LocationID = bmich.LocationID,
                CategoryID = workshopCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/4169E1/FFFFFF?text=Digital+Marketing",
                Status = "Active"
            },
            new Event
            {
                Title = "Jazz Under the Stars",
                Description =
                    "An intimate jazz performance under the beautiful Colombo sky. Featuring international and local jazz artists.",
                StartDateTime = DateTime.Now.AddDays(7).Date.AddHours(19),
                EndDateTime = DateTime.Now.AddDays(7).Date.AddHours(22),
                OrganizerID = organizer.Id,
                LocationID = nelumPokuna.LocationID,
                CategoryID = concertCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/191970/FFFFFF?text=Jazz+Under+Stars",
                Status = "Active"
            },

            // Upcoming Events
            new Event
            {
                Title = "Tech Innovation Summit 2024",
                Description =
                    "Join industry leaders and innovators for a comprehensive look at the future of technology in Sri Lanka.",
                StartDateTime = DateTime.Now.AddDays(15).Date.AddHours(8),
                EndDateTime = DateTime.Now.AddDays(15).Date.AddHours(18),
                OrganizerID = organizer.Id,
                LocationID = bmich.LocationID,
                CategoryID = workshopCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/32CD32/FFFFFF?text=Tech+Summit",
                Status = "Upcoming"
            },
            new Event
            {
                Title = "Shakespeare in the Park",
                Description =
                    "Experience the magic of Shakespeare's timeless plays in an outdoor setting. Perfect for families and literature lovers.",
                StartDateTime = DateTime.Now.AddDays(25).Date.AddHours(18),
                EndDateTime = DateTime.Now.AddDays(25).Date.AddHours(21),
                OrganizerID = organizer.Id,
                LocationID = galleFace.LocationID,
                CategoryID = theatreCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/228B22/FFFFFF?text=Shakespeare+Park",
                Status = "Upcoming"
            },
            new Event
            {
                Title = "Rock the City Concert",
                Description =
                    "The biggest rock concert of the year featuring international and local rock bands. Don't miss this epic musical experience.",
                StartDateTime = DateTime.Now.AddDays(35).Date.AddHours(19),
                EndDateTime = DateTime.Now.AddDays(35).Date.AddHours(23),
                OrganizerID = organizer.Id,
                LocationID = galleFace.LocationID,
                CategoryID = concertCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/DC143C/FFFFFF?text=Rock+Concert",
                Status = "Upcoming"
            },
            new Event
            {
                Title = "Stand-up Comedy Championship",
                Description =
                    "Witness the funniest comedians compete for the title of Sri Lanka's best stand-up comedian.",
                StartDateTime = DateTime.Now.AddDays(45).Date.AddHours(20),
                EndDateTime = DateTime.Now.AddDays(45).Date.AddHours(23),
                OrganizerID = organizer.Id,
                LocationID = lionelWendt.LocationID,
                CategoryID = comedyCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/FF8C00/FFFFFF?text=Comedy+Championship",
                Status = "Upcoming"
            },

            // Cancelled Event
            new Event
            {
                Title = "Cancelled Event Example",
                Description = "This event was cancelled due to unforeseen circumstances.",
                StartDateTime = DateTime.Now.AddDays(20).Date.AddHours(19),
                EndDateTime = DateTime.Now.AddDays(20).Date.AddHours(22),
                OrganizerID = organizer.Id,
                LocationID = nelumPokuna.LocationID,
                CategoryID = concertCategory.CategoryID,
                ImagePaths = "https://placehold.co/1024x768/696969/FFFFFF?text=Cancelled+Event",
                Status = "Cancelled"
            }
        };

        _context.Events.AddRange(events);
        _context.SaveChanges();

        // Seed Ticket Types for all events
        SeedTicketTypesForEvents();
    }

    private void SeedTicketTypesForEvents()
    {
        var events = _context.Events.ToList();

        foreach (var evt in events)
        {
            var ticketTypes = new List<TicketType>();

            // Create different ticket types based on event category
            if (evt.Category?.Name == "Live Concert")
                ticketTypes.AddRange(new[]
                {
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "VIP Box",
                        Price = 25,
                        TotalQuantity = 50,
                        AvailableQuantity = 50
                    },
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "Premium Seating",
                        Price = 18,
                        TotalQuantity = 200,
                        AvailableQuantity = 200
                    },
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "General Admission",
                        Price = 12,
                        TotalQuantity = 500,
                        AvailableQuantity = 500
                    }
                });
            else if (evt.Category?.Name == "Theatre & Drama")
                ticketTypes.AddRange(new[]
                {
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "Front Row",
                        Price = 15,
                        TotalQuantity = 100,
                        AvailableQuantity = 100
                    },
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "Standard Seating",
                        Price = 10,
                        TotalQuantity = 300,
                        AvailableQuantity = 300
                    },
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "Balcony",
                        Price = 7,
                        TotalQuantity = 200,
                        AvailableQuantity = 200
                    }
                });
            else if (evt.Category?.Name == "Cultural Festival")
                ticketTypes.AddRange(new[]
                {
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "Premium Pass",
                        Price = 20,
                        TotalQuantity = 100,
                        AvailableQuantity = 100
                    },
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "Standard Pass",
                        Price = 8,
                        TotalQuantity = 1000,
                        AvailableQuantity = 1000
                    }
                });
            else if (evt.Category?.Name == "Stand-up Comedy")
                ticketTypes.AddRange(new[]
                {
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "VIP Seating",
                        Price = 22,
                        TotalQuantity = 50,
                        AvailableQuantity = 50
                    },
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "Regular Seating",
                        Price = 15,
                        TotalQuantity = 400,
                        AvailableQuantity = 400
                    }
                });
            else if (evt.Category?.Name == "Workshop & Seminar")
                ticketTypes.AddRange(new[]
                {
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "Full Day Pass",
                        Price = 30,
                        TotalQuantity = 200,
                        AvailableQuantity = 200
                    },
                    new TicketType
                    {
                        EventID = evt.EventID,
                        Name = "Half Day Pass",
                        Price = 18,
                        TotalQuantity = 300,
                        AvailableQuantity = 300
                    }
                });

            _context.TicketTypes.AddRange(ticketTypes);
        }

        _context.SaveChanges();
    }

    private void SeedBookings()
    {
        if (_context.Bookings.Any()) return;

        var customer = _userManager.FindByEmailAsync("customer@starevents.lk").Result;
        var adminUser = _userManager.FindByEmailAsync("admin@starevents.lk").Result;
        var earlyBirdPromo = _context.Promotions.FirstOrDefault(p => p.PromoCode == "EARLYBIRD15");
        var save500Promo = _context.Promotions.FirstOrDefault(p => p.PromoCode == "SAVE500");
        var ticketTypes = _context.TicketTypes.ToList();

        if (customer == null || adminUser == null || !ticketTypes.Any()) return;

        var bookings = new List<Booking>();
        var random = new Random();

        // Create bookings for past events (completed events)
        var completedEvents = _context.Events.Where(e => e.Status == "Completed").ToList();

        foreach (var evt in completedEvents)
        {
            var eventTicketTypes = ticketTypes.Where(tt => tt.EventID == evt.EventID).ToList();
            if (!eventTicketTypes.Any()) continue;

            // Create 5-15 bookings per completed event
            var bookingCount = random.Next(5, 16);

            for (var i = 0; i < bookingCount; i++)
            {
                var ticketType = eventTicketTypes[random.Next(eventTicketTypes.Count)];
                var ticketQuantity = random.Next(1, 4); // 1-3 tickets per booking
                var baseAmount = ticketType.Price * ticketQuantity;

                // Apply promotions randomly
                Promotion? selectedPromo = null;
                var finalAmount = baseAmount;

                if (random.NextDouble() < 0.3) // 30% chance of using promotion
                {
                    selectedPromo = random.NextDouble() < 0.5 ? earlyBirdPromo : save500Promo;
                    if (selectedPromo != null)
                    {
                        if (selectedPromo.DiscountType == "Percentage")
                            finalAmount = baseAmount * (1 - selectedPromo.DiscountValue / 100);
                        else if (selectedPromo.DiscountType == "FixedAmount")
                            finalAmount = Math.Max(0, baseAmount - selectedPromo.DiscountValue);
                    }
                }

                var booking = new Booking
                {
                    UserID = customer.Id,
                    EventID = evt.EventID,
                    PromotionID = selectedPromo?.PromotionID,
                    BookingDateTime = evt.StartDateTime.AddDays(-random.Next(1, 30)), // Booked 1-30 days before event
                    TotalAmount = finalAmount,
                    Status = random.NextDouble() < 0.9 ? "Confirmed" : "Pending" // 90% confirmed, 10% pending
                };

                bookings.Add(booking);
            }
        }

        // Create bookings for active events
        var activeEvents = _context.Events.Where(e => e.Status == "Active").ToList();

        foreach (var evt in activeEvents)
        {
            var eventTicketTypes = ticketTypes.Where(tt => tt.EventID == evt.EventID).ToList();
            if (!eventTicketTypes.Any()) continue;

            // Create 3-10 bookings per active event
            var bookingCount = random.Next(3, 11);

            for (var i = 0; i < bookingCount; i++)
            {
                var ticketType = eventTicketTypes[random.Next(eventTicketTypes.Count)];
                var ticketQuantity = random.Next(1, 3); // 1-2 tickets per booking
                var baseAmount = ticketType.Price * ticketQuantity;

                // Apply promotions randomly
                Promotion? selectedPromo = null;
                var finalAmount = baseAmount;

                if (random.NextDouble() < 0.4) // 40% chance of using promotion for active events
                {
                    selectedPromo = random.NextDouble() < 0.5 ? earlyBirdPromo : save500Promo;
                    if (selectedPromo != null)
                    {
                        if (selectedPromo.DiscountType == "Percentage")
                            finalAmount = baseAmount * (1 - selectedPromo.DiscountValue / 100);
                        else if (selectedPromo.DiscountType == "FixedAmount")
                            finalAmount = Math.Max(0, baseAmount - selectedPromo.DiscountValue);
                    }
                }

                var booking = new Booking
                {
                    UserID = customer.Id,
                    EventID = evt.EventID,
                    PromotionID = selectedPromo?.PromotionID,
                    BookingDateTime = DateTime.Now.AddDays(-random.Next(1, 10)), // Booked recently
                    TotalAmount = finalAmount,
                    Status = random.NextDouble() < 0.8 ? "Confirmed" : "Pending" // 80% confirmed, 20% pending
                };

                bookings.Add(booking);
            }
        }

        // Create some bookings for upcoming events
        var upcomingEvents = _context.Events.Where(e => e.Status == "Upcoming").Take(3).ToList();

        foreach (var evt in upcomingEvents)
        {
            var eventTicketTypes = ticketTypes.Where(tt => tt.EventID == evt.EventID).ToList();
            if (!eventTicketTypes.Any()) continue;

            // Create 2-8 bookings per upcoming event
            var bookingCount = random.Next(2, 9);

            for (var i = 0; i < bookingCount; i++)
            {
                var ticketType = eventTicketTypes[random.Next(eventTicketTypes.Count)];
                var ticketQuantity = random.Next(1, 3); // 1-2 tickets per booking
                var baseAmount = ticketType.Price * ticketQuantity;

                // Apply promotions randomly
                Promotion? selectedPromo = null;
                var finalAmount = baseAmount;

                if (random.NextDouble() < 0.5) // 50% chance of using promotion for upcoming events
                {
                    selectedPromo = random.NextDouble() < 0.5 ? earlyBirdPromo : save500Promo;
                    if (selectedPromo != null)
                    {
                        if (selectedPromo.DiscountType == "Percentage")
                            finalAmount = baseAmount * (1 - selectedPromo.DiscountValue / 100);
                        else if (selectedPromo.DiscountType == "FixedAmount")
                            finalAmount = Math.Max(0, baseAmount - selectedPromo.DiscountValue);
                    }
                }

                var booking = new Booking
                {
                    UserID = customer.Id,
                    EventID = evt.EventID,
                    PromotionID = selectedPromo?.PromotionID,
                    BookingDateTime = DateTime.Now.AddDays(-random.Next(1, 5)),
                    TotalAmount = finalAmount,
                    Status = random.NextDouble() < 0.7 ? "Confirmed" : "Pending" // 70% confirmed, 30% pending
                };

                bookings.Add(booking);
            }
        }

        _context.Bookings.AddRange(bookings);
        _context.SaveChanges();
    }

    private void SeedPayments()
    {
        if (_context.Payments.Any()) return;

        var bookings = _context.Bookings.ToList();
        if (!bookings.Any()) return;

        var payments = new List<Payment>();
        var random = new Random();
        var paymentMethods = new[] { "Credit Card", "Debit Card", "Bank Transfer", "PayPal", "Digital Wallet" };

        foreach (var booking in bookings)
            // Create payment for most bookings (90% have payments)
            if (random.NextDouble() < 0.9)
            {
                var paymentMethod = paymentMethods[random.Next(paymentMethods.Length)];
                var paymentStatus = booking.Status == "Confirmed"
                    ? random.NextDouble() < 0.95
                        ? "Completed"
                        : "Pending" // 95% of confirmed bookings have completed payments
                    : random.NextDouble() < 0.3
                        ? "Completed"
                        : "Pending"; // 30% of pending bookings have completed payments

                var payment = new Payment
                {
                    BookingID = booking.BookingID,
                    PaymentGatewayTransactionID = "TXN_" + Guid.NewGuid().ToString("N")[..8].ToUpper(),
                    AmountPaid = booking.TotalAmount,
                    PaymentMethod = paymentMethod,
                    PaymentStatus = paymentStatus,
                    PaymentDateTime =
                        booking.BookingDateTime.AddMinutes(random.Next(1,
                            30)) // Payment made 1-30 minutes after booking
                };

                payments.Add(payment);
            }

        _context.Payments.AddRange(payments);
        _context.SaveChanges();
    }

    private void SeedTickets()
    {
        if (_context.Tickets.Any()) return;

        var bookings = _context.Bookings.ToList();
        var ticketTypes = _context.TicketTypes.ToList();

        if (!bookings.Any() || !ticketTypes.Any()) return;

        var tickets = new List<Ticket>();
        var random = new Random();

        // Create tickets for each booking
        foreach (var booking in bookings)
        {
            // Find ticket type based on booking amount
            var eventTicketTypes = ticketTypes.Where(tt => tt.Price <= booking.TotalAmount * 1.5m).ToList();
            
            if (!eventTicketTypes.Any())
            {
                eventTicketTypes = ticketTypes.ToList();
            }

            if (!eventTicketTypes.Any()) continue;

            TicketType? selectedTicketType = null;

            if (booking.TotalAmount >= 20) // High-end tickets
                selectedTicketType = eventTicketTypes.FirstOrDefault(tt =>
                    tt.Name.Contains("VIP") || tt.Name.Contains("Premium")) ?? eventTicketTypes.FirstOrDefault();
            else if (booking.TotalAmount >= 10) // Mid-range tickets
                selectedTicketType = eventTicketTypes.FirstOrDefault(tt =>
                    tt.Name.Contains("Standard") || tt.Name.Contains("Regular")) ?? eventTicketTypes.FirstOrDefault();
            else // Budget tickets
                selectedTicketType = eventTicketTypes.FirstOrDefault(tt =>
                    tt.Name.Contains("General") || tt.Name.Contains("Balcony")) ?? eventTicketTypes.FirstOrDefault();

            if (selectedTicketType == null) continue;

            // Calculate number of tickets based on booking amount
            var ticketCount = Math.Max(1, (int)(booking.TotalAmount / selectedTicketType.Price));
            ticketCount = Math.Min(ticketCount, 4); // Max 4 tickets per booking

            for (var i = 0; i < ticketCount; i++)
            {
                var isScanned = false;
                DateTime? scannedAt = null;

                var eventForTicket = _context.Events.FirstOrDefault(e => e.EventID == selectedTicketType.EventID);

                if (eventForTicket != null)
                {
                    if (eventForTicket.Status == "Completed" && booking.Status == "Confirmed")
                    {
                        isScanned = random.NextDouble() < 0.8; // 80% chance of being scanned
                        if (isScanned)
                            scannedAt = eventForTicket.StartDateTime.AddHours(random.Next(-2, 2)); // Scanned around event time
                    }
                    else if (eventForTicket.Status == "Active" && booking.Status == "Confirmed")
                    {
                        isScanned = random.NextDouble() < 0.3; // 30% chance of being scanned early
                        if (isScanned)
                            scannedAt = DateTime.Now.AddHours(-random.Next(1, 24)); // Scanned within last 24 hours
                    }
                }

                var ticket = new Ticket
                {
                    BookingID = booking.BookingID,
                    TicketTypeID = selectedTicketType.TicketTypeID,
                    QRCodeValue = "QR_" + Guid.NewGuid().ToString("N")[..12].ToUpper(),
                    IsScanned = isScanned,
                    ScannedAt = scannedAt
                };
                tickets.Add(ticket);
            }
        }

        _context.Tickets.AddRange(tickets);
        _context.SaveChanges();
    }
}