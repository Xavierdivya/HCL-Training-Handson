
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatsController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatsController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    // POST: api/Seats
    [HttpPost]
    public async Task<IActionResult> CreateSeat(CreateSeatDto dto)
    {
        var seat = new Seat
        {
            ScreenId = dto.ScreenId,
            RowName = dto.RowName,
            SeatNumber = dto.SeatNumber,
            SeatType = dto.SeatType
        };

        var createdSeat = await _seatService.AddAsync(seat);

        return CreatedAtAction(
            nameof(GetSeatById),
            new { id = createdSeat.SeatId },
            MapToResponse(createdSeat));
    }

    // POST: api/Seats/bulk
    [HttpPost("bulk")]
    public async Task<IActionResult> CreateSeatsBulk(
        BulkCreateSeatsDto dto)
    {
        var seats = dto.Seats.Select(s => new Seat
        {
            ScreenId = s.ScreenId,
            RowName = s.RowName,
            SeatNumber = s.SeatNumber,
            SeatType = s.SeatType
        }).ToList();

        await _seatService.AddRangeAsync(seats);

        var response = seats.Select(MapToResponse).ToList();

        return Ok(response);
    }

    // GET: api/Seats
    [HttpGet]
    public async Task<IActionResult> GetAllSeats()
    {
        var seats = await _seatService.GetAllAsync();

        return Ok(seats.Select(MapToResponse).ToList());
    }

    // GET: api/Seats/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSeatById(int id)
    {
        var seat = await _seatService.GetByIdAsync(id);

        if (seat == null)
            return NotFound();

        return Ok(MapToResponse(seat));
    }

    // GET: api/Seats/screen/5
    [HttpGet("screen/{screenId}")]
    public async Task<IActionResult> GetSeatsByScreen(int screenId)
    {
        var seats = await _seatService.GetByScreenIdAsync(screenId);

        return Ok(seats.Select(MapToResponse).ToList());
    }

    // PUT: api/Seats/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSeat(
        int id, CreateSeatDto dto)
    {
        var existingSeat = await _seatService.GetByIdAsync(id);

        if (existingSeat == null)
            return NotFound();

        existingSeat.ScreenId = dto.ScreenId;
        existingSeat.RowName = dto.RowName;
        existingSeat.SeatNumber = dto.SeatNumber;
        existingSeat.SeatType = dto.SeatType;

        await _seatService.UpdateAsync(existingSeat);

        return NoContent();
    }

    // DELETE: api/Seats/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSeat(int id)
    {
        var deleted = await _seatService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    private static SeatResponseDto MapToResponse(Seat seat)
    {
        return new SeatResponseDto
        {
            SeatId = seat.SeatId,
            ScreenId = seat.ScreenId,
            RowName = seat.RowName,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType
        };
    }
}
