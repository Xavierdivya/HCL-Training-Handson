namespace HotelBooking.DTOs.Hotels;

public class HotelImageResponseDto
{
    public int HotelImageId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}
