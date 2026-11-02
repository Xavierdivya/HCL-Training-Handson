using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> AddAsync(Booking booking);
        Task<Booking?> GetByIdAsync(int id);
        Task<Booking?> GetByReferenceAsync(string reference);
        Task UpdateAsync(Booking booking);

        // Create booking and reserve seats in one transaction
        Task<Booking> CreateBookingWithSeatsAsync(
            Booking booking,
            List<int> seatIds);

        // Cancel booking and release seats in one transaction
        Task CancelBookingAsync(int bookingId);
    }
}