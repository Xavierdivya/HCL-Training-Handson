using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [Authorize(Roles = "Admin")]
    // POST: api/Movies
    [HttpPost]
    public async Task<IActionResult> CreateMovie(CreateMovieDto dto)
    {
        var movie = new Movie
        {
            Title = dto.Title,
            Description = dto.Description,
            DurationMinutes = dto.DurationMinutes,
            ReleaseDate = dto.ReleaseDate,
            Language = dto.Language,
            Genre = dto.Genre
        };

        var createdMovie = await _movieService.AddAsync(movie);

        var response = MapToResponse(createdMovie);

        return CreatedAtAction(
            nameof(GetMovieById),
            new { id = response.MovieId },
            response);
    }

    // GET: api/Movies
    [HttpGet]
    public async Task<IActionResult> GetAllMovies(
        int pageNumber = 1,
        int pageSize = 10,
        string? title = null)
    {
        var movies = await _movieService.GetAllAsync(
            pageNumber, pageSize, title);

        var response = movies.Select(MapToResponse).ToList();

        return Ok(response);
    }

    // GET: api/Movies/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);

        if (movie == null)
            return NotFound();

        return Ok(MapToResponse(movie));
    }


    [Authorize(Roles = "Admin")]
    // PUT: api/Movies/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(
        int id, UpdateMovieDto dto)
    {
        var existingMovie = await _movieService.GetByIdAsync(id);

        if (existingMovie == null)
            return NotFound();

        existingMovie.Title = dto.Title;
        existingMovie.Description = dto.Description;
        existingMovie.DurationMinutes = dto.DurationMinutes;
        existingMovie.ReleaseDate = dto.ReleaseDate;
        existingMovie.Language = dto.Language;
        existingMovie.Genre = dto.Genre;

        await _movieService.UpdateAsync(existingMovie);

        return NoContent();
    }


    [Authorize(Roles = "Admin")]
    // DELETE: api/Movies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var deleted = await _movieService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    private static MovieResponseDto MapToResponse(Movie movie)
    {
        return new MovieResponseDto
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Language = movie.Language,
            Genre = movie.Genre
        };
    }
}
