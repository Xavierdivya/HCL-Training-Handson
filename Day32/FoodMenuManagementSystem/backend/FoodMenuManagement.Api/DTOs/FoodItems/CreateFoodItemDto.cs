using System.ComponentModel.DataAnnotations;

namespace FoodMenuManagement.Api.DTOs.FoodItems
{
    public class CreateFoodItemDto
    {
        [Required]
        [MaxLength(100)]
        public string FoodName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(typeof(decimal), "0.01", "99999999.99")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}