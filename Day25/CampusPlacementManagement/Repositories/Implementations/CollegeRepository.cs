
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class CollegeRepository : ICollegeRepository
    {
        private readonly ApplicationDbContext _context;

        public CollegeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<College>> GetAllAsync()
        {
            return await _context.Colleges
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<College?> GetByIdAsync(int collegeId)
        {
            return await _context.Colleges
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CollegeId == collegeId);
        }

        public async Task<College?> GetByCodeAsync(string collegeCode)
        {
            return await _context.Colleges
                .FirstOrDefaultAsync(c => c.CollegeCode == collegeCode);
        }

        public async Task AddAsync(College college)
        {
            await _context.Colleges.AddAsync(college);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
