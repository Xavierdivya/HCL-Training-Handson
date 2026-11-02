using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Database Tables
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Theatre> Theatres { get; set; }

        public DbSet<Screen> Screens { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Show> Shows { get; set; }

        public DbSet<ShowSeat> ShowSeats { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<BookingSeat> BookingSeats { get; set; }

        // Configure entity relationships and constraints

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. MOVIE ENTITY CONFIGURATION
            // ==========================================

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("Movies", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Movies_DurationMinutes",
                        "[DurationMinutes] > 0");
                });

                entity.HasKey(m => m.MovieId);

                entity.Property(m => m.Title)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(m => m.Description)
                      .HasMaxLength(500);

                entity.Property(m => m.DurationMinutes)
                      .IsRequired();

                entity.Property(m => m.ReleaseDate)
                      .HasColumnType("date")
                      .IsRequired();

                entity.Property(m => m.Language)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(m => m.Genre)
                      .IsRequired()
                      .HasMaxLength(50);
            });


            // ==========================================
            // 2. THEATRE ENTITY CONFIGURATION
            // ==========================================

            modelBuilder.Entity<Theatre>(entity =>
            {
                entity.ToTable("Theatres");

                entity.HasKey(t => t.TheatreId);

                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(t => t.Location)
                      .IsRequired()
                      .HasMaxLength(200);
            });


            // ==========================================
            // 3. SCREEN ENTITY CONFIGURATION
            // ==========================================

            modelBuilder.Entity<Screen>(entity =>
            {
                entity.ToTable("Screens");

                entity.HasKey(s => s.ScreenId);

                entity.Property(s => s.ScreenName)
                      .IsRequired()
                      .HasMaxLength(50);

                // Theatre 1 -> Many Screens
                entity.HasOne(s => s.Theatre)
                      .WithMany(t => t.Screens)
                      .HasForeignKey(s => s.TheatreId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique screen name within a theatre
                entity.HasIndex(s => new
                {
                    s.TheatreId,
                    s.ScreenName
                }).IsUnique();
            });


            // ==========================================
            // 4. SEAT ENTITY CONFIGURATION
            // ==========================================

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.ToTable("Seats", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Seats_SeatNumber",
                        "[SeatNumber] > 0");
                });

                entity.HasKey(s => s.SeatId);

                entity.Property(s => s.RowName)
                      .IsRequired()
                      .HasMaxLength(5);

                entity.Property(s => s.SeatNumber)
                      .IsRequired();

                entity.Property(s => s.SeatType)
                      .IsRequired()
                      .HasMaxLength(30);

                // Screen 1 -> Many Seats
                entity.HasOne(s => s.Screen)
                      .WithMany(sc => sc.Seats)
                      .HasForeignKey(s => s.ScreenId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique physical seat coordinates within a screen
                entity.HasIndex(s => new
                {
                    s.ScreenId,
                    s.RowName,
                    s.SeatNumber
                }).IsUnique();
            });


            // ==========================================
            // 5. SHOW ENTITY CONFIGURATION
            // ==========================================

            modelBuilder.Entity<Show>(entity =>
            {
                entity.ToTable("Shows", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Shows_EndTime",
                        "[EndTime] > [StartTime]");

                    table.HasCheckConstraint(
                        "CK_Shows_TicketPrice",
                        "[TicketPrice] >= 0");
                });

                entity.HasKey(s => s.ShowId);

                entity.Property(s => s.StartTime)
                      .HasColumnType("datetime2")
                      .IsRequired();

                entity.Property(s => s.EndTime)
                      .HasColumnType("datetime2")
                      .IsRequired();

                entity.Property(s => s.TicketPrice)
                      .HasPrecision(10, 2)
                      .IsRequired();

                // Movie 1 -> Many Shows
                entity.HasOne(s => s.Movie)
                      .WithMany(m => m.Shows)
                      .HasForeignKey(s => s.MovieId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Screen 1 -> Many Shows
                entity.HasOne(s => s.Screen)
                      .WithMany(sc => sc.Shows)
                      .HasForeignKey(s => s.ScreenId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // ==========================================
            // 6. SHOWSEAT ENTITY CONFIGURATION
            // ==========================================

            modelBuilder.Entity<ShowSeat>(entity =>
            {
                entity.ToTable("ShowSeats", table =>
                {
                    table.HasCheckConstraint(
                        "CK_ShowSeats_Status",
                        "[Status] IN (N'Available', N'Booked')");
                });

                entity.HasKey(ss => ss.ShowSeatId);

                entity.Property(ss => ss.Status)
                      .IsRequired()
                      .HasMaxLength(20);

                // Show 1 -> Many ShowSeats
                entity.HasOne(ss => ss.Show)
                      .WithMany(s => s.ShowSeats)
                      .HasForeignKey(ss => ss.ShowId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Seat 1 -> Many ShowSeats
                entity.HasOne(ss => ss.Seat)
                      .WithMany(s => s.ShowSeats)
                      .HasForeignKey(ss => ss.SeatId)
                      .OnDelete(DeleteBehavior.Restrict);

                // One availability record per seat per show
                entity.HasIndex(ss => new
                {
                    ss.ShowId,
                    ss.SeatId
                }).IsUnique();
            });


            // ==========================================
            // 7. BOOKING ENTITY CONFIGURATION
            // ==========================================

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Bookings_TotalAmount",
                        "[TotalAmount] >= 0");

                    table.HasCheckConstraint(
                        "CK_Bookings_Status",
                        "[Status] IN (N'Confirmed', N'Cancelled')");
                });

                entity.HasKey(b => b.BookingId);

                entity.Property(b => b.BookingReference)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(b => b.CustomerName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(b => b.CustomerEmail)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(b => b.CustomerPhone)
                      .IsRequired()
                      .HasMaxLength(15);

                entity.Property(b => b.BookingDate)
                      .HasColumnType("datetime2")
                      .IsRequired();

                entity.Property(b => b.TotalAmount)
                      .HasPrecision(10, 2)
                      .IsRequired();

                entity.Property(b => b.Status)
                      .IsRequired()
                      .HasMaxLength(20);

                // Unique booking reference
                entity.HasIndex(b => b.BookingReference)
                      .IsUnique();

                // Show 1 -> Many Bookings
                entity.HasOne(b => b.Show)
                      .WithMany(s => s.Bookings)
                      .HasForeignKey(b => b.ShowId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // ==========================================
            // 8. BOOKINGSEAT ENTITY CONFIGURATION
            // ==========================================

            modelBuilder.Entity<BookingSeat>(entity =>
            {
                entity.ToTable("BookingSeats", table =>
                {
                    table.HasCheckConstraint(
                        "CK_BookingSeats_Price",
                        "[Price] >= 0");
                });

                entity.HasKey(bs => bs.BookingSeatId);

                entity.Property(bs => bs.Price)
                      .HasPrecision(10, 2)
                      .IsRequired();

                // Booking 1 -> Many BookingSeats
                entity.HasOne(bs => bs.Booking)
                      .WithMany(b => b.BookingSeats)
                      .HasForeignKey(bs => bs.BookingId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Seat 1 -> Many BookingSeats
                entity.HasOne(bs => bs.Seat)
                      .WithMany(s => s.BookingSeats)
                      .HasForeignKey(bs => bs.SeatId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }



    }
}
