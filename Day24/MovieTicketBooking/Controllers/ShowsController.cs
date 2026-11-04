using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowsController : ControllerBase
{
    private readonly IShowService _showService;

    public ShowsController(IShowService showService)
    {
        _showService = showService;
    }


    [Authorize(Roles = "Admin")]
    // POST: api/Shows
    [HttpPost]
    public async Task<IActionResult> CreateShow(CreateShowDto dto)
    {
        var show = new Show
        {
            MovieId = dto.MovieId,
            ScreenId = dto.ScreenId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            TicketPrice = dto.TicketPrice
        };

        var createdShow = await _showService.AddAsync(show);

        var response = MapToResponse(createdShow);

        return CreatedAtAction(
            nameof(GetShowById),
            new { id = response.ShowId },
            response);
    }

    // GET: api/Shows
    [HttpGet]
    public async Task<IActionResult> GetAllShows()
    {
        var shows = await _showService.GetAllAsync();

        return Ok(shows.Select(MapToResponse).ToList());
    }

    // GET: api/Shows/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetShowById(int id)
    {
        var show = await _showService.GetByIdAsync(id);

        if (show == null)
            return NotFound();

        return Ok(MapToResponse(show));
    }

    // GET: api/Shows/movie/1
    [HttpGet("movie/{movieId}")]
    public async Task<IActionResult> GetShowsByMovie(int movieId)
    {
        var shows = await _showService.GetByMovieIdAsync(movieId);

        return Ok(shows.Select(MapToResponse).ToList());
    }

    // GET: api/Shows/screen/1
    [HttpGet("screen/{screenId}")]
    public async Task<IActionResult> GetShowsByScreen(int screenId)
    {
        var shows = await _showService.GetByScreenIdAsync(screenId);

        return Ok(shows.Select(MapToResponse).ToList());
    }

    // GET: api/Shows/date/2026-11-02
    [HttpGet("date/{date}")]
    public async Task<IActionResult> GetShowsByDate(DateTime date)
    {
        var shows = await _showService.GetByDateAsync(date);

        return Ok(shows.Select(MapToResponse).ToList());
    }


    [Authorize(Roles = "Admin")]
    // PUT: api/Shows/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShow(
        int id, UpdateShowDto dto)
    {
        var existingShow = await _showService.GetByIdAsync(id);

        if (existingShow == null)
            return NotFound();

        existingShow.MovieId = dto.MovieId;
        existingShow.ScreenId = dto.ScreenId;
        existingShow.StartTime = dto.StartTime;
        existingShow.EndTime = dto.EndTime;
        existingShow.TicketPrice = dto.TicketPrice;

        await _showService.UpdateAsync(existingShow);

        return NoContent();
    }


    [Authorize(Roles = "Admin")]
    // DELETE: api/Shows/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShow(int id)
    {
        await _showService.DeleteAsync(id);

        return NoContent();
    }

    private static ShowResponseDto MapToResponse(Show show)
    {
        return new ShowResponseDto
        {
            ShowId = show.ShowId,
            MovieId = show.MovieId,
            ScreenId = show.ScreenId,
            StartTime = show.StartTime,
            EndTime = show.EndTime,
            TicketPrice = show.TicketPrice
        };
    }
}
