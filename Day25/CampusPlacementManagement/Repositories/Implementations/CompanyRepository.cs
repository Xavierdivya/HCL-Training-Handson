
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies
                .Include(c => c.HRAccounts)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _context.Companies
                .Include(c => c.HRAccounts)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CompanyId == id);
        }

        public async Task<Company?> GetByCodeAsync(string companyCode)
        {
            return await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CompanyCode == companyCode);
        }

        public async Task<Company?> GetByOfficialEmailAsync(string email)
        {
            return await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.OfficialEmail == email);
        }

        public async Task<IEnumerable<Company>> GetPendingAsync()
        {
            return await _context.Companies
                .Where(c => c.Status == CompanyStatus.Pending)
                .Include(c => c.HRAccounts)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
