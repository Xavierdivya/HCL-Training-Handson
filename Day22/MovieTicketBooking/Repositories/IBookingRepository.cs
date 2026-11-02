
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> AddAsync(Booking booking);
        Task<Booking?> GetByIdAsync(int id);
        Task<Booking?> GetByReferenceAsync(string reference);
        Task UpdateAsync(Booking booking);
    }
}
