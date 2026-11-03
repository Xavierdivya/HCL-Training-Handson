
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IMovieService
    {
        Task<Movie> AddAsync(Movie movie);
        Task<List<Movie>> GetAllAsync(int pageNumber, int pageSize, string? title);
        Task<Movie?> GetByIdAsync(int id);
        Task UpdateAsync(Movie movie);
        Task<bool> DeleteAsync(int id);
    }
}
