using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenService _screenService;

        public ScreensController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Screen screen)
        {
            var result = await _screenService.AddAsync(screen);

            return CreatedAtAction(nameof(GetById),
                new { id = result.ScreenId }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _screenService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var screen = await _screenService.GetByIdAsync(id);

            if (screen == null)
                return NotFound();

            return Ok(screen);
        }

        [HttpGet("theatre/{theatreId:int}")]
        public async Task<IActionResult> GetByTheatreId(int theatreId)
        {
            return Ok(await _screenService.GetByTheatreIdAsync(theatreId));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Screen screen)
        {
            if (id != screen.ScreenId)
                return BadRequest("Screen ID mismatch.");

            if (await _screenService.GetByIdAsync(id) == null)
                return NotFound();

            await _screenService.UpdateAsync(screen);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _screenService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}