
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class CollegeService : ICollegeService
    {
        private readonly ICollegeRepository _collegeRepository;

        public CollegeService(ICollegeRepository collegeRepository)
        {
            _collegeRepository = collegeRepository;
        }

        public async Task<CollegeResponseDto> RegisterAsync(
            CollegeRegistrationDto registrationDto)
        {
            // Normalize the college code
            var collegeCode = registrationDto.CollegeCode.Trim()
                .ToUpperInvariant();

            // Check for duplicate college code
            var existingCollege = await _collegeRepository
                .GetByCodeAsync(collegeCode);

            if (existingCollege != null)
            {
                throw new InvalidOperationException(
                    "A college with this code already exists.");
            }

            // Create new college
            var college = new College
            {
                CollegeName = registrationDto.CollegeName.Trim(),
                CollegeCode = collegeCode,
                UniversityName = registrationDto.UniversityName?.Trim(),
                OfficialEmail = registrationDto.OfficialEmail.Trim(),
                PhoneNumber = registrationDto.PhoneNumber.Trim(),
                Address = registrationDto.Address.Trim(),
                Website = registrationDto.Website?.Trim(),
                Status = CollegeStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _collegeRepository.AddAsync(college);

            await _collegeRepository.SaveChangesAsync();

            return MapToResponseDto(college);
        }

        public async Task<IEnumerable<CollegeResponseDto>> GetAllAsync()
        {
            var colleges = await _collegeRepository.GetAllAsync();

            return colleges.Select(MapToResponseDto);
        }

        public async Task<CollegeResponseDto?> GetByIdAsync(int collegeId)
        {
            var college = await _collegeRepository.GetByIdAsync(collegeId);

            if (college == null)
            {
                return null;
            }

            return MapToResponseDto(college);
        }

        private static CollegeResponseDto MapToResponseDto(College college)
        {
            return new CollegeResponseDto
            {
                CollegeId = college.CollegeId,
                CollegeName = college.CollegeName,
                CollegeCode = college.CollegeCode,
                UniversityName = college.UniversityName,
                OfficialEmail = college.OfficialEmail,
                PhoneNumber = college.PhoneNumber,
                Address = college.Address,
                Website = college.Website,
                Status = college.Status,
                CreatedAt = college.CreatedAt
            };
        }
    }
}
