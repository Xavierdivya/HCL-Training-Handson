
using CampusPlacementManagement.DTOs.Colleges;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICollegeService
    {
        Task<CollegeResponseDto> RegisterAsync(
            CollegeRegistrationDto registrationDto);

        Task<IEnumerable<CollegeResponseDto>> GetAllAsync();

        Task<CollegeResponseDto?> GetByIdAsync(int collegeId);
    }
}
