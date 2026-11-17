
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodMenuManagement.Api.Entities
{
    public class FoodItem
    {
        public int FoodItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FoodName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation property
        public Category? Category { get; set; }
    }
}
