
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IShowRepository
    {
        Task<Show> AddAsync(Show show);
        Task<List<Show>> GetAllAsync();
        Task<Show?> GetByIdAsync(int id);
        Task<List<Show>> GetByMovieIdAsync(int movieId);
        Task<List<Show>> GetByScreenIdAsync(int screenId);
        Task<List<Show>> GetByDateAsync(DateTime date);
        Task UpdateAsync(Show show);
        Task<Show> AddWithSeatsAsync(Show show);

        Task<bool> HasScheduleConflictAsync(
    int screenId,
    DateTime startTime,
    DateTime endTime,
    int? excludeShowId = null);
    }
}
