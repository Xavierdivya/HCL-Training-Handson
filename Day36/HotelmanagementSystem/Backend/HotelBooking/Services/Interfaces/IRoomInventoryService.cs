
using HotelBooking.DTOs.RoomInventory;

namespace HotelBooking.Services.Interfaces;

public interface IRoomInventoryService
{
    Task<List<RoomAvailabilityResponseDto>> CheckAvailabilityAsync(
        RoomAvailabilityRequestDto request);

    Task<List<RoomInventoryResponseDto>> GetInventoryAsync(
        int roomTypeId,
        DateOnly fromDate,
        DateOnly toDate);

    Task<bool> SetInventoryAsync(
        int roomTypeId,
        SetRoomInventoryRequestDto request);
}
