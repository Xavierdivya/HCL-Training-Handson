using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private IQueryable<InterviewSchedule> QueryWithDetails()
        {
            return _context.InterviewSchedules
                .Include(i => i.JobApplication)
                    .ThenInclude(a => a.Job)
                        .ThenInclude(j => j.Company)
                .Include(i => i.JobApplication)
                    .ThenInclude(a => a.Student)
                        .ThenInclude(s => s.User)
                .Include(i => i.CompanyHR)
                    .ThenInclude(hr => hr.User)
                .Include(i => i.Company);
        }

        public async Task<IEnumerable<InterviewSchedule>> GetAllAsync()
        {
            return await QueryWithDetails()
                .AsNoTracking()
                .OrderBy(i => i.ScheduledAt)
                .ToListAsync();
        }

        public async Task<InterviewSchedule?> GetByIdAsync(int id)
        {
            return await QueryWithDetails()
                .FirstOrDefaultAsync(
                    i => i.InterviewScheduleId == id);
        }

        public async Task<IEnumerable<InterviewSchedule>>
            GetByCompanyIdAsync(int companyId)
        {
            return await QueryWithDetails()
                .Where(i => i.CompanyId == companyId)
                .AsNoTracking()
                .OrderBy(i => i.ScheduledAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<InterviewSchedule>>
            GetByStudentIdAsync(int studentId)
        {
            return await QueryWithDetails()
                .Where(i =>
                    i.JobApplication.StudentId == studentId)
                .AsNoTracking()
                .OrderBy(i => i.ScheduledAt)
                .ToListAsync();
        }

        public async Task AddAsync(InterviewSchedule interview)
        {
            await _context.InterviewSchedules.AddAsync(interview);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}