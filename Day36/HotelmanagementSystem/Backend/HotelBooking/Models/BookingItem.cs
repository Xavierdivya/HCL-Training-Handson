
namespace HotelBooking.Models;

public class BookingItem
{
    public int BookingItemId { get; set; }

    public int BookingId { get; set; }

    public int RoomTypeId { get; set; }

    public int Quantity { get; set; }

    public decimal PricePerNight { get; set; }

    public decimal SubTotal { get; set; }

    // Navigation properties
    public Booking Booking { get; set; } = null!;

    public RoomType RoomType { get; set; } = null!;
}

