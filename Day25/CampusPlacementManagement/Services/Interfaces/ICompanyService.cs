
using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.DTOs.CompanyHRs;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponseDto> RegisterAsync(
            CompanyRegistrationDto dto);

        Task<IEnumerable<CompanyResponseDto>> GetAllAsync();

        Task<CompanyResponseDto?> GetByIdAsync(int id);

        Task<IEnumerable<CompanyResponseDto>> GetPendingAsync();

        Task<bool> ApproveAsync(int id);

        Task<bool> RejectAsync(int id);

        Task<bool> SuspendAsync(int id);

        Task<CompanyHRResponseDto> RegisterHRAsync(
            string currentUserId,
            CompanyHRRegistrationDto dto);
    }
}
