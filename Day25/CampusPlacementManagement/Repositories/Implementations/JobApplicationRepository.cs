
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Common query including related Job, Company,
        // Student and Student User information.
        private IQueryable<JobApplication> QueryWithDetails()
        {
            return _context.JobApplications
                .Include(ja => ja.Job)
                    .ThenInclude(j => j.Company)
                .Include(ja => ja.Student)
                    .ThenInclude(s => s.User);
        }

        public async Task<IEnumerable<JobApplication>> GetAllAsync()
        {
            return await QueryWithDetails()
                .AsNoTracking()
                .OrderByDescending(ja => ja.AppliedAt)
                .ToListAsync();
        }

        public async Task<JobApplication?> GetByIdAsync(int id)
        {
            // Tracked entity, required for status updates.
            return await QueryWithDetails()
                .FirstOrDefaultAsync(
                    ja => ja.JobApplicationId == id);
        }

        public async Task<IEnumerable<JobApplication>> GetByStudentIdAsync(
            int studentId)
        {
            return await QueryWithDetails()
                .Where(ja => ja.StudentId == studentId)
                .AsNoTracking()
                .OrderByDescending(ja => ja.AppliedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetByCompanyIdAsync(
            int companyId)
        {
            return await QueryWithDetails()
                .Where(ja => ja.Job.CompanyId == companyId)
                .AsNoTracking()
                .OrderByDescending(ja => ja.AppliedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetByCollegeIdAsync(
            int collegeId)
        {
            return await QueryWithDetails()
                .Where(ja => ja.Student.CollegeId == collegeId)
                .AsNoTracking()
                .OrderByDescending(ja => ja.AppliedAt)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(
            int jobId,
            int studentId)
        {
            return await _context.JobApplications
                .AnyAsync(ja =>
                    ja.JobId == jobId &&
                    ja.StudentId == studentId);
        }

        public async Task AddAsync(JobApplication application)
        {
            await _context.JobApplications.AddAsync(application);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
