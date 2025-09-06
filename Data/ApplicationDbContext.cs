using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using star_events.Models;
using System.Net.Sockets;

namespace star_events.Data
{
<<<<<<< HEAD
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int>
=======
    public class ApplicationDbContext : IdentityDbContext
>>>>>>> 1a6e91a5c3e183d04a96b5123e9fbd038a46d20b
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

            // Customize Identity table names
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.Id).HasColumnName("UserID"); // Map Id to UserID
            });
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");


            // Optional: Configure RoleID relationship (if used)
            // builder.Entity<ApplicationUser>()
            //     .HasOne(u => u.Role)
            //     .WithMany()
            //     .HasForeignKey(u => u.RoleID)
            //     .IsRequired(false);

            // Configure relationships
            builder.Entity<Event>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryID);

            builder.Entity<Event>()
                .HasOne(e => e.Location)
                .WithMany(l => l.Events)
                .HasForeignKey(e => e.LocationID);

            builder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(u => u.OrganizedEvents)
                .HasForeignKey(e => e.OrganizerID);

            builder.Entity<TicketType>()
                .HasOne(tt => tt.Event)
                .WithMany(e => e.TicketTypes)
                .HasForeignKey(tt => tt.EventID);

            builder.Entity<Ticket>()
                .HasOne(t => t.TicketType)
                .WithMany(tt => tt.Tickets)
                .HasForeignKey(t => t.TicketTypeID);

            builder.Entity<Ticket>()
                .HasOne(t => t.Booking)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BookingID);

            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserID);

            builder.Entity<Booking>()
                .HasOne(b => b.Promotion)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PromotionID)
                .IsRequired(false);

            builder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingID);

            //builder.Entity<ApplicationUser>()
            //    .HasOne(u => u.Role)
            //    .WithMany()
            //    .HasForeignKey(u => u.RoleID);

            // Explicit value generation for INT PKs (MySQL AUTO_INCREMENT)
            builder.Entity<Event>().Property(e => e.EventID).ValueGeneratedOnAdd();
            builder.Entity<Category>().Property(c => c.CategoryID).ValueGeneratedOnAdd();
            builder.Entity<Location>().Property(l => l.LocationID).ValueGeneratedOnAdd();
            builder.Entity<TicketType>().Property(tt => tt.TicketTypeID).ValueGeneratedOnAdd();
            builder.Entity<Ticket>().Property(t => t.TicketID).ValueGeneratedOnAdd();
            builder.Entity<Booking>().Property(b => b.BookingID).ValueGeneratedOnAdd();
            builder.Entity<Payment>().Property(p => p.PaymentID).ValueGeneratedOnAdd();
            builder.Entity<Promotion>().Property(p => p.PromotionID).ValueGeneratedOnAdd();

            // Seed initial data if needed (e.g., roles are seeded in Program.cs)
            // Example: builder.Entity<Category>().HasData(new Category { CategoryID = 1, Name = "Concert" });
            // Seed roles if not already seeded in Program.cs
            builder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Id = 2, Name = "Organizer", NormalizedName = "ORGANIZER" },
                new Role { Id = 3, Name = "Customer", NormalizedName = "CUSTOMER" }
                );

        }
    }
}