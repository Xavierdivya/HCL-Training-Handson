
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICompanyHRRepository
    {
        Task<IEnumerable<CompanyHR>> GetAllAsync();

        Task<CompanyHR?> GetByIdAsync(int id);

        Task<CompanyHR?> GetByUserIdAsync(string userId);

        Task<IEnumerable<CompanyHR>> GetByCompanyIdAsync(int companyId);

        Task AddAsync(CompanyHR companyHR);

        Task SaveChangesAsync();
    }
}
