
namespace HotelBooking.Models;

public class Hotel
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

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<HotelImage> HotelImages { get; set; }
        = new List<HotelImage>();

    public ICollection<HotelAmenity> HotelAmenities { get; set; }
        = new List<HotelAmenity>();

    public ICollection<RoomType> RoomTypes { get; set; }
        = new List<RoomType>();

    public ICollection<Booking> Bookings { get; set; }
        = new List<Booking>();
}
