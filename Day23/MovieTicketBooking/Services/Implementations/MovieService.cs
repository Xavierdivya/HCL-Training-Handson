
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
            ValidateMovie(movie);
            return await _movieRepository.AddAsync(movie);
        }

        public async Task<List<Movie>> GetAllAsync(
            int pageNumber, int pageSize, string? title)
        {
            if (pageNumber < 1)
                throw new ArgumentException("Page number must be greater than zero.");

            if (pageSize < 1)
                throw new ArgumentException("Page size must be greater than zero.");

            return await _movieRepository.GetAllAsync(pageNumber, pageSize, title);
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid movie ID.");

            return await _movieRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Movie movie)
        {
            ValidateMovie(movie);

            var existingMovie =
                await _movieRepository.GetByIdAsync(movie.MovieId);

            if (existingMovie == null)
                throw new KeyNotFoundException("Movie not found.");

            await _movieRepository.UpdateAsync(movie);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid movie ID.");

            return await _movieRepository.DeleteAsync(id);
        }

        private static void ValidateMovie(Movie movie)
        {
            if (movie == null)
                throw new ArgumentNullException(nameof(movie));

            if (string.IsNullOrWhiteSpace(movie.Title))
                throw new ArgumentException("Movie title is required.");

            if (movie.DurationMinutes <= 0)
                throw new ArgumentException("Movie duration must be greater than zero.");

            if (string.IsNullOrWhiteSpace(movie.Language))
                throw new ArgumentException("Movie language is required.");

            if (string.IsNullOrWhiteSpace(movie.Genre))
                throw new ArgumentException("Movie genre is required.");
        }
    }
}
