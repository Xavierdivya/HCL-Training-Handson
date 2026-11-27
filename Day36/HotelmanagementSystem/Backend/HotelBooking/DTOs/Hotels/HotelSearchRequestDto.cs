namespace HotelBooking.DTOs.Hotels;

public class HotelSearchRequestDto
{
    public string? SearchTerm { get; set; }
    public string? City { get; set; }
    public int? StarRating { get; set; }

    public DateOnly? CheckInDate { get; set; }
    public DateOnly? CheckOutDate { get; set; }

    public int? Guests { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}