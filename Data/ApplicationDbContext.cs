using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using star_events.Models;

namespace star_events.Data;

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

        // Configure ApplicationUser relationships
        // builder.Entity<ApplicationUser>()
        //     .HasMany(u => u.OrganizedEvents)
        //     .WithOne(e => e.Organizer)
        //     .HasForeignKey(e => e.OrganizerID)
        //     .IsRequired(false)
        //     .OnDelete(DeleteBehavior.SetNull);

        // builder.Entity<ApplicationUser>()
        //     .HasMany(u => u.Bookings)
        //     .WithOne(b => b.User)
        //     .HasForeignKey(b => b.UserID)
        //     .OnDelete(DeleteBehavior.Cascade);

        // // Configure Event relationships
        // builder.Entity<Event>()
        //     .HasOne(e => e.Category)
        //     .WithMany(c => c.Events)
        //     .HasForeignKey(e => e.CategoryID)
        //     .OnDelete(DeleteBehavior.Restrict);

        // builder.Entity<Event>()
        //     .HasOne(e => e.Location)
        //     .WithMany(l => l.Events)
        //     .HasForeignKey(e => e.LocationID)
        //     .OnDelete(DeleteBehavior.Restrict);

        // builder.Entity<Event>()
        //     .HasMany(e => e.Bookings)
        //     .WithOne(b => b.Event)
        //     .HasForeignKey(b => b.EventID)
        //     .OnDelete(DeleteBehavior.Cascade);

        // // Configure Category relationships (one-to-many with Events)
        // builder.Entity<Category>()
        //     .HasMany(c => c.Events)
        //     .WithOne(e => e.Category)
        //     .HasForeignKey(e => e.CategoryID)
        //     .OnDelete(DeleteBehavior.Restrict);

        // // Configure Location relationships (one-to-many with Events)
        // builder.Entity<Location>()
        //     .HasMany(l => l.Events)
        //     .WithOne(e => e.Location)
        //     .HasForeignKey(e => e.LocationID)
        //     .OnDelete(DeleteBehavior.Restrict);

        // // Configure TicketType relationships
        // builder.Entity<TicketType>()
        //     .HasOne(tt => tt.Event)
        //     .WithMany()
        //     .HasForeignKey(tt => tt.EventID)
        //     .OnDelete(DeleteBehavior.Cascade);

        // builder.Entity<TicketType>()
        //     .HasMany(tt => tt.Tickets)
        //     .WithOne(t => t.TicketType)
        //     .HasForeignKey(t => t.TicketTypeID)
        //     .OnDelete(DeleteBehavior.Cascade);

        // // Configure Ticket relationships
        // builder.Entity<Ticket>()
        //     .HasOne(t => t.Booking)
        //     .WithMany(b => b.Tickets)
        //     .HasForeignKey(t => t.BookingID)
        //     .OnDelete(DeleteBehavior.Cascade);

        // builder.Entity<Ticket>()
        //     .HasOne(t => t.TicketType)
        //     .WithMany(tt => tt.Tickets)
        //     .HasForeignKey(t => t.TicketTypeID)
        //     .OnDelete(DeleteBehavior.Restrict);

        // // Configure Booking relationships
        // builder.Entity<Booking>()
        //     .HasOne(b => b.User)
        //     .WithMany(u => u.Bookings)
        //     .HasForeignKey(b => b.UserID)
        //     .OnDelete(DeleteBehavior.Cascade);

        // builder.Entity<Booking>()
        //     .HasOne(b => b.Event)
        //     .WithMany(e => e.Bookings)
        //     .HasForeignKey(b => b.EventID)
        //     .OnDelete(DeleteBehavior.Restrict);

        // builder.Entity<Booking>()
        //     .HasOne(b => b.Promotion)
        //     .WithMany(p => p.Bookings)
        //     .HasForeignKey(b => b.PromotionID)
        //     .IsRequired(false)
        //     .OnDelete(DeleteBehavior.SetNull);

        // builder.Entity<Booking>()
        //     .HasMany(b => b.Tickets)
        //     .WithOne(t => t.Booking)
        //     .HasForeignKey(t => t.BookingID)
        //     .OnDelete(DeleteBehavior.Cascade);

        // builder.Entity<Booking>()
        //     .HasMany(b => b.Payments)
        //     .WithOne(p => p.Booking)
        //     .HasForeignKey(p => p.BookingID)
        //     .OnDelete(DeleteBehavior.Cascade);

        // // Configure Payment relationships
        // builder.Entity<Payment>()
        //     .HasOne(p => p.Booking)
        //     .WithMany(b => b.Payments)
        //     .HasForeignKey(p => p.BookingID)
        //     .OnDelete(DeleteBehavior.Cascade);

        // // Configure Promotion relationships
        // builder.Entity<Promotion>()
        //     .HasOne(p => p.Event)
        //     .WithMany()
        //     .HasForeignKey(p => p.EventID)
        //     .IsRequired(false)
        //     .OnDelete(DeleteBehavior.SetNull);

        // builder.Entity<Promotion>()
        //     .HasMany(p => p.Bookings)
        //     .WithOne(b => b.Promotion)
        //     .HasForeignKey(b => b.PromotionID)
        //     .IsRequired(false)
        //     .OnDelete(DeleteBehavior.SetNull);

        // // Configure decimal precision for monetary fields
        // builder.Entity<Booking>()
        //     .Property(b => b.TotalAmount)
        //     .HasPrecision(18, 2);

        // builder.Entity<Payment>()
        //     .Property(p => p.AmountPaid)
        //     .HasPrecision(18, 2);

        // builder.Entity<TicketType>()
        //     .Property(tt => tt.Price)
        //     .HasPrecision(18, 2);

        // builder.Entity<Promotion>()
        //     .Property(p => p.DiscountValue)
        //     .HasPrecision(18, 2);

        // // Configure string length constraints
        // builder.Entity<Event>()
        //     .Property(e => e.Title)
        //     .HasMaxLength(100);

        // builder.Entity<Event>()
        //     .Property(e => e.Description)
        //     .HasMaxLength(500);

        // builder.Entity<Event>()
        //     .Property(e => e.Status)
        //     .HasMaxLength(20);

        // builder.Entity<Category>()
        //     .Property(c => c.Name)
        //     .HasMaxLength(50);

        // builder.Entity<Category>()
        //     .Property(c => c.Description)
        //     .HasMaxLength(255);

        // builder.Entity<Location>()
        //     .Property(l => l.Name)
        //     .HasMaxLength(100);

        // builder.Entity<Location>()
        //     .Property(l => l.Address)
        //     .HasMaxLength(255);

        // builder.Entity<Location>()
        //     .Property(l => l.City)
        //     .HasMaxLength(50);

        // builder.Entity<TicketType>()
        //     .Property(tt => tt.Name)
        //     .HasMaxLength(50);

        // builder.Entity<Ticket>()
        //     .Property(t => t.QRCodeValue)
        //     .HasMaxLength(255);

        // builder.Entity<Booking>()
        //     .Property(b => b.Status)
        //     .HasMaxLength(20);

        // builder.Entity<Payment>()
        //     .Property(p => p.PaymentGatewayTransactionID)
        //     .HasMaxLength(100);

        // builder.Entity<Payment>()
        //     .Property(p => p.PaymentMethod)
        //     .HasMaxLength(50);

        // builder.Entity<Payment>()
        //     .Property(p => p.PaymentStatus)
        //     .HasMaxLength(20);

        // builder.Entity<Promotion>()
        //     .Property(p => p.PromoCode)
        //     .HasMaxLength(20);

        // builder.Entity<Promotion>()
        //     .Property(p => p.DiscountType)
        //     .HasMaxLength(20);

        // // Configure indexes for better performance
        // builder.Entity<Event>()
        //     .HasIndex(e => e.StartDateTime)
        //     .HasDatabaseName("IX_Events_StartDateTime");

        // builder.Entity<Event>()
        //     .HasIndex(e => e.Status)
        //     .HasDatabaseName("IX_Events_Status");

        // builder.Entity<Booking>()
        //     .HasIndex(b => b.BookingDateTime)
        //     .HasDatabaseName("IX_Bookings_BookingDateTime");

        // builder.Entity<Booking>()
        //     .HasIndex(b => b.Status)
        //     .HasDatabaseName("IX_Bookings_Status");

        // builder.Entity<Ticket>()
        //     .HasIndex(t => t.QRCodeValue)
        //     .HasDatabaseName("IX_Tickets_QRCodeValue");

        // builder.Entity<Ticket>()
        //     .HasIndex(t => t.IsScanned)
        //     .HasDatabaseName("IX_Tickets_IsScanned");

        // builder.Entity<Promotion>()
        //     .HasIndex(p => p.PromoCode)
        //     .HasDatabaseName("IX_Promotions_PromoCode");

        // builder.Entity<Promotion>()
        //     .HasIndex(p => p.IsActive)
        //     .HasDatabaseName("IX_Promotions_IsActive");
    }
}