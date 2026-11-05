using CampusPlacementManagement.DTOs.Auth;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterCollegeRepresentativeAsync(
            CollegeRepresentativeRegistrationDto registrationDto);

        Task<LoginResponseDto> LoginAsync(
            LoginRequestDto loginDto);

        Task<UserResponseDto?> GetCurrentUserAsync(string userId);
    }
}