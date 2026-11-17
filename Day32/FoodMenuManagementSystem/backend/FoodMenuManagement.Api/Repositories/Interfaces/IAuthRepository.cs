
using FoodMenuManagement.Api.Entities;

namespace FoodMenuManagement.Api.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<AppUser?> GetByEmailAsync(string email);

        Task AddAsync(AppUser user);

        Task SaveChangesAsync();
    }
}
