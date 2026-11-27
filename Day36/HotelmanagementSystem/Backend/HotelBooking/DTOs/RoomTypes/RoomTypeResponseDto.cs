namespace HotelBooking.DTOs.RoomTypes;

public class RoomTypeResponseDto
{
    public int RoomTypeId { get; set; }

    public int HotelId { get; set; }

    public string HotelName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }

    public int TotalRooms { get; set; }

    public bool IsActive { get; set; }

    public List<string> Amenities { get; set; } = new();
}