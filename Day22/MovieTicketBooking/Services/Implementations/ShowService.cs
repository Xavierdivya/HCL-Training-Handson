
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services.Implementations
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IScreenRepository _screenRepository;

        public ShowService(
            IShowRepository showRepository,
            IMovieRepository movieRepository,
            IScreenRepository screenRepository)
        {
            _showRepository = showRepository;
            _movieRepository = movieRepository;
            _screenRepository = screenRepository;
        }

        public async Task<Show> AddAsync(Show show)
        {
            ValidateShow(show);

            var movie = await _movieRepository.GetByIdAsync(show.MovieId);

            if (movie == null)
                throw new KeyNotFoundException("Movie not found.");

            var screen = await _screenRepository.GetByIdAsync(show.ScreenId);

            if (screen == null)
                throw new KeyNotFoundException("Screen not found.");

            if (show.StartTime <= DateTime.Now)
                throw new ArgumentException(
                    "Show start time must be in the future.");

            return await _showRepository.AddWithSeatsAsync(show);
        }

        public async Task<List<Show>> GetAllAsync()
        {
            return await _showRepository.GetAllAsync();
        }

        public async Task<Show?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid show ID.");

            return await _showRepository.GetByIdAsync(id);
        }

        public async Task<List<Show>> GetByMovieIdAsync(int movieId)
        {
            if (movieId <= 0)
                throw new ArgumentException("Invalid movie ID.");

            return await _showRepository.GetByMovieIdAsync(movieId);
        }

        public async Task<List<Show>> GetByScreenIdAsync(int screenId)
        {
            if (screenId <= 0)
                throw new ArgumentException("Invalid screen ID.");

            return await _showRepository.GetByScreenIdAsync(screenId);
        }

        public async Task<List<Show>> GetByDateAsync(DateTime date)
        {
            return await _showRepository.GetByDateAsync(date);
        }

        public async Task UpdateAsync(Show show)
        {
            bool hasOverlap = await _showRepository.HasScheduleConflictAsync(
    show.ScreenId,
    show.StartTime,
    show.EndTime,
    show.ShowId);

            if (hasOverlap)
                throw new InvalidOperationException(
                    "Another show is already scheduled during this time.");

            await _showRepository.UpdateAsync(show);
        }

        private static void ValidateShow(Show show)
        {
            if (show == null)
                throw new ArgumentNullException(nameof(show));

            if (show.MovieId <= 0)
                throw new ArgumentException("Valid movie ID is required.");

            if (show.ScreenId <= 0)
                throw new ArgumentException("Valid screen ID is required.");

            if (show.EndTime <= show.StartTime)
                throw new ArgumentException(
                    "Show end time must be later than start time.");

            if (show.TicketPrice < 0)
                throw new ArgumentException(
                    "Ticket price cannot be negative.");
        }
    }
}
