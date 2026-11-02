using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheatresController : ControllerBase
    {
        private readonly ITheatreService _theatreService;

        public TheatresController(ITheatreService theatreService)
        {
            _theatreService = theatreService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Theatre theatre)
        {
            var result = await _theatreService.AddAsync(theatre);

            return CreatedAtAction(nameof(GetById),
                new { id = result.TheatreId }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _theatreService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var theatre = await _theatreService.GetByIdAsync(id);

            if (theatre == null)
                return NotFound();

            return Ok(theatre);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Theatre theatre)
        {
            if (id != theatre.TheatreId)
                return BadRequest("Theatre ID mismatch.");

            if (await _theatreService.GetByIdAsync(id) == null)
                return NotFound();

            await _theatreService.UpdateAsync(theatre);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _theatreService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}