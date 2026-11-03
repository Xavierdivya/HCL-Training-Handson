
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IScreenService
    {
        Task<Screen> AddAsync(Screen screen);
        Task<List<Screen>> GetAllAsync();
        Task<Screen?> GetByIdAsync(int id);
        Task<List<Screen>> GetByTheatreIdAsync(int theatreId);
        Task UpdateAsync(Screen screen);
        Task<bool> DeleteAsync(int id);
    }
}
