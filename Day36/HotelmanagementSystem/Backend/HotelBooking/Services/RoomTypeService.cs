
using HotelBooking.DTOs.RoomTypes;
using HotelBooking.Models;
using HotelBooking.Repositories.Interfaces;
using HotelBooking.Services.Interfaces;

namespace HotelBooking.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomTypeRepository _repository;

    public RoomTypeService(IRoomTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RoomTypeResponseDto>> GetByHotelAsync(int hotelId)
    {
        var roomTypes = await _repository.GetByHotelAsync(hotelId);

        return roomTypes.Select(MapToResponse).ToList();
    }

    public async Task<RoomTypeResponseDto?> GetByIdAsync(int id)
    {
        var roomType = await _repository.GetByIdAsync(id);

        return roomType == null ? null : MapToResponse(roomType);
    }

    public async Task<RoomTypeResponseDto> CreateAsync(
        CreateRoomTypeRequestDto request)
    {
        if (!await _repository.HotelExistsAsync(request.HotelId))
            throw new InvalidOperationException("Hotel not found.");

        var amenities = await GetValidatedAmenitiesAsync(request.AmenityIds);

        var roomType = new RoomType
        {
            HotelId = request.HotelId,
            Name = request.Name.Trim(),
            Description = request.Description ?? string.Empty,
            PricePerNight = request.PricePerNight,
            Capacity = request.Capacity,
            TotalRooms = request.TotalRooms,
            IsActive = true,
            RoomTypeAmenities = amenities.Select(a => new RoomTypeAmenity
            {
                AmenityId = a.AmenityId,
                Amenity = a
            }).ToList()
        };

        await _repository.AddAsync(roomType);
        await _repository.SaveChangesAsync();

        var created = await _repository.GetByIdAsync(roomType.RoomTypeId);

        return created == null
            ? throw new InvalidOperationException("Room type could not be retrieved.")
            : MapToResponse(created);
    }

    public async Task<RoomTypeResponseDto?> UpdateAsync(
        int id,
        UpdateRoomTypeRequestDto request)
    {
        var roomType = await _repository.GetByIdAsync(id);

        if (roomType == null)
            return null;

        var amenities = await GetValidatedAmenitiesAsync(request.AmenityIds);

        roomType.Name = request.Name.Trim();
        roomType.Description = request.Description ?? string.Empty;
        roomType.PricePerNight = request.PricePerNight;
        roomType.Capacity = request.Capacity;
        roomType.TotalRooms = request.TotalRooms;
        roomType.IsActive = request.IsActive;

        roomType.RoomTypeAmenities.Clear();

        foreach (var amenity in amenities)
        {
            roomType.RoomTypeAmenities.Add(new RoomTypeAmenity
            {
                RoomTypeId = roomType.RoomTypeId,
                AmenityId = amenity.AmenityId
            });
        }

        await _repository.SaveChangesAsync();

        return MapToResponse(roomType);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var roomType = await _repository.GetByIdAsync(id);

        if (roomType == null)
            return false;

        await _repository.DeleteAsync(roomType);
        await _repository.SaveChangesAsync();

        return true;
    }

    private async Task<List<Amenity>> GetValidatedAmenitiesAsync(
        List<int> amenityIds)
    {
        var ids = amenityIds.Distinct().ToList();

        var amenities = await _repository.GetAmenitiesByIdsAsync(ids);

        if (amenities.Count != ids.Count)
            throw new InvalidOperationException(
                "One or more selected amenities do not exist.");

        return amenities;
    }

    private static RoomTypeResponseDto MapToResponse(RoomType roomType)
    {
        return new RoomTypeResponseDto
        {
            RoomTypeId = roomType.RoomTypeId,
            HotelId = roomType.HotelId,
            HotelName = roomType.Hotel?.Name ?? string.Empty,
            Name = roomType.Name,
            Description = roomType.Description,
            PricePerNight = roomType.PricePerNight,
            Capacity = roomType.Capacity,
            TotalRooms = roomType.TotalRooms,
            IsActive = roomType.IsActive,
            Amenities = roomType.RoomTypeAmenities
                .Select(a => a.Amenity?.Name ?? string.Empty)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList()
        };
    }
}
