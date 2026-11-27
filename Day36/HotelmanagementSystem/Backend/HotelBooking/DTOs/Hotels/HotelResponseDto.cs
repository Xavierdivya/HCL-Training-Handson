namespace HotelBooking.DTOs.Hotels;

public class HotelResponseDto
{
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int StarRating { get; set; }
    public string ContactNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public List<HotelImageResponseDto> Images { get; set; } = new();

    public List<string> Amenities { get; set; } = new();
}