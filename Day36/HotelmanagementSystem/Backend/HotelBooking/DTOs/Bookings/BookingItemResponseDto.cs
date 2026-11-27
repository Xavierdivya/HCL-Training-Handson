namespace HotelBooking.DTOs.Bookings;

public class BookingItemResponseDto
{
    public int BookingItemId { get; set; }
    public int RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal PricePerNight { get; set; }
    public decimal SubTotal { get; set; }
}