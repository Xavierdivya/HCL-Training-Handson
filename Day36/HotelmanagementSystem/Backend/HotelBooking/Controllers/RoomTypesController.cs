
using HotelBooking.DTOs.RoomTypes;
using HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomTypesController : ControllerBase
{
    private readonly IRoomTypeService _roomTypeService;

    public RoomTypesController(IRoomTypeService roomTypeService)
    {
        _roomTypeService = roomTypeService;
    }

    [HttpGet("hotel/{hotelId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByHotel(int hotelId)
    {
        var result = await _roomTypeService.GetByHotelAsync(hotelId);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _roomTypeService.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateRoomTypeRequestDto request)
    {
        var result = await _roomTypeService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.RoomTypeId },
            result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id,
        UpdateRoomTypeRequestDto request)
    {
        var result = await _roomTypeService.UpdateAsync(id, request);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _roomTypeService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
