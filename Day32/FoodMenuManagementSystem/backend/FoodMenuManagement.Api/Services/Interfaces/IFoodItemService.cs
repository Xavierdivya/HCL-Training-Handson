
using FoodMenuManagement.Api.DTOs.FoodItems;

namespace FoodMenuManagement.Api.Services.Interfaces
{
    public interface IFoodItemService
    {
        Task<List<FoodItemResponseDto>> GetAllAsync();

        Task<FoodItemResponseDto?> GetByIdAsync(int id);

        Task<FoodItemResponseDto> CreateAsync(
            CreateFoodItemDto dto);

        Task<FoodItemResponseDto> UpdateAsync(
            int id,
            UpdateFoodItemDto dto);

        Task DeleteAsync(int id);
    }
}
