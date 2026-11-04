
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories.Implementations
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Seat> AddAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();

            return seat;
        }

        public async Task AddRangeAsync(IEnumerable<Seat> seats)
        {
            await _context.Seats.AddRangeAsync(seats);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Seat>> GetAllAsync()
        {
            return await _context.Seats
                .AsNoTracking()
                .OrderBy(s => s.ScreenId)
                .ThenBy(s => s.RowName)
                .ThenBy(s => s.SeatNumber)
                .ToListAsync();
        }

        public async Task<Seat?> GetByIdAsync(int id)
        {
            return await _context.Seats
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SeatId == id);
        }

        public async Task<List<Seat>> GetByScreenIdAsync(int screenId)
        {
            return await _context.Seats
                .AsNoTracking()
                .Where(s => s.ScreenId == screenId)
                .OrderBy(s => s.RowName)
                .ThenBy(s => s.SeatNumber)
                .ToListAsync();
        }

        public async Task UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var seat = await _context.Seats.FindAsync(id);

            if (seat == null)
            {
                return false;
            }

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
