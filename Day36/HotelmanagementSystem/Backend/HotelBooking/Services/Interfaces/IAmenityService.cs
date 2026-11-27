
using HotelBooking.DTOs.Amenities;

namespace HotelBooking.Services.Interfaces;

public interface IAmenityService
{
    Task<IEnumerable<AmenityResponseDto>> GetAllAsync();

    Task<AmenityResponseDto?> GetByIdAsync(int amenityId);

    Task<AmenityResponseDto> CreateAsync(CreateAmenityRequestDto request);

    Task<AmenityResponseDto?> UpdateAsync(
        int amenityId,
        UpdateAmenityRequestDto request);

    Task<bool> DeleteAsync(int amenityId);
}
