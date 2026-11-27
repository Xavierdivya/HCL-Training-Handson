
using HotelBooking.Models;

namespace HotelBooking.Repositories.Interfaces;

public interface IHotelRepository
{
    // Get all hotels
    Task<IEnumerable<Hotel>> GetAllAsync();

    // Get hotel by ID
    Task<Hotel?> GetByIdAsync(int hotelId);

    // Add a new hotel
    Task<Hotel> AddAsync(Hotel hotel);

    // Update an existing hotel
    Task<Hotel> UpdateAsync(Hotel hotel);

    // Delete a hotel
    Task<bool> DeleteAsync(int hotelId);

    // Save database changes
    Task SaveChangesAsync();
}
