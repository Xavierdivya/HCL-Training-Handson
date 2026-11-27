
namespace HotelBooking.Models;

public class RoomInventory
{
    public int InventoryId { get; set; }

    public int RoomTypeId { get; set; }

    public DateOnly InventoryDate { get; set; }

    public int TotalRooms { get; set; }

    public int AvailableRooms { get; set; }

    // Navigation property
    public RoomType RoomType { get; set; } = null!;
}
