using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.RoomTypes;

public class CreateRoomTypeRequestDto
{
    [Required]
    public int HotelId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Range(0.01, 1000000)]
    public decimal PricePerNight { get; set; }

    [Range(1, 20)]
    public int Capacity { get; set; }

    [Range(1, 10000)]
    public int TotalRooms { get; set; }

    public List<int> AmenityIds { get; set; } = new();
}
