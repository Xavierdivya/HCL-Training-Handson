
namespace HotelBooking.Models;

public class Amenity
{
    public int AmenityId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<HotelAmenity> HotelAmenities { get; set; }
        = new List<HotelAmenity>();

    public ICollection<RoomTypeAmenity> RoomTypeAmenities { get; set; }
        = new List<RoomTypeAmenity>();
}
