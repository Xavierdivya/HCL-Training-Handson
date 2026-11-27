
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Repositories.Implementations;

public class HotelRepository : IHotelRepository
{
    private readonly ApplicationDbContext _context;

    public HotelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Hotel>> GetAllAsync()
    {
        return await _context.Hotels
            .Include(h => h.HotelImages)
            .Include(h => h.HotelAmenities)
                .ThenInclude(ha => ha.Amenity)
            .ToListAsync();
    }

    public async Task<Hotel?> GetByIdAsync(int hotelId)
    {
        return await _context.Hotels
            .Include(h => h.HotelImages)
            .Include(h => h.HotelAmenities)
                .ThenInclude(ha => ha.Amenity)
            .FirstOrDefaultAsync(h => h.HotelId == hotelId);
    }

    public async Task<Hotel> AddAsync(Hotel hotel)
    {
        await _context.Hotels.AddAsync(hotel);
        return hotel;
    }

    public Task<Hotel> UpdateAsync(Hotel hotel)
    {
        _context.Hotels.Update(hotel);
        return Task.FromResult(hotel);
    }

    public async Task<bool> DeleteAsync(int hotelId)
    {
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.HotelId == hotelId);

        if (hotel == null)
            return false;

        _context.Hotels.Remove(hotel);
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
