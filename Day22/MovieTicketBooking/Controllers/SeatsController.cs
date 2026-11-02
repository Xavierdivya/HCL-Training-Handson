using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Seat seat)
        {
            var result = await _seatService.AddAsync(seat);

            return CreatedAtAction(nameof(GetById),
                new { id = result.SeatId }, result);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulk(List<Seat> seats)
        {
            await _seatService.AddRangeAsync(seats);
            return Ok("Seats created successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _seatService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seat = await _seatService.GetByIdAsync(id);

            if (seat == null)
                return NotFound();

            return Ok(seat);
        }

        [HttpGet("screen/{screenId:int}")]
        public async Task<IActionResult> GetByScreenId(int screenId)
        {
            return Ok(await _seatService.GetByScreenIdAsync(screenId));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Seat seat)
        {
            if (id != seat.SeatId)
                return BadRequest("Seat ID mismatch.");

            if (await _seatService.GetByIdAsync(id) == null)
                return NotFound();

            await _seatService.UpdateAsync(seat);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _seatService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}