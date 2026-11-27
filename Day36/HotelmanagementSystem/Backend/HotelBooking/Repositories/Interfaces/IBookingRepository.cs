
using HotelBooking.DTOs.Bookings;
using HotelBooking.Enums;
using HotelBooking.Models;

namespace HotelBooking.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<Hotel?> GetHotelAsync(int hotelId);

    Task<List<RoomType>> GetRoomTypesAsync(
        int hotelId,
        IReadOnlyCollection<int> roomTypeIds);

    Task<List<RoomInventory>> GetInventoryAsync(
        IReadOnlyCollection<int> roomTypeIds,
        DateOnly checkInDate,
        DateOnly checkOutDate);

    Task<List<BookingItem>> GetOverlappingBookingItemsAsync(
        IReadOnlyCollection<int> roomTypeIds,
        DateOnly checkInDate,
        DateOnly checkOutDate);

    Task AddAsync(Booking booking);

    Task<Booking?> GetByIdAsync(int bookingId);

    Task<Booking?> GetByIdForCustomerAsync(
        int bookingId,
        string userId);

    Task<List<Booking>> GetByCustomerAsync(string userId);

    Task<List<Booking>> SearchAsync(BookingSearchRequestDto request);

    Task<int> CountAsync(BookingSearchRequestDto request);

    Task SaveChangesAsync();
}
