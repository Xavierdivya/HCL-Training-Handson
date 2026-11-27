
using HotelBooking.DTOs.Hotels;
using HotelBooking.Models;
using HotelBooking.Repositories.Interfaces;
using HotelBooking.Services.Interfaces;


namespace HotelBooking.Services.Implementations;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IAmenityRepository _amenityRepository;

    public HotelService(
        IHotelRepository hotelRepository,
        IAmenityRepository amenityRepository)
    {
        _hotelRepository = hotelRepository;
        _amenityRepository = amenityRepository;
    }

    public async Task<IEnumerable<HotelResponseDto>> GetAllAsync(
        HotelSearchRequestDto request)
    {
        var hotels = await _hotelRepository.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            hotels = hotels.Where(h =>
                h.City.Contains(
                    request.City,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            hotels = hotels.Where(h =>
                h.Name.Contains(
                    request.SearchTerm,
                    StringComparison.OrdinalIgnoreCase) ||
                h.Description.Contains(
                    request.SearchTerm,
                    StringComparison.OrdinalIgnoreCase) ||
                h.City.Contains(
                    request.SearchTerm,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (request.StarRating.HasValue)
        {
            hotels = hotels.Where(h =>
                h.StarRating == request.StarRating.Value);
        }

        return hotels
            .Where(h => h.IsActive)
            .Select(MapToResponse);
    }

    public async Task<HotelResponseDto?> GetByIdAsync(int hotelId)
    {
        var hotel = await _hotelRepository.GetByIdAsync(hotelId);

        if (hotel == null || !hotel.IsActive)
            return null;

        return MapToResponse(hotel);
    }

    public async Task<HotelResponseDto> CreateAsync(
        CreateHotelRequestDto request)
    {
        var hotel = new Hotel
        {
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            City = request.City,
            State = request.State,
            Country = request.Country,
            StarRating = request.StarRating,
            ContactNumber = request.ContactNumber,
            IsActive = true
        };

        await _hotelRepository.AddAsync(hotel);
        await _hotelRepository.SaveChangesAsync();

        return MapToResponse(hotel);
    }

    public async Task<HotelResponseDto?> UpdateAsync(
        int hotelId,
        UpdateHotelRequestDto request)
    {
        var hotel = await _hotelRepository.GetByIdAsync(hotelId);

        if (hotel == null)
            return null;

        hotel.Name = request.Name;
        hotel.Description = request.Description;
        hotel.Address = request.Address;
        hotel.City = request.City;
        hotel.State = request.State;
        hotel.Country = request.Country;
        hotel.StarRating = request.StarRating;
        hotel.ContactNumber = request.ContactNumber;

        await _hotelRepository.UpdateAsync(hotel);
        await _hotelRepository.SaveChangesAsync();

        return MapToResponse(hotel);
    }

    public async Task<bool> DeleteAsync(int hotelId)
    {
        var deleted = await _hotelRepository.DeleteAsync(hotelId);

        if (!deleted)
            return false;

        await _hotelRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAmenitiesAsync(
        int hotelId,
        List<int> amenityIds)
    {
        var hotel = await _hotelRepository.GetByIdAsync(hotelId);

        if (hotel == null)
            return false;

        var uniqueAmenityIds = amenityIds.Distinct().ToList();

        var amenities = await _amenityRepository.GetByIdsAsync(
            uniqueAmenityIds);

        if (amenities.Count != uniqueAmenityIds.Count)
            return false;

        hotel.HotelAmenities.Clear();

        foreach (var amenity in amenities)
        {
            hotel.HotelAmenities.Add(new HotelAmenity
            {
                HotelId = hotelId,
                AmenityId = amenity.AmenityId
            });
        }

        await _hotelRepository.SaveChangesAsync();

        return true;
    }

    private static HotelResponseDto MapToResponse(Hotel hotel)
    {
        return new HotelResponseDto
        {
            HotelId = hotel.HotelId,
            Name = hotel.Name,
            Description = hotel.Description,
            Address = hotel.Address,
            City = hotel.City,
            State = hotel.State,
            Country = hotel.Country,
            StarRating = hotel.StarRating,
            ContactNumber = hotel.ContactNumber,
            IsActive = hotel.IsActive,

            // DTO property is Images, not HotelImages.
            Images = hotel.HotelImages.Select(image =>
                new HotelImageResponseDto
                {
                    HotelImageId = image.HotelImageId,
                    ImageUrl = image.ImageUrl,
                    IsPrimary = image.IsPrimary
                }).ToList(),

            // DTO property is List<string>, not List<AmenityResponseDto>.
            Amenities = hotel.HotelAmenities
                .Where(ha => ha.Amenity != null)
                .Select(ha => ha.Amenity.Name)
                .ToList()
        };
    }
}
