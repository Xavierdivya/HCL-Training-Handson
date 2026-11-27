using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Bookings;

public class CreateBookingRequestDto
{
    [Required]
    public int HotelId { get; set; }

    [Required]
    public DateOnly CheckInDate { get; set; }

    [Required]
    public DateOnly CheckOutDate { get; set; }

    [Required]
    [MinLength(1)]
    public List<CreateBookingItemRequestDto> Items { get; set; } = new();
}

public class CreateBookingItemRequestDto
{
    [Required]
    public int RoomTypeId { get; set; }

    [Range(1, 20)]
    public int Quantity { get; set; }
}