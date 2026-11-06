
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllAsync();

        Task<Company?> GetByIdAsync(int id);

        Task<Company?> GetByCodeAsync(string companyCode);

        Task<Company?> GetByOfficialEmailAsync(string email);

        Task<IEnumerable<Company>> GetPendingAsync();

        Task AddAsync(Company company);

        Task SaveChangesAsync();
    }
}
