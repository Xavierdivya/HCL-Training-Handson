using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScreensController : ControllerBase
{
    private readonly IScreenService _screenService;

    public ScreensController(IScreenService screenService)
    {
        _screenService = screenService;
    }


    [Authorize(Roles = "Admin")]
    // POST: api/Screens
    [HttpPost]
    public async Task<IActionResult> CreateScreen(CreateScreenDto dto)
    {
        var screen = new Screen
        {
            TheatreId = dto.TheatreId,
            ScreenName = dto.ScreenName
        };

        var createdScreen = await _screenService.AddAsync(screen);

        var response = MapToResponse(createdScreen);

        return CreatedAtAction(
            nameof(GetScreenById),
            new { id = response.ScreenId },
            response);
    }

    // GET: api/Screens
    [HttpGet]
    public async Task<IActionResult> GetAllScreens()
    {
        var screens = await _screenService.GetAllAsync();

        var response = screens.Select(MapToResponse).ToList();

        return Ok(response);
    }

    // GET: api/Screens/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetScreenById(int id)
    {
        var screen = await _screenService.GetByIdAsync(id);

        if (screen == null)
            return NotFound();

        return Ok(MapToResponse(screen));
    }

    // GET: api/Screens/theatre/5
    [HttpGet("theatre/{theatreId}")]
    public async Task<IActionResult> GetScreensByTheatre(int theatreId)
    {
        var screens = await _screenService.GetByTheatreIdAsync(theatreId);

        var response = screens.Select(MapToResponse).ToList();

        return Ok(response);
    }


    [Authorize(Roles = "Admin")]
    // PUT: api/Screens/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateScreen(
        int id, UpdateScreenDto dto)
    {
        var existingScreen = await _screenService.GetByIdAsync(id);

        if (existingScreen == null)
            return NotFound();

        existingScreen.TheatreId = dto.TheatreId;
        existingScreen.ScreenName = dto.ScreenName;

        await _screenService.UpdateAsync(existingScreen);

        return NoContent();
    }


    [Authorize(Roles = "Admin")]
    // DELETE: api/Screens/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteScreen(int id)
    {
        var deleted = await _screenService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    private static ScreenResponseDto MapToResponse(Screen screen)
    {
        return new ScreenResponseDto
        {
            ScreenId = screen.ScreenId,
            TheatreId = screen.TheatreId,
            ScreenName = screen.ScreenName
        };
    }
}
