
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IScreenRepository
    {
        Task<Screen> AddAsync(Screen screen);
        Task<List<Screen>> GetAllAsync();
        Task<Screen?> GetByIdAsync(int id);
        Task<List<Screen>> GetByTheatreIdAsync(int theatreId);
        Task UpdateAsync(Screen screen);
        Task<bool> DeleteAsync(int id);
    }
}
