
using CampusPlacementManagement.DTOs.PlacementOfficers;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IPlacementOfficerService
    {
        Task<IEnumerable<PlacementOfficerResponseDto>> GetAllAsync();

        Task<PlacementOfficerResponseDto?> GetByIdAsync(int id);

        Task<PlacementOfficerResponseDto?> GetByUserIdAsync(string userId);
    }
}
