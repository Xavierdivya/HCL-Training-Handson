using FoodMenuManagement.Api.Entities;
using FoodMenuManagement.Api.Repositories.Interfaces;
using FoodMenuManagement.Api.Services.Interfaces;

namespace FoodMenuManagement.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        // GET ALL
        public async Task<List<Category>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET BY ID
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // CREATE
        public async Task<Category> CreateAsync(Category category)
        {
            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            return category;
        }

        // UPDATE
        public async Task<Category> UpdateAsync(
            int id,
            Category updatedCategory)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            category.CategoryName = updatedCategory.CategoryName;
            category.Description = updatedCategory.Description;
            category.IsActive = updatedCategory.IsActive;

            await _repository.SaveChangesAsync();

            return category;
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            _repository.Delete(category);
            await _repository.SaveChangesAsync();
        }
    }
}