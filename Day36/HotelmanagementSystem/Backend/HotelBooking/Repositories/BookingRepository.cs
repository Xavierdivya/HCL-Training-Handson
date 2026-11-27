
using HotelBooking.Data;
using HotelBooking.DTOs.Bookings;
using HotelBooking.Enums;
using HotelBooking.Models;
using HotelBooking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Hotel?> GetHotelAsync(int hotelId)
    {
        return _context.Hotels
            .FirstOrDefaultAsync(h => h.HotelId == hotelId);
    }

    public async Task<List<RoomType>> GetRoomTypesAsync(
        int hotelId,
        IReadOnlyCollection<int> roomTypeIds)
    {
        return await _context.RoomTypes
            .Where(r => r.HotelId == hotelId &&
                        r.IsActive &&
                        roomTypeIds.Contains(r.RoomTypeId))
            .ToListAsync();
    }

    public async Task<List<RoomInventory>> GetInventoryAsync(
        IReadOnlyCollection<int> roomTypeIds,
        DateOnly checkInDate,
        DateOnly checkOutDate)
    {
        return await _context.RoomInventories
            .Where(i => roomTypeIds.Contains(i.RoomTypeId) &&
                        i.InventoryDate >= checkInDate &&
                        i.InventoryDate < checkOutDate)
            .ToListAsync();
    }

    public async Task<List<BookingItem>> GetOverlappingBookingItemsAsync(
        IReadOnlyCollection<int> roomTypeIds,
        DateOnly checkInDate,
        DateOnly checkOutDate)
    {
        return await _context.BookingItems
            .AsNoTracking()
            .Include(i => i.Booking)
            .Where(i =>
                roomTypeIds.Contains(i.RoomTypeId) &&
                (i.Booking.BookingStatus == BookingStatus.Pending ||
                 i.Booking.BookingStatus == BookingStatus.Confirmed) &&
                i.Booking.CheckInDate < checkOutDate &&
                i.Booking.CheckOutDate > checkInDate)
            .ToListAsync();
    }

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
    }

    public Task<Booking?> GetByIdAsync(int bookingId)
    {
        return _context.Bookings
            .Include(b => b.Hotel)
            .Include(b => b.User)
            .Include(b => b.BookingItems)
                .ThenInclude(i => i.RoomType)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    public Task<Booking?> GetByIdForCustomerAsync(
    int bookingId,
    string userId)
    {
        return _context.Bookings
            .Include(b => b.Hotel)
            .Include(b => b.User)
            .Include(b => b.BookingItems)
                .ThenInclude(i => i.RoomType)
            .FirstOrDefaultAsync(b =>
                b.BookingId == bookingId &&
                b.UserId == userId);
    }

    public async Task<List<Booking>> GetByCustomerAsync(string userId)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Include(b => b.Hotel)
            .Include(b => b.BookingItems)
                .ThenInclude(i => i.RoomType)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    public async Task<List<Booking>> SearchAsync(
        BookingSearchRequestDto request)
    {
        return await ApplyFilters(_context.Bookings.AsNoTracking(), request)
            .Include(b => b.Hotel)
            .Include(b => b.User)
            .Include(b => b.BookingItems)
                .ThenInclude(i => i.RoomType)
            .OrderByDescending(b => b.BookingDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
    }

    public Task<int> CountAsync(BookingSearchRequestDto request)
    {
        return ApplyFilters(_context.Bookings.AsNoTracking(), request)
            .CountAsync();
    }

    private static IQueryable<Booking> ApplyFilters(
        IQueryable<Booking> query,
        BookingSearchRequestDto request)
    {
        if (request.HotelId.HasValue)
            query = query.Where(b => b.HotelId == request.HotelId);

        if (!string.IsNullOrWhiteSpace(request.UserId))
            query = query.Where(b => b.UserId == request.UserId);

        if (request.Status.HasValue)
            query = query.Where(b => b.BookingStatus == request.Status);

        if (request.CheckInDate.HasValue)
            query = query.Where(b =>
                b.CheckInDate >= request.CheckInDate.Value);

        if (request.CheckOutDate.HasValue)
            query = query.Where(b =>
                b.CheckOutDate <= request.CheckOutDate.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(b =>
                b.BookingReference.Contains(search) ||
                b.User.Email!.Contains(search) ||
                b.User.UserName!.Contains(search));
        }

        return query;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
