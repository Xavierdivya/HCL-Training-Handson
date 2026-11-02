
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services.Implementations
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IScreenRepository _screenRepository;

        public SeatService(
            ISeatRepository seatRepository,
            IScreenRepository screenRepository)
        {
            _seatRepository = seatRepository;
            _screenRepository = screenRepository;
        }

        public async Task<Seat> AddAsync(Seat seat)
        {
            await ValidateSeatAsync(seat);
            return await _seatRepository.AddAsync(seat);
        }

        public async Task AddRangeAsync(List<Seat> seats)
        {
            if (seats == null || seats.Count == 0)
                throw new ArgumentException("At least one seat is required.");

            foreach (var seat in seats)
            {
                await ValidateSeatAsync(seat);
            }

            await _seatRepository.AddRangeAsync(seats);
        }

        public async Task<List<Seat>> GetAllAsync()
        {
            return await _seatRepository.GetAllAsync();
        }

        public async Task<Seat?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid seat ID.");

            return await _seatRepository.GetByIdAsync(id);
        }

        public async Task<List<Seat>> GetByScreenIdAsync(int screenId)
        {
            if (screenId <= 0)
                throw new ArgumentException("Invalid screen ID.");

            return await _seatRepository.GetByScreenIdAsync(screenId);
        }

        public async Task UpdateAsync(Seat seat)
        {
            await ValidateSeatAsync(seat);

            var existingSeat =
                await _seatRepository.GetByIdAsync(seat.SeatId);

            if (existingSeat == null)
                throw new KeyNotFoundException("Seat not found.");

            await _seatRepository.UpdateAsync(seat);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid seat ID.");

            return await _seatRepository.DeleteAsync(id);
        }

        private async Task ValidateSeatAsync(Seat seat)
        {
            if (seat == null)
                throw new ArgumentNullException(nameof(seat));

            if (seat.ScreenId <= 0)
                throw new ArgumentException("Valid screen ID is required.");

            if (string.IsNullOrWhiteSpace(seat.RowName))
                throw new ArgumentException("Seat row name is required.");

            if (seat.SeatNumber <= 0)
                throw new ArgumentException("Seat number must be greater than zero.");

            if (string.IsNullOrWhiteSpace(seat.SeatType))
                throw new ArgumentException("Seat type is required.");

            var screen =
                await _screenRepository.GetByIdAsync(seat.ScreenId);

            if (screen == null)
                throw new KeyNotFoundException("Screen not found.");
        }
    }
}
