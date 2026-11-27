using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Amenities;

public class CreateAmenityRequestDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}