
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICollegeRepository
    {
        Task<IEnumerable<College>> GetAllAsync();

        Task<IEnumerable<College>> GetPendingAsync();

        Task<College?> GetByIdAsync(int collegeId);

        Task<College?> GetByIdForUpdateAsync(int collegeId);

        Task<College?> GetByCodeAsync(string collegeCode);

        Task AddAsync(College college);

        Task<int> SaveChangesAsync();
    }
}
