using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using star_events.Models;
using System.Net.Sockets;

namespace star_events.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for ERD entities
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Promotion> Promotions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize Identity table names to match ERD
            builder.Entity<ApplicationUser>().ToTable("Users"); // Updated to ApplicationUser
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            // Configure relationships and constraints based on ERD
            builder.Entity<Event>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryID);

            builder.Entity<Event>()
                .HasOne(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationID);

            builder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(u => u.OrganizedEvents) // Now valid with ApplicationUser
                .HasForeignKey(e => e.OrganizerID);

            builder.Entity<TicketType>()
                .HasOne(tt => tt.Event)
                .WithMany(e => e.TicketTypes)
                .HasForeignKey(tt => tt.EventID);

            builder.Entity<Ticket>()
                .HasOne(t => t.TicketType)
                .WithMany(tt => tt.Tickets) // Now valid with updated TicketType
                .HasForeignKey(t => t.TicketTypeID);

            builder.Entity<Ticket>()
                .HasOne(t => t.Booking)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BookingID);

            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings) // Now valid with ApplicationUser
                .HasForeignKey(b => b.UserID);

            builder.Entity<Booking>()
                .HasOne(b => b.Promotion)
                .WithMany()
                .HasForeignKey(b => b.PromotionID)
                .IsRequired(false); // Optional relationship

            builder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingID);

            // Seed initial data if needed (e.g., roles are seeded in Program.cs)
            // Example: builder.Entity<Category>().HasData(new Category { CategoryID = 1, Name = "Concert" });

        }
    }
}