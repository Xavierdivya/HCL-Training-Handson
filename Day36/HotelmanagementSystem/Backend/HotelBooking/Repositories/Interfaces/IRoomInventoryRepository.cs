
using HotelBooking.Models;

namespace HotelBooking.Repositories.Interfaces;

public interface IRoomInventoryRepository
{
    Task<RoomType?> GetRoomTypeAsync(int roomTypeId);

    Task<List<RoomInventory>> GetInventoryAsync(
        int roomTypeId,
        DateOnly fromDate,
        DateOnly toDate);

    Task<RoomInventory?> GetByRoomTypeAndDateAsync(
        int roomTypeId,
        DateOnly inventoryDate);

    Task<List<RoomType>> GetAvailabilityCandidatesAsync(
        int hotelId,
        int guests,
        DateOnly checkInDate,
        DateOnly checkOutDate);

    Task<List<BookingItem>> GetBookedRoomItemsAsync(
        int roomTypeId,
        DateOnly checkInDate,
        DateOnly checkOutDate);

    Task AddAsync(RoomInventory inventory);

    Task SaveChangesAsync();
}
