using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Authorization;


namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    private readonly IAuthorizationService _authorizationService;

    public BookingsController(
     IBookingService bookingService,
     IAuthorizationService authorizationService)
    {
        _bookingService = bookingService;
        _authorizationService = authorizationService;
    }

    // POST: api/Bookings
    [HttpPost]
    public async Task<IActionResult> CreateBooking(CreateBookingDto dto)
    {
        var booking = await _bookingService.CreateBookingAsync(
            dto.ShowId,
            dto.SeatIds,
            dto.CustomerName,
            dto.CustomerEmail,
            dto.CustomerPhone);

        var response = MapToResponse(booking, dto.SeatIds);

        return CreatedAtAction(
            nameof(GetBookingById),
            new { id = response.BookingId },
            response);
    }

    // GET: api/Bookings/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);

        if (booking == null)
            return NotFound();

        return Ok(MapToResponse(
            booking,
            booking.BookingSeats.Select(bs => bs.SeatId).ToList()));
    }

    // GET: api/Bookings/reference/ABC123
    [HttpGet("reference/{bookingReference}")]
    public async Task<IActionResult> GetBookingByReference(
        string bookingReference)
    {
        var booking = await _bookingService.GetByReferenceAsync(
            bookingReference);

        if (booking == null)
            return NotFound();

        return Ok(MapToResponse(
            booking,
            booking.BookingSeats.Select(bs => bs.SeatId).ToList()));
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        // Get booking
        var booking = await _bookingService.GetByIdAsync(id);

        if (booking == null)
            return NotFound();

        // Create authorization requirement
        var requirement = new OperationAuthorizationRequirement
        {
            Name = BookingOperations.Access
        };

        // Check booking ownership
        var authorizationResult =
            await _authorizationService.AuthorizeAsync(
                User,
                booking,
                requirement);

        if (!authorizationResult.Succeeded)
            return Forbid();

        // Cancel booking
        await _bookingService.CancelBookingAsync(id);

        return NoContent();
    }

    private static BookingResponseDto MapToResponse(
        Booking booking,
        List<int> seatIds)
    {
        return new BookingResponseDto
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            ShowId = booking.ShowId,
            CustomerName = booking.CustomerName,
            CustomerEmail = booking.CustomerEmail,
            BookingDate = booking.BookingDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            SeatIds = seatIds
        };
    }
}
