
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class PlacementOfficerRepository : IPlacementOfficerRepository
    {
        private readonly ApplicationDbContext _context;

        public PlacementOfficerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlacementOfficer>> GetAllAsync()
        {
            return await _context.PlacementOfficers
                .Include(p => p.User)
                .Include(p => p.College)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PlacementOfficer?> GetByIdAsync(int id)
        {
            return await _context.PlacementOfficers
                .Include(p => p.User)
                .Include(p => p.College)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PlacementOfficerId == id);
        }

        public async Task<PlacementOfficer?> GetByUserIdAsync(string userId)
        {
            return await _context.PlacementOfficers
                .Include(p => p.User)
                .Include(p => p.College)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddAsync(PlacementOfficer placementOfficer)
        {
            await _context.PlacementOfficers.AddAsync(placementOfficer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
