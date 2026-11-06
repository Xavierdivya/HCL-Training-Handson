
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class CompanyHRRepository : ICompanyHRRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyHRRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyHR>> GetAllAsync()
        {
            return await _context.CompanyHRs
                .Include(h => h.User)
                .Include(h => h.Company)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CompanyHR?> GetByIdAsync(int id)
        {
            return await _context.CompanyHRs
                .Include(h => h.User)
                .Include(h => h.Company)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.CompanyHRId == id);
        }

        public async Task<CompanyHR?> GetByUserIdAsync(string userId)
        {
            return await _context.CompanyHRs
                .Include(h => h.User)
                .Include(h => h.Company)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.UserId == userId);
        }

        public async Task<IEnumerable<CompanyHR>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.CompanyHRs
                .Where(h => h.CompanyId == companyId)
                .Include(h => h.User)
                .Include(h => h.Company)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(CompanyHR companyHR)
        {
            await _context.CompanyHRs.AddAsync(companyHR);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
