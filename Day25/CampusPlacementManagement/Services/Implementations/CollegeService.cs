
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

        // Register a new college
        public async Task<CollegeResponseDto> RegisterAsync(
            CollegeRegistrationDto registrationDto)
        {
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

        // Get all colleges
        public async Task<IEnumerable<CollegeResponseDto>> GetAllAsync()
        {
            var colleges = await _collegeRepository.GetAllAsync();

            return colleges.Select(MapToResponseDto);
        }

        // Get a college by ID
        public async Task<CollegeResponseDto?> GetByIdAsync(int collegeId)
        {
            var college = await _collegeRepository.GetByIdAsync(collegeId);

            return college == null
                ? null
                : MapToResponseDto(college);
        }

        // Get colleges awaiting approval
        public async Task<IEnumerable<CollegeResponseDto>> GetPendingAsync()
        {
            var colleges = await _collegeRepository.GetPendingAsync();

            return colleges.Select(MapToResponseDto);
        }

        // Approve a pending college
        public async Task<CollegeResponseDto?> ApproveAsync(int collegeId)
        {
            var college = await _collegeRepository
                .GetByIdForUpdateAsync(collegeId);

            if (college == null)
            {
                return null;
            }

            if (college.Status != CollegeStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Only pending colleges can be approved.");
            }

            college.Status = CollegeStatus.Approved;

            await _collegeRepository.SaveChangesAsync();

            return MapToResponseDto(college);
        }

        // Reject a pending college
        public async Task<CollegeResponseDto?> RejectAsync(int collegeId)
        {
            var college = await _collegeRepository
                .GetByIdForUpdateAsync(collegeId);

            if (college == null)
            {
                return null;
            }

            if (college.Status != CollegeStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Only pending colleges can be rejected.");
            }

            college.Status = CollegeStatus.Rejected;

            await _collegeRepository.SaveChangesAsync();

            return MapToResponseDto(college);
        }

        // Suspend an approved college
        public async Task<CollegeResponseDto?> SuspendAsync(int collegeId)
        {
            var college = await _collegeRepository
                .GetByIdForUpdateAsync(collegeId);

            if (college == null)
            {
                return null;
            }

            if (college.Status != CollegeStatus.Approved)
            {
                throw new InvalidOperationException(
                    "Only approved colleges can be suspended.");
            }

            college.Status = CollegeStatus.Suspended;

            await _collegeRepository.SaveChangesAsync();

            return MapToResponseDto(college);
        }

        // Convert College entity to response DTO
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
