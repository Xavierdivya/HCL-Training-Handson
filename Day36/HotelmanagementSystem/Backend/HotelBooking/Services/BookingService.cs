using HotelBooking.DTOs.Bookings;
using HotelBooking.Enums;
using HotelBooking.Models;
using HotelBooking.Repositories.Interfaces;
using HotelBooking.Services.Interfaces;

namespace HotelBooking.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _repository;

    public BookingService(IBookingRepository repository)
    {
        _repository = repository;
    }

    public async Task<BookingResponseDto> CreateAsync(
        string userId,
        CreateBookingRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("Invalid customer.");

        if (request.Items == null || request.Items.Count == 0)
            throw new ArgumentException("At least one room type is required.");

        if (request.CheckInDate < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("Check-in date cannot be in the past.");

        if (request.CheckInDate >= request.CheckOutDate)
            throw new ArgumentException(
                "Check-out date must be after check-in date.");

        if (request.Items.Any(i => i.RoomTypeId <= 0 || i.Quantity < 1))
            throw new ArgumentException("Invalid room type or quantity.");

        if (request.Items.Select(i => i.RoomTypeId).Distinct().Count()
            != request.Items.Count)
        {
            throw new ArgumentException(
                "Duplicate room types are not allowed. Combine their quantities.");
        }

        var hotel = await _repository.GetHotelAsync(request.HotelId);

        if (hotel == null)
            throw new InvalidOperationException("Hotel not found.");

        var roomTypeIds = request.Items
            .Select(i => i.RoomTypeId)
            .ToList();

        var roomTypes = await _repository.GetRoomTypesAsync(
            request.HotelId,
            roomTypeIds);

        if (roomTypes.Count != roomTypeIds.Count)
            throw new InvalidOperationException(
                "One or more room types are invalid or inactive for this hotel.");

        var inventory = await _repository.GetInventoryAsync(
            roomTypeIds,
            request.CheckInDate,
            request.CheckOutDate);

        var bookedItems = await _repository.GetOverlappingBookingItemsAsync(
            roomTypeIds,
            request.CheckInDate,
            request.CheckOutDate);

        var numberOfNights =
            request.CheckOutDate.DayNumber - request.CheckInDate.DayNumber;

        var booking = new Booking
        {
            BookingReference = $"HB-{Guid.NewGuid():N}"[..13].ToUpperInvariant(),
            UserId = userId,
            HotelId = request.HotelId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            BookingDate = DateTime.UtcNow,
            BookingStatus = BookingStatus.Pending
        };

        foreach (var requestedItem in request.Items)
        {
            var roomType = roomTypes.First(
                r => r.RoomTypeId == requestedItem.RoomTypeId);

            var dailyInventory = inventory
                .Where(i => i.RoomTypeId == roomType.RoomTypeId)
                .ToList();

            if (dailyInventory.Count != numberOfNights)
            {
                throw new InvalidOperationException(
                    $"Complete inventory is not available for room type '{roomType.Name}'.");
            }

            for (var date = request.CheckInDate;
                 date < request.CheckOutDate;
                 date = date.AddDays(1))
            {
                var dayInventory = dailyInventory.FirstOrDefault(
                    i => i.InventoryDate == date);

                if (dayInventory == null)
                {
                    throw new InvalidOperationException(
                        $"Inventory is missing for {roomType.Name} on {date}.");
                }

                var alreadyBooked = bookedItems
                    .Where(item =>
                        item.RoomTypeId == roomType.RoomTypeId &&
                        item.Booking.CheckInDate <= date &&
                        item.Booking.CheckOutDate > date)
                    .Sum(item => item.Quantity);

                var availableRooms = Math.Max(
                    0,
                    dayInventory.AvailableRooms - alreadyBooked);

                if (requestedItem.Quantity > availableRooms)
                {
                    throw new InvalidOperationException(
                        $"Only {availableRooms} rooms are available for " +
                        $"{roomType.Name} on {date}.");
                }
            }

            var subTotal =
                roomType.PricePerNight *
                requestedItem.Quantity *
                numberOfNights;

            booking.TotalAmount += subTotal;

            booking.BookingItems.Add(new BookingItem
            {
                RoomTypeId = roomType.RoomTypeId,
                Quantity = requestedItem.Quantity,
                PricePerNight = roomType.PricePerNight,
                SubTotal = subTotal
            });
        }

        await _repository.AddAsync(booking);
        await _repository.SaveChangesAsync();

        var savedBooking = await _repository.GetByIdAsync(booking.BookingId);

        if (savedBooking == null)
            throw new InvalidOperationException(
                "Booking was created but could not be retrieved.");

        return MapToResponse(savedBooking);
    }

    public async Task<List<BookingResponseDto>> GetMyBookingsAsync(
        string userId)
    {
        var bookings = await _repository.GetByCustomerAsync(userId);

        return bookings.Select(MapToResponse).ToList();
    }

    public async Task<BookingResponseDto?> GetMyBookingByIdAsync(
        int bookingId,
        string userId)
    {
        var booking = await _repository.GetByIdForCustomerAsync(
            bookingId,
            userId);

        return booking == null ? null : MapToResponse(booking);
    }

    public async Task<bool> CancelAsync(int bookingId, string userId)
    {
        var booking = await _repository.GetByIdForCustomerAsync(
            bookingId,
            userId);

        if (booking == null)
            return false;

        if (booking.BookingStatus != BookingStatus.Pending &&
            booking.BookingStatus != BookingStatus.Confirmed)
        {
            return false;
        }

        booking.BookingStatus = BookingStatus.Cancelled;

        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<List<BookingResponseDto>> GetAllAsync(
        BookingSearchRequestDto request)
    {
        var bookings = await _repository.SearchAsync(request);

        return bookings.Select(MapToResponse).ToList();
    }

    public async Task<BookingResponseDto?> GetByIdAsync(int bookingId)
    {
        var booking = await _repository.GetByIdAsync(bookingId);

        return booking == null ? null : MapToResponse(booking);
    }

    public async Task<bool> UpdateStatusAsync(
        int bookingId,
        BookingStatus status)
    {
        if (!Enum.IsDefined(typeof(BookingStatus), status))
            throw new ArgumentException("Invalid booking status.");

        var booking = await _repository.GetByIdAsync(bookingId);

        if (booking == null)
            return false;

        booking.BookingStatus = status;

        await _repository.SaveChangesAsync();

        return true;
    }

    private static BookingResponseDto MapToResponse(Booking booking)
    {
        return new BookingResponseDto
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            HotelId = booking.HotelId,
            HotelName = booking.Hotel?.Name ?? string.Empty,
            UserId = booking.UserId,
            CustomerName = booking.User?.FullName ?? string.Empty,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            BookingDate = booking.BookingDate,
            TotalAmount = booking.TotalAmount,
            BookingStatus = booking.BookingStatus.ToString(),

            Items = booking.BookingItems.Select(item =>
                new BookingItemResponseDto
                {
                    BookingItemId = item.BookingItemId,
                    RoomTypeId = item.RoomTypeId,
                    RoomTypeName = item.RoomType?.Name ?? string.Empty,
                    Quantity = item.Quantity,
                    PricePerNight = item.PricePerNight,
                    SubTotal = item.SubTotal
                }).ToList()
        };
    }
}