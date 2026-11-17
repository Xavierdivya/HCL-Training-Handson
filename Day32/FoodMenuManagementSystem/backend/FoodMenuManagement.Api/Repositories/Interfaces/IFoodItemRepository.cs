using FoodMenuManagement.Api.Entities;

namespace FoodMenuManagement.Api.Repositories.Interfaces
{
    public interface IFoodItemRepository
    {
        Task<List<FoodItem>> GetAllAsync();

        Task<FoodItem?> GetByIdAsync(int id);

        Task<bool> CategoryExistsAsync(int categoryId);

        Task AddAsync(FoodItem foodItem);

        void Delete(FoodItem foodItem);

        Task SaveChangesAsync();
    }
}