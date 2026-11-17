using FoodMenuManagement.Api.DTOs.FoodItems;
using FoodMenuManagement.Api.Entities;
using FoodMenuManagement.Api.Repositories.Interfaces;
using FoodMenuManagement.Api.Services.Interfaces;

namespace FoodMenuManagement.Api.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IFoodItemRepository _repository;

        public FoodItemService(IFoodItemRepository repository)
        {
            _repository = repository;
        }

        // GET ALL
        public async Task<List<FoodItemResponseDto>> GetAllAsync()
        {
            var foodItems = await _repository.GetAllAsync();

            return foodItems.Select(MapToDto).ToList();
        }

        // GET BY ID
        public async Task<FoodItemResponseDto?> GetByIdAsync(int id)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            if (foodItem == null)
                return null;

            return MapToDto(foodItem);
        }

        // CREATE
        public async Task<FoodItemResponseDto> CreateAsync(
            CreateFoodItemDto dto)
        {
            if (!await _repository.CategoryExistsAsync(dto.CategoryId))
            {
                throw new ArgumentException("Invalid CategoryId.");
            }

            var foodItem = new FoodItem
            {
                FoodName = dto.FoodName,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                IsAvailable = dto.IsAvailable,
                CategoryId = dto.CategoryId
            };

            await _repository.AddAsync(foodItem);
            await _repository.SaveChangesAsync();

            return MapToDto(foodItem);
        }

        // UPDATE
        public async Task<FoodItemResponseDto> UpdateAsync(
            int id,
            UpdateFoodItemDto dto)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            if (foodItem == null)
            {
                throw new KeyNotFoundException("Food item not found.");
            }

            if (!await _repository.CategoryExistsAsync(dto.CategoryId))
            {
                throw new ArgumentException("Invalid CategoryId.");
            }

            foodItem.FoodName = dto.FoodName;
            foodItem.Description = dto.Description;
            foodItem.Price = dto.Price;
            foodItem.ImageUrl = dto.ImageUrl;
            foodItem.IsAvailable = dto.IsAvailable;
            foodItem.CategoryId = dto.CategoryId;

            await _repository.SaveChangesAsync();

            return MapToDto(foodItem);
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            if (foodItem == null)
            {
                throw new KeyNotFoundException("Food item not found.");
            }

            _repository.Delete(foodItem);

            await _repository.SaveChangesAsync();
        }

        // ENTITY TO RESPONSE DTO MAPPING
        private static FoodItemResponseDto MapToDto(FoodItem foodItem)
        {
            return new FoodItemResponseDto
            {
                FoodItemId = foodItem.FoodItemId,
                FoodName = foodItem.FoodName,
                Description = foodItem.Description,
                Price = foodItem.Price,
                ImageUrl = foodItem.ImageUrl,
                IsAvailable = foodItem.IsAvailable,
                CategoryId = foodItem.CategoryId
            };
        }
    }
}