
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<Seat> AddAsync(Seat seat);
        Task AddRangeAsync(IEnumerable<Seat> seats);
        Task<List<Seat>> GetAllAsync();
        Task<Seat?> GetByIdAsync(int id);
        Task<List<Seat>> GetByScreenIdAsync(int screenId);
        Task UpdateAsync(Seat seat);
        Task<bool> DeleteAsync(int id);
    }
}
