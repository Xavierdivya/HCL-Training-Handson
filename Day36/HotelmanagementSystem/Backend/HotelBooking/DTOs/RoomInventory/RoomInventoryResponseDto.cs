namespace HotelBooking.DTOs.RoomInventory;

public class RoomInventoryResponseDto
{
    public int InventoryId { get; set; }
    public int RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public DateOnly InventoryDate { get; set; }
    public int TotalRooms { get; set; }
    public int AvailableRooms { get; set; }
}