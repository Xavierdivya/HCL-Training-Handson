using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Show)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<Booking?> GetByReferenceAsync(string reference)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Show)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .FirstOrDefaultAsync(b => b.BookingReference == reference);
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        // Create booking and reserve seats in one transaction
        public async Task<Booking> CreateBookingWithSeatsAsync(
            Booking booking,
            List<int> seatIds)
        {
            if (seatIds == null || seatIds.Count == 0)
                throw new InvalidOperationException(
                    "At least one seat must be selected.");

            var uniqueSeatIds = seatIds.Distinct().ToList();

            if (uniqueSeatIds.Count != seatIds.Count)
                throw new InvalidOperationException(
                    "Duplicate seat selection is not allowed.");

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Find the show and its ticket price
                var show = await _context.Shows
                    .FirstOrDefaultAsync(s => s.ShowId == booking.ShowId);

                if (show == null)
                    throw new InvalidOperationException(
                        "Show not found.");

                // Retrieve selected seats for this particular show
                var showSeats = await _context.ShowSeats
                    .Where(ss =>
                        ss.ShowId == booking.ShowId &&
                        uniqueSeatIds.Contains(ss.SeatId))
                    .ToListAsync();

                if (showSeats.Count != uniqueSeatIds.Count)
                    throw new InvalidOperationException(
                        "One or more selected seats do not belong to this show.");

                if (showSeats.Any(ss => ss.Status != "Available"))
                    throw new InvalidOperationException(
                        "One or more selected seats are already booked.");

                // Calculate the amount on the server
                booking.TotalAmount =
                    show.TicketPrice * uniqueSeatIds.Count;

                booking.BookingDate = DateTime.Now;
                booking.Status = "Confirmed";

                // Save booking to generate BookingId
                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

                // Create booking-seat records
                var bookingSeats = uniqueSeatIds.Select(seatId =>
                    new BookingSeat
                    {
                        BookingId = booking.BookingId,
                        SeatId = seatId,
                        Price = show.TicketPrice
                    }).ToList();

                await _context.BookingSeats.AddRangeAsync(bookingSeats);

                // Mark selected show seats as booked
                foreach (var showSeat in showSeats)
                {
                    showSeat.Status = "Booked";
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return booking;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Cancel booking and release its seats in one transaction
        public async Task CancelBookingAsync(int bookingId)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.BookingSeats)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);

                if (booking == null)
                    throw new InvalidOperationException(
                        "Booking not found.");

                if (booking.Status != "Confirmed")
                    throw new InvalidOperationException(
                        "Only confirmed bookings can be cancelled.");

                var seatIds = booking.BookingSeats
                    .Select(bs => bs.SeatId)
                    .ToList();

                var showSeats = await _context.ShowSeats
                    .Where(ss =>
                        ss.ShowId == booking.ShowId &&
                        seatIds.Contains(ss.SeatId))
                    .ToListAsync();

                if (showSeats.Count != seatIds.Count)
                    throw new InvalidOperationException(
                        "Could not find all seats associated with this booking.");

                // Release seats
                foreach (var showSeat in showSeats)
                {
                    showSeat.Status = "Available";
                }

                // Preserve booking history
                booking.Status = "Cancelled";

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}