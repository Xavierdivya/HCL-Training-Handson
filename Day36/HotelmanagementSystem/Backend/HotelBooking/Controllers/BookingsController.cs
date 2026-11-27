
using System.Security.Claims;
using HotelBooking.DTOs.Bookings;
using HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // Customer creates a booking
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Create(CreateBookingRequestDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            var result = await _bookingService.CreateAsync(userId, request);

            return CreatedAtAction(
                nameof(GetMyBookingById),
                new { id = result.BookingId },
                result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // Customer views their own bookings
    [HttpGet("my-bookings")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _bookingService.GetMyBookingsAsync(userId);

        return Ok(result);
    }

    // Customer views one of their own bookings
    [HttpGet("my-bookings/{id:int}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMyBookingById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _bookingService.GetMyBookingByIdAsync(id, userId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // Customer cancels their own booking
    [HttpPut("{id:int}/cancel")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _bookingService.CancelAsync(id, userId);

        if (!result)
            return NotFound();

        return NoContent();
    }

    // Admin views all bookings
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(
        [FromQuery] BookingSearchRequestDto request)
    {
        var result = await _bookingService.GetAllAsync(request);

        return Ok(result);
    }

    // Admin views a particular booking
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _bookingService.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // Admin updates booking status
    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateBookingStatusRequestDto request)
    {
        var result = await _bookingService.UpdateStatusAsync(
            id, request.Status);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
