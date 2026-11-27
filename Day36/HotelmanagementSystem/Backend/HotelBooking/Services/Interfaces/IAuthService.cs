using HotelBooking.DTOs.Auth;

namespace HotelBooking.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

    Task<CurrentUserResponseDto?> GetCurrentUserAsync(string userId);
}