
namespace HotelBooking.Models;

public class RoomType
{
    public int RoomTypeId { get; set; }

    public int HotelId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }

    public int TotalRooms { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Hotel Hotel { get; set; } = null!;

    public ICollection<RoomTypeAmenity> RoomTypeAmenities { get; set; }
        = new List<RoomTypeAmenity>();

    public ICollection<RoomInventory> RoomInventories { get; set; }
        = new List<RoomInventory>();

    public ICollection<BookingItem> BookingItems { get; set; }
        = new List<BookingItem>();
}
