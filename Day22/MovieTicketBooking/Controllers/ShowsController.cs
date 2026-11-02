using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Show show)
        {
            var result = await _showService.AddAsync(show);

            return CreatedAtAction(nameof(GetById),
                new { id = result.ShowId }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _showService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var show = await _showService.GetByIdAsync(id);

            if (show == null)
                return NotFound();

            return Ok(show);
        }

        [HttpGet("movie/{movieId:int}")]
        public async Task<IActionResult> GetByMovieId(int movieId)
        {
            return Ok(await _showService.GetByMovieIdAsync(movieId));
        }

        [HttpGet("screen/{screenId:int}")]
        public async Task<IActionResult> GetByScreenId(int screenId)
        {
            return Ok(await _showService.GetByScreenIdAsync(screenId));
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            return Ok(await _showService.GetByDateAsync(date));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Show show)
        {
            if (id != show.ShowId)
                return BadRequest("Show ID mismatch.");

            if (await _showService.GetByIdAsync(id) == null)
                return NotFound();

            await _showService.UpdateAsync(show);
            return NoContent();
        }
    }
}