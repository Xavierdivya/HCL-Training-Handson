
using FoodMenuManagement.Api.DTOs.FoodItems;
using FoodMenuManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodMenuManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItemService _service;

        public FoodItemController(IFoodItemService service)
        {
            _service = service;
        }

        // GET: api/FoodItem
        // Admin, Staff and Customer
        [HttpGet]
        [Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> GetAll()
        {
            var foodItems = await _service.GetAllAsync();
            return Ok(foodItems);
        }

        // GET: api/FoodItem/1
        // Admin, Staff and Customer
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> GetById(int id)
        {
            var foodItem = await _service.GetByIdAsync(id);

            if (foodItem == null)
            {
                return NotFound(new
                {
                    message = "Food item not found."
                });
            }

            return Ok(foodItem);
        }

        // POST: api/FoodItem
        // Admin and Staff
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create(CreateFoodItemDto dto)
        {
            var foodItem = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = foodItem.FoodItemId },
                new
                {
                    message = "Food item created successfully.",
                    data = foodItem
                });
        }

        // PUT: api/FoodItem/1
        // Admin and Staff
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Update(
            int id,
            UpdateFoodItemDto dto)
        {
            var foodItem = await _service.UpdateAsync(id, dto);

            return Ok(new
            {
                message = "Food item updated successfully.",
                data = foodItem
            });
        }

        // DELETE: api/FoodItem/1
        // Admin only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new
            {
                message = "Food item deleted successfully."
            });
        }
    }
}
