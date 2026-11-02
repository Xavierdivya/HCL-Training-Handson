using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            var result = await _movieService.AddAsync(movie);
            return CreatedAtAction(nameof(GetById),
                new { id = result.MovieId }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            int pageNumber = 1,
            int pageSize = 10,
            string? title = null)
        {
            var movies = await _movieService.GetAllAsync(
                pageNumber, pageSize, title);

            return Ok(movies);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Movie movie)
        {
            if (id != movie.MovieId)
                return BadRequest("Movie ID mismatch.");

            if (await _movieService.GetByIdAsync(id) == null)
                return NotFound();

            await _movieService.UpdateAsync(movie);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _movieService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}