
using HotelBooking.Models;

namespace HotelBooking.Repositories.Interfaces;

public interface IAmenityRepository
{
    Task<IEnumerable<Amenity>> GetAllAsync();

    Task<Amenity?> GetByIdAsync(int amenityId);

    Task<List<Amenity>> GetByIdsAsync(List<int> amenityIds);

    Task<Amenity> AddAsync(Amenity amenity);

    Task<Amenity> UpdateAsync(Amenity amenity);

    Task<bool> DeleteAsync(int amenityId);

    Task SaveChangesAsync();
}
