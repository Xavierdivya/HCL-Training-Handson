
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Repositories.Implementations;

public class AmenityRepository : IAmenityRepository
{
    private readonly ApplicationDbContext _context;

    public AmenityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Amenity>> GetAllAsync()
    {
        return await _context.Amenities.ToListAsync();
    }

    public async Task<Amenity?> GetByIdAsync(int amenityId)
    {
        return await _context.Amenities
            .FirstOrDefaultAsync(a => a.AmenityId == amenityId);
    }

    public async Task<List<Amenity>> GetByIdsAsync(List<int> amenityIds)
    {
        return await _context.Amenities
            .Where(a => amenityIds.Contains(a.AmenityId))
            .ToListAsync();
    }

    public async Task<Amenity> AddAsync(Amenity amenity)
    {
        await _context.Amenities.AddAsync(amenity);
        return amenity;
    }

    public Task<Amenity> UpdateAsync(Amenity amenity)
    {
        _context.Amenities.Update(amenity);
        return Task.FromResult(amenity);
    }

    public async Task<bool> DeleteAsync(int amenityId)
    {
        var amenity = await _context.Amenities
            .FirstOrDefaultAsync(a => a.AmenityId == amenityId);

        if (amenity == null)
            return false;

        _context.Amenities.Remove(amenity);
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
