
using FoodMenuManagement.Api.Entities;
using FoodMenuManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodMenuManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // GET: api/Category
        // Admin, Staff and Customer
        [HttpGet]
        [Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/Category/1
        // Admin, Staff and Customer
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _service.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            return Ok(category);
        }

        // POST: api/Category
        // Admin and Staff
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            var createdCategory = await _service.CreateAsync(category);

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = createdCategory.CategoryId },
                createdCategory);
        }

        // PUT: api/Category/1
        // Admin and Staff
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            Category updatedCategory)
        {
            var category = await _service.UpdateAsync(id, updatedCategory);
            return Ok(category);
        }

        // DELETE: api/Category/1
        // Admin only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Category deleted successfully.");
        }
    }
}
