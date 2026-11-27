using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.RoomInventory;

public class RoomAvailabilityRequestDto
{
    [Required]
    public int HotelId { get; set; }

    [Required]
    public DateOnly CheckInDate { get; set; }

    [Required]
    public DateOnly CheckOutDate { get; set; }

    [Range(1, 20)]
    public int Guests { get; set; } = 1;
}