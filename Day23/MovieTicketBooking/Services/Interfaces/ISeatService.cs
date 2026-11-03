
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ISeatService
    {
        Task<Seat> AddAsync(Seat seat);
        Task AddRangeAsync(List<Seat> seats);
        Task<List<Seat>> GetAllAsync();
        Task<Seat?> GetByIdAsync(int id);
        Task<List<Seat>> GetByScreenIdAsync(int screenId);
        Task UpdateAsync(Seat seat);
        Task<bool> DeleteAsync(int id);
    }
}
