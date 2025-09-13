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
                OrganizerId = organizer.Id,
                LocationID = nelumPokuna.LocationID,
                CategoryID = concertCategory.CategoryID,
                ImageURL = "https://placehold.co/1024x768/334155/FFFFFF?text=Symphony+of+Strings",
                Status = "Upcoming"
            },
            new Event
            {
                Title = "Colombo International Theatre Festival",
                Description =
                    "Showcasing the best of local and international drama. A week-long festival of captivating performances, workshops, and artist talks.",
                StartDateTime = DateTime.Now.AddDays(70).Date.AddHours(18), // 6 PM
                EndDateTime = DateTime.Now.AddDays(70).Date.AddHours(21), // 9 PM
                OrganizerId = organizer.Id,
                LocationID = lionelWendt.LocationID,
                CategoryID = theatreCategory.CategoryID,
                ImageURL = "https://placehold.co/1024x768/78350F/FFFFFF?text=Theatre+Festival",
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
                    EventID = symphonyEvent.EventID, Name = "VIP Box", Price = 7500, TotalQuantity = 100,
                    AvailableQuantity = 100
                },
                new TicketType
                {
                    EventID = symphonyEvent.EventID, Name = "Balcony", Price = 4000, TotalQuantity = 400,
                    AvailableQuantity = 400
                },
                new TicketType
                {
                    EventID = symphonyEvent.EventID, Name = "Orchestra Stalls", Price = 2500, TotalQuantity = 788,
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
                    EventID = theatreEvent.EventID, Name = "Premium Seating", Price = 3500, TotalQuantity = 200,
                    AvailableQuantity = 200
                },
                new TicketType
                {
                    EventID = theatreEvent.EventID, Name = "General Admission", Price = 1500, TotalQuantity = 422,
                    AvailableQuantity = 422
                }
            };
            _context.TicketTypes.AddRange(theatreTickets);
        }

        _context.SaveChanges();
    }
}