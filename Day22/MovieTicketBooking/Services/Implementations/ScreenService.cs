
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services.Implementations
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _screenRepository;
        private readonly ITheatreRepository _theatreRepository;

        public ScreenService(
            IScreenRepository screenRepository,
            ITheatreRepository theatreRepository)
        {
            _screenRepository = screenRepository;
            _theatreRepository = theatreRepository;
        }

        public async Task<Screen> AddAsync(Screen screen)
        {
            await ValidateScreenAsync(screen);
            return await _screenRepository.AddAsync(screen);
        }

        public async Task<List<Screen>> GetAllAsync()
        {
            return await _screenRepository.GetAllAsync();
        }

        public async Task<Screen?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid screen ID.");

            return await _screenRepository.GetByIdAsync(id);
        }

        public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
        {
            if (theatreId <= 0)
                throw new ArgumentException("Invalid theatre ID.");

            return await _screenRepository.GetByTheatreIdAsync(theatreId);
        }

        public async Task UpdateAsync(Screen screen)
        {
            await ValidateScreenAsync(screen);

            var existingScreen =
                await _screenRepository.GetByIdAsync(screen.ScreenId);

            if (existingScreen == null)
                throw new KeyNotFoundException("Screen not found.");

            await _screenRepository.UpdateAsync(screen);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid screen ID.");

            return await _screenRepository.DeleteAsync(id);
        }

        private async Task ValidateScreenAsync(Screen screen)
        {
            if (screen == null)
                throw new ArgumentNullException(nameof(screen));

            if (string.IsNullOrWhiteSpace(screen.ScreenName))
                throw new ArgumentException("Screen name is required.");

            if (screen.TheatreId <= 0)
                throw new ArgumentException("Valid theatre ID is required.");

            var theatre =
                await _theatreRepository.GetByIdAsync(screen.TheatreId);

            if (theatre == null)
                throw new KeyNotFoundException("Theatre not found.");
        }
    }
}
