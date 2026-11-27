using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.RoomInventory;

public class SetRoomInventoryRequestDto
{
    [Required]
    public DateOnly InventoryDate { get; set; }

    [Range(0, 10000)]
    public int TotalRooms { get; set; }

    [Range(0, 10000)]
    public int AvailableRooms { get; set; }
}