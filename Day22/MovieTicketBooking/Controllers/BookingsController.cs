using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            int showId,
            List<int> seatIds,
            string customerName,
            string customerEmail,
            string customerPhone)
        {
            var booking = await _bookingService.CreateBookingAsync(
                showId,
                seatIds,
                customerName,
                customerEmail,
                customerPhone);

            return CreatedAtAction(
                nameof(GetById),
                new { id = booking.BookingId },
                booking);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpGet("reference/{reference}")]
        public async Task<IActionResult> GetByReference(string reference)
        {
            var booking = await _bookingService.GetByReferenceAsync(reference);

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _bookingService.CancelBookingAsync(id);
            return Ok("Booking cancelled successfully.");
        }
    }
}