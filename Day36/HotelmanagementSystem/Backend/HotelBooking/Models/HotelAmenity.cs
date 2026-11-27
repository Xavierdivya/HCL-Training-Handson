
namespace HotelBooking.Models;

public class HotelAmenity
{
    public int HotelId { get; set; }

    public int AmenityId { get; set; }

    // Navigation properties
    public Hotel Hotel { get; set; } = null!;

    public Amenity Amenity { get; set; } = null!;
}
