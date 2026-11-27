
using HotelBooking.DTOs.RoomInventory;
using HotelBooking.Repositories.Interfaces;
using HotelBooking.Services.Interfaces;

namespace HotelBooking.Services;

public class RoomInventoryService : IRoomInventoryService
{
    private readonly IRoomInventoryRepository _repository;

    public RoomInventoryService(IRoomInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RoomAvailabilityResponseDto>> CheckAvailabilityAsync(
        RoomAvailabilityRequestDto request)
    {
        if (request.CheckInDate >= request.CheckOutDate)
            throw new ArgumentException(
                "Check-out date must be after check-in date.");

        var roomTypes = await _repository.GetAvailabilityCandidatesAsync(
            request.HotelId,
            request.Guests,
            request.CheckInDate,
            request.CheckOutDate);

        var results = new List<RoomAvailabilityResponseDto>();

        foreach (var roomType in roomTypes)
        {
            var bookedItems = await _repository.GetBookedRoomItemsAsync(
                roomType.RoomTypeId,
                request.CheckInDate,
                request.CheckOutDate);

            var dailyAvailability = new List<RoomAvailabilityDateDto>();
            var hasCompleteInventory = true;

            for (var date = request.CheckInDate;
                 date < request.CheckOutDate;
                 date = date.AddDays(1))
            {
                var inventory = roomType.RoomInventories
                    .FirstOrDefault(i => i.InventoryDate == date);

                if (inventory == null)
                {
                    hasCompleteInventory = false;
                    break;
                }

                var bookedRooms = bookedItems
                    .Where(item =>
                        item.Booking.CheckInDate <= date &&
                        item.Booking.CheckOutDate > date)
                    .Sum(item => item.Quantity);

                var availableRooms = Math.Max(
                    0,
                    inventory.AvailableRooms - bookedRooms);

                dailyAvailability.Add(new RoomAvailabilityDateDto
                {
                    Date = date,
                    AvailableRooms = availableRooms
                });
            }

            if (!hasCompleteInventory || dailyAvailability.Count == 0)
                continue;

            var minimumAvailable = dailyAvailability.Min(
                d => d.AvailableRooms);

            if (minimumAvailable <= 0)
                continue;

            results.Add(new RoomAvailabilityResponseDto
            {
                RoomTypeId = roomType.RoomTypeId,
                RoomTypeName = roomType.Name,
                PricePerNight = roomType.PricePerNight,
                Capacity = roomType.Capacity,
                AvailableRooms = minimumAvailable,
                DailyAvailability = dailyAvailability
            });
        }

        return results;
    }

    public async Task<List<RoomInventoryResponseDto>> GetInventoryAsync(
        int roomTypeId,
        DateOnly fromDate,
        DateOnly toDate)
    {
        if (fromDate > toDate)
            throw new ArgumentException("Invalid inventory date range.");

        var inventory = await _repository.GetInventoryAsync(
            roomTypeId, fromDate, toDate);

        return inventory.Select(i => new RoomInventoryResponseDto
        {
            InventoryId = i.InventoryId,
            RoomTypeId = i.RoomTypeId,
            RoomTypeName = i.RoomType.Name,
            InventoryDate = i.InventoryDate,
            TotalRooms = i.TotalRooms,
            AvailableRooms = i.AvailableRooms
        }).ToList();
    }

    public async Task<bool> SetInventoryAsync(
        int roomTypeId,
        SetRoomInventoryRequestDto request)
    {
        if (request.AvailableRooms > request.TotalRooms)
            throw new ArgumentException(
                "Available rooms cannot exceed total rooms.");

        var roomType = await _repository.GetRoomTypeAsync(roomTypeId);

        if (roomType == null)
            return false;

        if (request.TotalRooms > roomType.TotalRooms)
            throw new ArgumentException(
                "Inventory cannot exceed the room type's total rooms.");

        var inventory = await _repository.GetByRoomTypeAndDateAsync(
            roomTypeId,
            request.InventoryDate);

        if (inventory == null)
        {
            inventory = new Models.RoomInventory
            {
                RoomTypeId = roomTypeId,
                InventoryDate = request.InventoryDate,
                TotalRooms = request.TotalRooms,
                AvailableRooms = request.AvailableRooms
            };

            await _repository.AddAsync(inventory);
        }
        else
        {
            inventory.TotalRooms = request.TotalRooms;
            inventory.AvailableRooms = request.AvailableRooms;
        }

        await _repository.SaveChangesAsync();

        return true;
    }
}
