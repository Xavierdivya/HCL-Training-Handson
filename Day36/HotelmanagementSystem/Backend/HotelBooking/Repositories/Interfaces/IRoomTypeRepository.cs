
using HotelBooking.Models;

namespace HotelBooking.Repositories.Interfaces;

public interface IRoomTypeRepository
{
    Task<List<RoomType>> GetByHotelAsync(int hotelId);

    Task<RoomType?> GetByIdAsync(int roomTypeId);

    Task<bool> HotelExistsAsync(int hotelId);

    Task<List<Amenity>> GetAmenitiesByIdsAsync(
        IReadOnlyCollection<int> amenityIds);

    Task AddAsync(RoomType roomType);

    Task DeleteAsync(RoomType roomType);

    Task SaveChangesAsync();
}
