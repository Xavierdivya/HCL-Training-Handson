
using System.ComponentModel.DataAnnotations;
using HotelBooking.Enums;

namespace HotelBooking.DTOs.Bookings;

public class BookingSearchRequestDto
{
    public string? Search { get; set; }

    public int? HotelId { get; set; }

    public string? UserId { get; set; }

    public BookingStatus? Status { get; set; }

    public DateOnly? CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    [Range(1, 100)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}
