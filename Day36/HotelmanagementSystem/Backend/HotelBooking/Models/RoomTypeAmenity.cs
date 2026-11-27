
namespace HotelBooking.Models;

public class RoomTypeAmenity
{
    public int RoomTypeId { get; set; }

    public int AmenityId { get; set; }

    // Navigation properties
    public RoomType RoomType { get; set; } = null!;

    public Amenity Amenity { get; set; } = null!;
}
