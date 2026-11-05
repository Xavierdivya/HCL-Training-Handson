using CampusPlacementManagement.DTOs.Colleges;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICollegeService
    {
        // College registration and retrieval
        Task<CollegeResponseDto> RegisterAsync(
            CollegeRegistrationDto registrationDto);

        Task<IEnumerable<CollegeResponseDto>> GetAllAsync();

        Task<CollegeResponseDto?> GetByIdAsync(int collegeId);

        // College approval workflow
        Task<IEnumerable<CollegeResponseDto>> GetPendingAsync();

        Task<CollegeResponseDto?> ApproveAsync(int collegeId);

        Task<CollegeResponseDto?> RejectAsync(int collegeId);

        Task<CollegeResponseDto?> SuspendAsync(int collegeId);
    }
}