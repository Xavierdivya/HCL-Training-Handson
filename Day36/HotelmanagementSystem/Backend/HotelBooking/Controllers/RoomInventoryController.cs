
using HotelBooking.DTOs.RoomInventory;
using HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomInventoryController : ControllerBase
{
    private readonly IRoomInventoryService _roomInventoryService;

    public RoomInventoryController(
        IRoomInventoryService roomInventoryService)
    {
        _roomInventoryService = roomInventoryService;
    }

    // Customers can check available rooms
    [HttpGet("availability")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckAvailability(
        [FromQuery] RoomAvailabilityRequestDto request)
    {
        var result = await _roomInventoryService.CheckAvailabilityAsync(request);
        return Ok(result);
    }

    // Admin can view inventory for a room type
    [HttpGet("roomtype/{roomTypeId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetInventory(
        int roomTypeId,
        [FromQuery] DateOnly fromDate,
        [FromQuery] DateOnly toDate)
    {
        var result = await _roomInventoryService.GetInventoryAsync(
            roomTypeId, fromDate, toDate);

        return Ok(result);
    }

    // Admin can set inventory for a particular date
    [HttpPut("roomtype/{roomTypeId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetInventory(
        int roomTypeId,
        SetRoomInventoryRequestDto request)
    {
        var result = await _roomInventoryService.SetInventoryAsync(
            roomTypeId, request);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
