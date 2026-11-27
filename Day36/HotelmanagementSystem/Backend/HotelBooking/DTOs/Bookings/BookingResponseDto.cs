namespace HotelBooking.DTOs.Bookings;

public class BookingResponseDto
{
    public int BookingId { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;

    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public DateTime BookingDate { get; set; }

    public decimal TotalAmount { get; set; }
    public string BookingStatus { get; set; } = string.Empty;

    public List<BookingItemResponseDto> Items { get; set; } = new();
}