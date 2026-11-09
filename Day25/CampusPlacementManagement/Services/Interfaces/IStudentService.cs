using CampusPlacementManagement.DTOs.Students;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentResponseDto> RegisterAsync(
            string placementOfficerUserId,
            StudentRegistrationDto dto);

        Task<IEnumerable<StudentResponseDto>> GetAllAsync();

        Task<StudentResponseDto?> GetByIdAsync(int id);

        Task<IEnumerable<StudentResponseDto>> GetByCollegeIdAsync(
            int collegeId);

        Task<StudentResponseDto?> GetByUserIdAsync(string userId);
    }
}