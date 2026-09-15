using Microsoft.EntityFrameworkCore;
using VillaManager.Core.Entities;
using VillaManager.Core.Enums;

namespace VillaManager.Infrastructure.Data;

/// <summary>
/// Entity Framework Core DbContext for Villa Manager
/// </summary>
public class VillaManagerDbContext : DbContext
{
    public VillaManagerDbContext(DbContextOptions<VillaManagerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Property> Properties { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Property entity
        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(255);
            entity.Property(p => p.Description).HasColumnType("text");
            entity.Property(p => p.Location).HasMaxLength(255);
            entity.Property(p => p.PricePerNight).HasPrecision(10, 2);
            entity.HasMany(p => p.Bookings)
                .WithOne(b => b.Property)
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(p => p.Messages)
                .WithOne(m => m.Property)
                .HasForeignKey(m => m.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Booking entity
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.GuestName).IsRequired().HasMaxLength(255);
            entity.Property(b => b.GuestEmail).HasMaxLength(255);
            entity.Property(b => b.GuestPhone).HasMaxLength(20);
            entity.Property(b => b.TotalPrice).HasPrecision(10, 2);
            entity.Property(b => b.Status).HasConversion<string>();
            entity.HasOne(b => b.Property)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Create indexes for performance
            entity.HasIndex(b => b.PropertyId);
            entity.HasIndex(b => b.CheckInDate);
            entity.HasIndex(b => b.Status);
        });

        // Configure Message entity
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.SenderName).HasMaxLength(255);
            entity.Property(m => m.SenderPhone).HasMaxLength(20);
            entity.Property(m => m.Content).IsRequired().HasColumnType("text");
            entity.Property(m => m.Source).HasConversion<string>();
            entity.Property(m => m.Type).HasConversion<string>();
            entity.HasOne(m => m.Property)
                .WithMany(p => p.Messages)
                .HasForeignKey(m => m.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(m => m.Booking)
                .WithMany()
                .HasForeignKey(m => m.BookingId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Create indexes for performance
            entity.HasIndex(m => m.PropertyId);
            entity.HasIndex(m => m.BookingId);
            entity.HasIndex(m => m.ReceivedAt).IsDescending();
            entity.HasIndex(m => m.IsRead);
        });

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed a default property
        modelBuilder.Entity<Property>().HasData(
            new Property
            {
                Id = 1,
                Name = "Your Villa Name",
                Description = "Beautiful villa in Indonesia with stunning views",
                Location = "Indonesia",
                PricePerNight = 100m,
                MaxGuests = 6,
                ImageUrls = new[] { "villa1.jpg", "villa2.jpg", "villa3.jpg" },
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
