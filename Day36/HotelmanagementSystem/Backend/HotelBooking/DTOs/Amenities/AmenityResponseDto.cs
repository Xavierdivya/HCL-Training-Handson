namespace HotelBooking.DTOs.Amenities;

public class AmenityResponseDto
{
    public int AmenityId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}