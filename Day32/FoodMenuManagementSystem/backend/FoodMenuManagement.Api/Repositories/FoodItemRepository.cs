using FoodMenuManagement.Api.Data;
using FoodMenuManagement.Api.Entities;
using FoodMenuManagement.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodMenuManagement.Api.Repositories
{
    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly ApplicationDbContext _context;

        public FoodItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoodItem>> GetAllAsync()
        {
            return await _context.FoodItems
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FoodItem?> GetByIdAsync(int id)
        {
            return await _context.FoodItems
                .FirstOrDefaultAsync(f => f.FoodItemId == id);
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories
                .AnyAsync(c => c.CategoryId == categoryId);
        }

        public async Task AddAsync(FoodItem foodItem)
        {
            await _context.FoodItems.AddAsync(foodItem);
        }

        public void Delete(FoodItem foodItem)
        {
            _context.FoodItems.Remove(foodItem);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}