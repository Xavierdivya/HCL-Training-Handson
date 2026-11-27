using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Hotels;

public class UpdateHotelRequestDto
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    [Range(1, 5)]
    public int StarRating { get; set; }

    [Required]
    public string ContactNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public List<string> ImageUrls { get; set; } = new();

    public List<int> AmenityIds { get; set; } = new();
}