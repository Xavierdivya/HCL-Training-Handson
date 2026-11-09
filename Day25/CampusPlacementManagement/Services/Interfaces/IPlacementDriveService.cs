
using CampusPlacementManagement.DTOs.PlacementDrives;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IPlacementDriveService
    {
        Task<PlacementDriveResponseDto> CreateAsync(
            string placementOfficerUserId,
            PlacementDriveCreateDto dto);

        Task<PlacementDriveResponseDto> UpdateAsync(
            int driveId,
            string placementOfficerUserId,
            PlacementDriveUpdateDto dto);

        Task<IEnumerable<PlacementDriveResponseDto>> GetAllAsync();

        Task<IEnumerable<PlacementDriveResponseDto>> GetByCollegeIdAsync(
            int collegeId);

        Task<IEnumerable<PlacementDriveResponseDto>> GetMyDrivesAsync(
            string placementOfficerUserId);

        Task<IEnumerable<PlacementDriveResponseDto>> GetOpenDrivesAsync();

        Task<PlacementDriveResponseDto?> GetByIdAsync(int id);
    }
}
