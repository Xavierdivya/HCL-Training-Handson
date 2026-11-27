
using System.ComponentModel.DataAnnotations;
using HotelBooking.Enums;

namespace HotelBooking.DTOs.Bookings;

public class UpdateBookingStatusRequestDto
{
    [Required]
    [EnumDataType(typeof(BookingStatus))]
    public BookingStatus Status { get; set; }
}
