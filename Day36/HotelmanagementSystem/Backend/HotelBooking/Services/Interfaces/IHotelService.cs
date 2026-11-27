
using HotelBooking.DTOs.Hotels;

namespace HotelBooking.Services.Interfaces;

public interface IHotelService
{
    Task<IEnumerable<HotelResponseDto>> GetAllAsync(
        HotelSearchRequestDto request);

    Task<HotelResponseDto?> GetByIdAsync(int hotelId);

    Task<HotelResponseDto> CreateAsync(
        CreateHotelRequestDto request);

    Task<HotelResponseDto?> UpdateAsync(
        int hotelId,
        UpdateHotelRequestDto request);

    Task<bool> DeleteAsync(int hotelId);

    Task<bool> UpdateAmenitiesAsync(
        int hotelId,
        List<int> amenityIds);
}
