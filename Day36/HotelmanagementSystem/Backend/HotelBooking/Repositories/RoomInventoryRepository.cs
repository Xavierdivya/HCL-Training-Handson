
using HotelBooking.Data;
using HotelBooking.Enums;
using HotelBooking.Models;
using HotelBooking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Repositories;

public class RoomInventoryRepository : IRoomInventoryRepository
{
    private readonly ApplicationDbContext _context;

    public RoomInventoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<RoomType?> GetRoomTypeAsync(int roomTypeId)
    {
        return _context.RoomTypes
            .FirstOrDefaultAsync(r => r.RoomTypeId == roomTypeId);
    }

    public async Task<List<RoomInventory>> GetInventoryAsync(
        int roomTypeId,
        DateOnly fromDate,
        DateOnly toDate)
    {
        return await _context.RoomInventories
            .AsNoTracking()
            .Include(i => i.RoomType)
            .Where(i =>
                i.RoomTypeId == roomTypeId &&
                i.InventoryDate >= fromDate &&
                i.InventoryDate <= toDate)
            .OrderBy(i => i.InventoryDate)
            .ToListAsync();
    }

    public Task<RoomInventory?> GetByRoomTypeAndDateAsync(
        int roomTypeId,
        DateOnly inventoryDate)
    {
        return _context.RoomInventories
            .FirstOrDefaultAsync(i =>
                i.RoomTypeId == roomTypeId &&
                i.InventoryDate == inventoryDate);
    }

    public async Task<List<RoomType>> GetAvailabilityCandidatesAsync(
        int hotelId,
        int guests,
        DateOnly checkInDate,
        DateOnly checkOutDate)
    {
        return await _context.RoomTypes
            .AsNoTracking()
            .Where(r =>
                r.HotelId == hotelId &&
                r.IsActive &&
                r.Capacity >= guests)
            .Include(r => r.RoomInventories.Where(i =>
                i.InventoryDate >= checkInDate &&
                i.InventoryDate < checkOutDate))
            .ToListAsync();
    }

    public async Task<List<BookingItem>> GetBookedRoomItemsAsync(
        int roomTypeId,
        DateOnly checkInDate,
        DateOnly checkOutDate)
    {
        return await _context.BookingItems
            .AsNoTracking()
            .Include(item => item.Booking)
            .Where(item =>
                item.RoomTypeId == roomTypeId &&
                (item.Booking.BookingStatus == BookingStatus.Pending ||
                 item.Booking.BookingStatus == BookingStatus.Confirmed) &&
                item.Booking.CheckInDate < checkOutDate &&
                item.Booking.CheckOutDate > checkInDate)
            .ToListAsync();
    }

    public async Task AddAsync(RoomInventory inventory)
    {
        await _context.RoomInventories.AddAsync(inventory);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
