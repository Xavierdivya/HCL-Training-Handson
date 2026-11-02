
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<Movie> AddAsync(Movie movie);

        Task<List<Movie>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? title);

        Task<Movie?> GetByIdAsync(int id);

        Task UpdateAsync(Movie movie);

        Task<bool> DeleteAsync(int id);
    }
}
