
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services.Implementations
{
    public class TheatreService : ITheatreService
    {
        private readonly ITheatreRepository _theatreRepository;

        public TheatreService(ITheatreRepository theatreRepository)
        {
            _theatreRepository = theatreRepository;
        }

        public async Task<Theatre> AddAsync(Theatre theatre)
        {
            ValidateTheatre(theatre);
            return await _theatreRepository.AddAsync(theatre);
        }

        public async Task<List<Theatre>> GetAllAsync()
        {
            return await _theatreRepository.GetAllAsync();
        }

        public async Task<Theatre?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid theatre ID.");

            return await _theatreRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Theatre theatre)
        {
            ValidateTheatre(theatre);

            var existingTheatre =
                await _theatreRepository.GetByIdAsync(theatre.TheatreId);

            if (existingTheatre == null)
                throw new KeyNotFoundException("Theatre not found.");

            await _theatreRepository.UpdateAsync(theatre);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid theatre ID.");

            return await _theatreRepository.DeleteAsync(id);
        }

        private static void ValidateTheatre(Theatre theatre)
        {
            if (theatre == null)
                throw new ArgumentNullException(nameof(theatre));

            if (string.IsNullOrWhiteSpace(theatre.Name))
                throw new ArgumentException("Theatre name is required.");

            if (string.IsNullOrWhiteSpace(theatre.Location))
                throw new ArgumentException("Theatre location is required.");
        }
    }
}
