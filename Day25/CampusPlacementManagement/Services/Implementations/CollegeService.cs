using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations
{
    public class CollegeService : ICollegeService
    {
        private readonly ICollegeRepository _collegeRepository;
        private readonly ApplicationDbContext _context;

        public CollegeService(
            ICollegeRepository collegeRepository,
            ApplicationDbContext context)
        {
            _collegeRepository = collegeRepository;
            _context = context;
        }

        // Register a college.
        // This endpoint will be restricted to SuperAdmin.
        public async Task<CollegeResponseDto> RegisterAsync(
            CollegeRegistrationDto registrationDto)
        {
            var collegeCode = registrationDto.CollegeCode.Trim()
                .ToUpperInvariant();

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

        public async Task<IEnumerable<CollegeResponseDto>> GetAllAsync()
        {
            var colleges = await _collegeRepository.GetAllAsync();

            return colleges.Select(MapToResponseDto);
        }

        public async Task<CollegeResponseDto?> GetByIdAsync(int collegeId)
        {
            var college = await _collegeRepository.GetByIdAsync(collegeId);

            return college == null
                ? null
                : MapToResponseDto(college);
        }

        public async Task<IEnumerable<CollegeResponseDto>> GetPendingAsync()
        {
            var colleges = await _collegeRepository.GetPendingAsync();

            return colleges.Select(MapToResponseDto);
        }

        // Approve college and its representatives together.
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

            await UpdateRepresentativeStatusesAsync(
                collegeId,
                AccountStatus.Approved);

            await _collegeRepository.SaveChangesAsync();

            return MapToResponseDto(college);
        }

        // Reject college and its representatives together.
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

            await UpdateRepresentativeStatusesAsync(
                collegeId,
                AccountStatus.Rejected);

            await _collegeRepository.SaveChangesAsync();

            return MapToResponseDto(college);
        }

        // Suspend college and its representatives together.
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

            await UpdateRepresentativeStatusesAsync(
                collegeId,
                AccountStatus.Suspended);

            await _collegeRepository.SaveChangesAsync();

            return MapToResponseDto(college);
        }

        // Update all representative accounts belonging to a college.
        private async Task UpdateRepresentativeStatusesAsync(
            int collegeId,
            AccountStatus accountStatus)
        {
            var representatives = await _context.Users
                .Where(u => u.CollegeId == collegeId)
                .ToListAsync();

            foreach (var representative in representatives)
            {
                representative.AccountStatus = accountStatus;
                representative.UpdatedAt = DateTime.UtcNow;
            }
        }

        private static CollegeResponseDto MapToResponseDto(
            College college)
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