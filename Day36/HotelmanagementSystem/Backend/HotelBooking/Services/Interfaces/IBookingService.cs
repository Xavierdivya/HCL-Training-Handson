using HotelBooking.DTOs.Bookings;
using HotelBooking.Enums;

namespace HotelBooking.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponseDto> CreateAsync(
        string userId,
        CreateBookingRequestDto request);

    Task<List<BookingResponseDto>> GetMyBookingsAsync(string userId);

    Task<BookingResponseDto?> GetMyBookingByIdAsync(
        int bookingId,
        string userId);

    Task<bool> CancelAsync(int bookingId, string userId);

    Task<List<BookingResponseDto>> GetAllAsync(
        BookingSearchRequestDto request);

    Task<BookingResponseDto?> GetByIdAsync(int bookingId);

    Task<bool> UpdateStatusAsync(
        int bookingId,
        BookingStatus status);
}