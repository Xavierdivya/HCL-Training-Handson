using FoodMenuManagement.Api.DTOs.Auth;
using System.Threading.Tasks;

namespace FoodMenuManagement.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}