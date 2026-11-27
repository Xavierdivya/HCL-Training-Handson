
using HotelBooking.DTOs.RoomTypes;

namespace HotelBooking.Services.Interfaces;

public interface IRoomTypeService
{
    Task<List<RoomTypeResponseDto>> GetByHotelAsync(int hotelId);

    Task<RoomTypeResponseDto?> GetByIdAsync(int id);

    Task<RoomTypeResponseDto> CreateAsync(
        CreateRoomTypeRequestDto request);

    Task<RoomTypeResponseDto?> UpdateAsync(
        int id,
        UpdateRoomTypeRequestDto request);

    Task<bool> DeleteAsync(int id);
}
