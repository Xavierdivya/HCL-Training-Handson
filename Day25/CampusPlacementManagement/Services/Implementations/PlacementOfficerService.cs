
using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class PlacementOfficerService : IPlacementOfficerService
    {
        private readonly IPlacementOfficerRepository _repository;

        public PlacementOfficerService(
            IPlacementOfficerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PlacementOfficerResponseDto>> GetAllAsync()
        {
            var officers = await _repository.GetAllAsync();

            return officers.Select(MapToDto);
        }

        public async Task<PlacementOfficerResponseDto?> GetByIdAsync(int id)
        {
            var officer = await _repository.GetByIdAsync(id);

            return officer == null ? null : MapToDto(officer);
        }

        public async Task<PlacementOfficerResponseDto?> GetByUserIdAsync(
            string userId)
        {
            var officer = await _repository.GetByUserIdAsync(userId);

            return officer == null ? null : MapToDto(officer);
        }

        private static PlacementOfficerResponseDto MapToDto(
            PlacementOfficer officer)
        {
            return new PlacementOfficerResponseDto
            {
                PlacementOfficerId = officer.PlacementOfficerId,
                UserId = officer.UserId,
                FullName = officer.User.FullName,
                Email = officer.User.Email ?? string.Empty,
                PhoneNumber = officer.User.PhoneNumber ?? string.Empty,
                CollegeId = officer.CollegeId,
                CollegeName = officer.College.CollegeName,
                Designation = officer.Designation,
                CreatedAt = officer.CreatedAt
            };
        }
    }
}
