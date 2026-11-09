using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllAsync();

        Task<IEnumerable<Job>> GetByCompanyIdAsync(int companyId);

        Task<IEnumerable<Job>> GetOpenJobsAsync();

        Task<Job?> GetByIdAsync(int id);

        Task AddAsync(Job job);

        Task SaveChangesAsync();
    }
}