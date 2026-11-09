
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class PlacementDriveRepository : IPlacementDriveRepository
    {
        private readonly ApplicationDbContext _context;

        public PlacementDriveRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlacementDrive>> GetAllAsync()
        {
            return await _context.PlacementDrives
                .Include(pd => pd.College)
                .Include(pd => pd.Company)
                .Include(pd => pd.PlacementOfficer)
                    .ThenInclude(po => po.User)
                .AsNoTracking()
                .OrderByDescending(pd => pd.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlacementDrive>> GetByCollegeIdAsync(
            int collegeId)
        {
            return await _context.PlacementDrives
                .Include(pd => pd.College)
                .Include(pd => pd.Company)
                .Include(pd => pd.PlacementOfficer)
                    .ThenInclude(po => po.User)
                .Where(pd => pd.CollegeId == collegeId)
                .AsNoTracking()
                .OrderByDescending(pd => pd.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlacementDrive>>
            GetByPlacementOfficerIdAsync(int placementOfficerId)
        {
            return await _context.PlacementDrives
                .Include(pd => pd.College)
                .Include(pd => pd.Company)
                .Include(pd => pd.PlacementOfficer)
                    .ThenInclude(po => po.User)
                .Where(pd => pd.PlacementOfficerId == placementOfficerId)
                .AsNoTracking()
                .OrderByDescending(pd => pd.CreatedAt)
                .ToListAsync();
        }

        public async Task<PlacementDrive?> GetByIdAsync(int id)
        {
            return await _context.PlacementDrives
                .Include(pd => pd.College)
                .Include(pd => pd.Company)
                .Include(pd => pd.PlacementOfficer)
                    .ThenInclude(po => po.User)
                .FirstOrDefaultAsync(pd => pd.PlacementDriveId == id);
        }

        public async Task AddAsync(PlacementDrive placementDrive)
        {
            await _context.PlacementDrives.AddAsync(placementDrive);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
