
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories.Implementations
{
    public class ShowRepository : IShowRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Show> AddAsync(Show show)
        {
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();

            return show;
        }

        public async Task<List<Show>> GetAllAsync()
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<Show?> GetByIdAsync(int id)
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .FirstOrDefaultAsync(s => s.ShowId == id);
        }

        public async Task<List<Show>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .Where(s => s.MovieId == movieId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<Show>> GetByScreenIdAsync(int screenId)
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Where(s => s.ScreenId == screenId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<Show>> GetByDateAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .Where(s => s.StartTime >= startOfDay &&
                            s.StartTime < endOfDay)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task UpdateAsync(Show show)
        {
            _context.Shows.Update(show);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var show = await _context.Shows
                .Include(s => s.ShowSeats)
                .FirstOrDefaultAsync(s => s.ShowId == id);

            if (show == null)
                throw new KeyNotFoundException("Show not found.");

            // Prevent deletion if booking records exist
            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.ShowId == id);

            if (hasBookings)
                throw new InvalidOperationException(
                    "Cannot delete this show because bookings already exist.");

            // Delete associated ShowSeat records
            _context.ShowSeats.RemoveRange(show.ShowSeats);

            // Delete the Show
            _context.Shows.Remove(show);

            await _context.SaveChangesAsync();
        }


        public async Task<Show> AddWithSeatsAsync(Show show)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Check whether another show overlaps on the same screen
                bool hasOverlap = await _context.Shows.AnyAsync(s =>
                    s.ScreenId == show.ScreenId &&
                    s.StartTime < show.EndTime &&
                    s.EndTime > show.StartTime);

                if (hasOverlap)
                    throw new InvalidOperationException(
                        "Another show is already scheduled during this time.");

                // Save the show first to generate its ShowId
                await _context.Shows.AddAsync(show);
                await _context.SaveChangesAsync();

                // Retrieve all physical seats belonging to this screen
                var seats = await _context.Seats
                    .Where(s => s.ScreenId == show.ScreenId)
                    .ToListAsync();

                if (seats.Count == 0)
                {
                    throw new InvalidOperationException(
                        "Cannot create a show because this screen has no seats.");
                }

                // Create an Available ShowSeat for every physical seat
                var showSeats = seats.Select(seat => new ShowSeat
                {
                    ShowId = show.ShowId,
                    SeatId = seat.SeatId,
                    Status = "Available"
                }).ToList();

                await _context.ShowSeats.AddRangeAsync(showSeats);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return show;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> HasScheduleConflictAsync(
            int screenId,
            DateTime startTime,
            DateTime endTime,
            int? excludeShowId = null)
        {
            return await _context.Shows.AnyAsync(s =>
                s.ScreenId == screenId &&
                s.StartTime < endTime &&
                s.EndTime > startTime &&
                (!excludeShowId.HasValue ||
                 s.ShowId != excludeShowId.Value));
        }


    }
}
