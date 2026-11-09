
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<JobApplication>> GetAllAsync();

        Task<JobApplication?> GetByIdAsync(int id);

        Task<IEnumerable<JobApplication>> GetByStudentIdAsync(
            int studentId);

        Task<IEnumerable<JobApplication>> GetByCompanyIdAsync(
            int companyId);

        Task<IEnumerable<JobApplication>> GetByCollegeIdAsync(
            int collegeId);

        Task<bool> ExistsAsync(int jobId, int studentId);

        Task AddAsync(JobApplication application);

        Task SaveChangesAsync();
    }
}
