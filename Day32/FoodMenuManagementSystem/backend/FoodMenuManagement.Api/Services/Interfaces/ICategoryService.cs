using FoodMenuManagement.Api.Entities;

namespace FoodMenuManagement.Api.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<Category> CreateAsync(Category category);

        Task<Category> UpdateAsync(int id, Category updatedCategory);

        Task DeleteAsync(int id);
    }
}