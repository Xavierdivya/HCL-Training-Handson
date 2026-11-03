
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ITheatreService
    {
        Task<Theatre> AddAsync(Theatre theatre);
        Task<List<Theatre>> GetAllAsync();
        Task<Theatre?> GetByIdAsync(int id);
        Task UpdateAsync(Theatre theatre);
        Task<bool> DeleteAsync(int id);
    }
}
