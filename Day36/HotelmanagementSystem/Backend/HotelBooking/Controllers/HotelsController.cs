using HotelBooking.DTOs.Amenities;
using HotelBooking.DTOs.Hotels;
using HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace HotelBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    // GET: api/hotels
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(
        [FromQuery] HotelSearchRequestDto request)
    {
        var result = await _hotelService.GetAllAsync(request);
        return Ok(result);
    }

    // GET: api/hotels/5
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _hotelService.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // POST: api/hotels
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateHotelRequestDto request)
    {
        var result = await _hotelService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.HotelId },
            result);
    }

    // PUT: api/hotels/5
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id,
        UpdateHotelRequestDto request)
    {
        var result = await _hotelService.UpdateAsync(id, request);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // DELETE: api/hotels/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _hotelService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    // PUT: api/hotels/5/amenities
    [HttpPut("{id:int}/amenities")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAmenities(
        int id,
        UpdateHotelAmenitiesRequestDto request)
    {
        var result = await _hotelService.UpdateAmenitiesAsync(
            id, request.AmenityIds);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
