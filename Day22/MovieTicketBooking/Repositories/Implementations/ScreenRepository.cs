
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories.Implementations
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly ApplicationDbContext _context;

        public ScreenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Screen> AddAsync(Screen screen)
        {
            await _context.Screens.AddAsync(screen);
            await _context.SaveChangesAsync();

            return screen;
        }

        public async Task<List<Screen>> GetAllAsync()
        {
            return await _context.Screens
                .AsNoTracking()
                .Include(s => s.Theatre)
                .OrderBy(s => s.ScreenName)
                .ToListAsync();
        }

        public async Task<Screen?> GetByIdAsync(int id)
        {
            return await _context.Screens
                .AsNoTracking()
                .Include(s => s.Theatre)
                .FirstOrDefaultAsync(s => s.ScreenId == id);
        }

        public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
        {
            return await _context.Screens
                .AsNoTracking()
                .Where(s => s.TheatreId == theatreId)
                .OrderBy(s => s.ScreenName)
                .ToListAsync();
        }

        public async Task UpdateAsync(Screen screen)
        {
            _context.Screens.Update(screen);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var screen = await _context.Screens.FindAsync(id);

            if (screen == null)
            {
                return false;
            }

            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
