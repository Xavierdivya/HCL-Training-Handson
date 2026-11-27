namespace HotelBooking.DTOs.RoomInventory;

public class RoomAvailabilityResponseDto
{
    public int RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public int AvailableRooms { get; set; }
    public List<RoomAvailabilityDateDto> DailyAvailability { get; set; } = new();
}

public class RoomAvailabilityDateDto
{
    public DateOnly Date { get; set; }
    public int AvailableRooms { get; set; }
}
