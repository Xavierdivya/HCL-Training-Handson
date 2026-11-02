
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
    }
}
