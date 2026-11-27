
using HotelBooking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<HotelImage> HotelImages { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<HotelAmenity> HotelAmenities { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<RoomTypeAmenity> RoomTypeAmenities { get; set; }
    public DbSet<RoomInventory> RoomInventories { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingItem> BookingItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Hotel
        builder.Entity<Hotel>()
            .HasMany(h => h.HotelImages)
            .WithOne(i => i.Hotel)
            .HasForeignKey(i => i.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Hotel>()
            .HasMany(h => h.RoomTypes)
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        // Hotel Amenity
        builder.Entity<HotelAmenity>()
            .HasKey(ha => new { ha.HotelId, ha.AmenityId });

        builder.Entity<HotelAmenity>()
            .HasOne(ha => ha.Hotel)
            .WithMany(h => h.HotelAmenities)
            .HasForeignKey(ha => ha.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<HotelAmenity>()
            .HasOne(ha => ha.Amenity)
            .WithMany(a => a.HotelAmenities)
            .HasForeignKey(ha => ha.AmenityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Room Type Amenity
        builder.Entity<RoomTypeAmenity>()
            .HasKey(ra => new { ra.RoomTypeId, ra.AmenityId });

        builder.Entity<RoomTypeAmenity>()
            .HasOne(ra => ra.RoomType)
            .WithMany(r => r.RoomTypeAmenities)
            .HasForeignKey(ra => ra.RoomTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RoomTypeAmenity>()
            .HasOne(ra => ra.Amenity)
            .WithMany(a => a.RoomTypeAmenities)
            .HasForeignKey(ra => ra.AmenityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Room Inventory
        builder.Entity<RoomInventory>()
            .HasKey(i => i.InventoryId);

        builder.Entity<RoomInventory>()
            .HasOne(i => i.RoomType)
            .WithMany(r => r.RoomInventories)
            .HasForeignKey(i => i.RoomTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RoomInventory>()
            .HasIndex(i => new { i.RoomTypeId, i.InventoryDate })
            .IsUnique();

        // Room Type
        builder.Entity<RoomType>()
            .Property(r => r.PricePerNight)
            .HasPrecision(18, 2);

        builder.Entity<RoomType>()
            .HasIndex(r => new { r.HotelId, r.Name })
            .IsUnique();

        // Booking
        builder.Entity<Booking>()
            .HasIndex(b => b.BookingReference)
            .IsUnique();

        builder.Entity<Booking>()
            .Property(b => b.TotalAmount)
            .HasPrecision(18, 2);

        builder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Booking>()
            .HasOne(b => b.Hotel)
            .WithMany(h => h.Bookings)
            .HasForeignKey(b => b.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        // Booking Item
        builder.Entity<BookingItem>()
            .HasOne(i => i.Booking)
            .WithMany(b => b.BookingItems)
            .HasForeignKey(i => i.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BookingItem>()
            .HasOne(i => i.RoomType)
            .WithMany(r => r.BookingItems)
            .HasForeignKey(i => i.RoomTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<BookingItem>()
            .Property(i => i.PricePerNight)
            .HasPrecision(18, 2);

        builder.Entity<BookingItem>()
            .Property(i => i.SubTotal)
            .HasPrecision(18, 2);
    }
}
