using System.ComponentModel.DataAnnotations;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Booking> CreateBookingAsync(
            int showId,
            List<int> seatIds,
            string customerName,
            string customerEmail,
            string customerPhone)
        {
            if (showId <= 0)
                throw new ArgumentException("Invalid show ID.");

            if (seatIds == null || seatIds.Count == 0)
                throw new ArgumentException("Select at least one seat.");

            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Customer name is required.");

            if (string.IsNullOrWhiteSpace(customerEmail) ||
                !new EmailAddressAttribute().IsValid(customerEmail))
                throw new ArgumentException("Enter a valid email address.");

            if (string.IsNullOrWhiteSpace(customerPhone))
                throw new ArgumentException("Customer phone is required.");

            if (seatIds.Any(id => id <= 0))
                throw new ArgumentException("Invalid seat ID.");

            var booking = new Booking
            {
                BookingReference = Guid.NewGuid().ToString("N").ToUpper(),
                ShowId = showId,
                CustomerName = customerName.Trim(),
                CustomerEmail = customerEmail.Trim(),
                CustomerPhone = customerPhone.Trim()
            };

            return await _bookingRepository.CreateBookingWithSeatsAsync(
                booking, seatIds);
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid booking ID.");

            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<Booking?> GetByReferenceAsync(
            string bookingReference)
        {
            if (string.IsNullOrWhiteSpace(bookingReference))
                throw new ArgumentException("Booking reference is required.");

            return await _bookingRepository.GetByReferenceAsync(
                bookingReference);
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            if (bookingId <= 0)
                throw new ArgumentException("Invalid booking ID.");

            await _bookingRepository.CancelBookingAsync(bookingId);
        }
    }
}