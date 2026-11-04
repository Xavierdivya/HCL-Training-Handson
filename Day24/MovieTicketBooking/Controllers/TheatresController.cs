using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TheatresController : ControllerBase
{
    private readonly ITheatreService _theatreService;

    public TheatresController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }


    [Authorize(Roles = "Admin")]
    // POST: api/Theatres
    [HttpPost]
    public async Task<IActionResult> CreateTheatre(CreateTheatreDto dto)
    {
        var theatre = new Theatre
        {
            Name = dto.Name,
            Location = dto.Location
        };

        var createdTheatre = await _theatreService.AddAsync(theatre);

        var response = MapToResponse(createdTheatre);

        return CreatedAtAction(
            nameof(GetTheatreById),
            new { id = response.TheatreId },
            response);
    }

    // GET: api/Theatres
    [HttpGet]
    public async Task<IActionResult> GetAllTheatres()
    {
        var theatres = await _theatreService.GetAllAsync();

        var response = theatres.Select(MapToResponse).ToList();

        return Ok(response);
    }

    // GET: api/Theatres/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTheatreById(int id)
    {
        var theatre = await _theatreService.GetByIdAsync(id);

        if (theatre == null)
            return NotFound();

        return Ok(MapToResponse(theatre));
    }


    [Authorize(Roles = "Admin")]
    // PUT: api/Theatres/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTheatre(
        int id, UpdateTheatreDto dto)
    {
        var existingTheatre = await _theatreService.GetByIdAsync(id);

        if (existingTheatre == null)
            return NotFound();

        existingTheatre.Name = dto.Name;
        existingTheatre.Location = dto.Location;

        await _theatreService.UpdateAsync(existingTheatre);

        return NoContent();
    }


    [Authorize(Roles = "Admin")]
    // DELETE: api/Theatres/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTheatre(int id)
    {
        var deleted = await _theatreService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    private static TheatreResponseDto MapToResponse(Theatre theatre)
    {
        return new TheatreResponseDto
        {
            TheatreId = theatre.TheatreId,
            Name = theatre.Name,
            Location = theatre.Location
        };
    }
}
