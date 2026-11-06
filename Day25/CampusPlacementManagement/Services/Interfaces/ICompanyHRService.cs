
using CampusPlacementManagement.DTOs.CompanyHRs;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICompanyHRService
    {
        Task<IEnumerable<CompanyHRResponseDto>> GetAllAsync();

        Task<CompanyHRResponseDto?> GetByIdAsync(int id);

        Task<CompanyHRResponseDto?> GetByUserIdAsync(
            string userId);

        Task<IEnumerable<CompanyHRResponseDto>> GetByCompanyIdAsync(
            int companyId);
    }
}
