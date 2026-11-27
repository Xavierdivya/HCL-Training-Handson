
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Repositories;

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly ApplicationDbContext _context;

    public RoomTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoomType>> GetByHotelAsync(int hotelId)
    {
        return await _context.RoomTypes
            .AsNoTracking()
            .Where(r => r.HotelId == hotelId)
            .Include(r => r.Hotel)
            .Include(r => r.RoomTypeAmenities)
                .ThenInclude(a => a.Amenity)
            .ToListAsync();
    }

    public async Task<RoomType?> GetByIdAsync(int roomTypeId)
    {
        return await _context.RoomTypes
            .Include(r => r.Hotel)
            .Include(r => r.RoomTypeAmenities)
                .ThenInclude(a => a.Amenity)
            .FirstOrDefaultAsync(r => r.RoomTypeId == roomTypeId);
    }

    public Task<bool> HotelExistsAsync(int hotelId)
    {
        return _context.Hotels
            .AnyAsync(h => h.HotelId == hotelId);
    }

    public async Task<List<Amenity>> GetAmenitiesByIdsAsync(
        IReadOnlyCollection<int> amenityIds)
    {
        if (amenityIds.Count == 0)
            return new List<Amenity>();

        return await _context.Amenities
            .Where(a => amenityIds.Contains(a.AmenityId))
            .ToListAsync();
    }

    public async Task AddAsync(RoomType roomType)
    {
        await _context.RoomTypes.AddAsync(roomType);
    }

    public Task DeleteAsync(RoomType roomType)
    {
        _context.RoomTypes.Remove(roomType);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
