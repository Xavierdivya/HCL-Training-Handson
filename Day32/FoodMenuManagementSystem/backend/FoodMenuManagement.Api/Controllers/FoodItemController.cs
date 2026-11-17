
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

        // GET: api/FoodItem
        [HttpGet]
        public async Task<IActionResult> GetAllFoodItems()
        {
            var foodItems = await _context.FoodItems
                .Select(foodItem => new FoodItemResponseDto
                {
                    FoodItemId = foodItem.FoodItemId,
                    FoodName = foodItem.FoodName,
                    Description = foodItem.Description,
                    Price = foodItem.Price,
                    ImageUrl = foodItem.ImageUrl,
                    IsAvailable = foodItem.IsAvailable,
                    CategoryId = foodItem.CategoryId
                })
                .ToListAsync();

            return Ok(foodItems);
        }

        // GET: api/FoodItem/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodItemById(int id)
        {
            var foodItem = await _context.FoodItems
                .Where(item => item.FoodItemId == id)
                .Select(item => new FoodItemResponseDto
                {
                    FoodItemId = item.FoodItemId,
                    FoodName = item.FoodName,
                    Description = item.Description,
                    Price = item.Price,
                    ImageUrl = item.ImageUrl,
                    IsAvailable = item.IsAvailable,
                    CategoryId = item.CategoryId
                })
                .FirstOrDefaultAsync();

            if (foodItem == null)
            {
                return NotFound("Food item not found.");
            }

            return Ok(foodItem);
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

        // PUT: api/FoodItem/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFoodItem(
            int id,
            UpdateFoodItemDto foodItemDto)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return NotFound("Food item not found.");
            }

            var category = await _context.Categories
                .FindAsync(foodItemDto.CategoryId);

            if (category == null)
            {
                return BadRequest("Invalid CategoryId.");
            }

            foodItem.FoodName = foodItemDto.FoodName;
            foodItem.Description = foodItemDto.Description;
            foodItem.Price = foodItemDto.Price;
            foodItem.ImageUrl = foodItemDto.ImageUrl;
            foodItem.IsAvailable = foodItemDto.IsAvailable;
            foodItem.CategoryId = foodItemDto.CategoryId;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Food item updated successfully.",
                foodItem = new FoodItemResponseDto
                {
                    FoodItemId = foodItem.FoodItemId,
                    FoodName = foodItem.FoodName,
                    Description = foodItem.Description,
                    Price = foodItem.Price,
                    ImageUrl = foodItem.ImageUrl,
                    IsAvailable = foodItem.IsAvailable,
                    CategoryId = foodItem.CategoryId
                }
            });
        }

        // DELETE: api/FoodItem/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodItem(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return NotFound("Food item not found.");
            }

            _context.FoodItems.Remove(foodItem);

            await _context.SaveChangesAsync();

            return Ok("Food item deleted successfully.");
        }
    }
}
