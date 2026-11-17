
using FoodMenuManagement.Api.Data;
using FoodMenuManagement.Api.DTOs.FoodItems;
using FoodMenuManagement.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodMenuManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/FoodItem
        [HttpPost]
        public async Task<IActionResult> CreateFoodItem(
            CreateFoodItemDto foodItemDto)
        {
            // Check whether the category exists
            var category = await _context.Categories
                .FindAsync(foodItemDto.CategoryId);

            if (category == null)
            {
                return BadRequest("Invalid CategoryId. Category does not exist.");
            }

            // Convert DTO into Entity
            var foodItem = new FoodItem
            {
                FoodName = foodItemDto.FoodName,
                Description = foodItemDto.Description,
                Price = foodItemDto.Price,
                ImageUrl = foodItemDto.ImageUrl,
                IsAvailable = foodItemDto.IsAvailable,
                CategoryId = foodItemDto.CategoryId
            };

            // Add food item
            _context.FoodItems.Add(foodItem);

            // Save into SQL Server
            await _context.SaveChangesAsync();

            var response = new FoodItemResponseDto
            {
                FoodItemId = foodItem.FoodItemId,
                FoodName = foodItem.FoodName,
                Description = foodItem.Description,
                Price = foodItem.Price,
                ImageUrl = foodItem.ImageUrl,
                IsAvailable = foodItem.IsAvailable,
                CategoryId = foodItem.CategoryId
            };

            return StatusCode(201, new
            {
                message = "Food item created successfully.",
                foodItem = response
            });
        }
    }
}
