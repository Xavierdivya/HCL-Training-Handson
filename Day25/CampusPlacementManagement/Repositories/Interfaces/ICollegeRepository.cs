
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICollegeRepository
    {
        Task<IEnumerable<College>> GetAllAsync();

        Task<College?> GetByIdAsync(int collegeId);

        Task<College?> GetByCodeAsync(string collegeCode);

        Task AddAsync(College college);

        Task<int> SaveChangesAsync();
    }
}
