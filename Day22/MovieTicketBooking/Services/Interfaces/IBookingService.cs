
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(
            int showId,
            List<int> seatIds,
            string customerName,
            string customerEmail,
            string customerPhone);

        Task<Booking?> GetByIdAsync(int id);

        Task<Booking?> GetByReferenceAsync(string bookingReference);

        Task CancelBookingAsync(int bookingId);
    }
}
