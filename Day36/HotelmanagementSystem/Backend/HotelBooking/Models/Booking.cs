
using HotelBooking.Enums;

namespace HotelBooking.Models;

public class Booking
{
    public int BookingId { get; set; }

    public string BookingReference { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public int HotelId { get; set; }

    public DateOnly CheckInDate { get; set; }

    public DateOnly CheckOutDate { get; set; }

    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public BookingStatus BookingStatus { get; set; }
        = BookingStatus.Pending;

    // Navigation properties
    public ApplicationUser User { get; set; } = null!;

    public Hotel Hotel { get; set; } = null!;

    public ICollection<BookingItem> BookingItems { get; set; }
        = new List<BookingItem>();
}
