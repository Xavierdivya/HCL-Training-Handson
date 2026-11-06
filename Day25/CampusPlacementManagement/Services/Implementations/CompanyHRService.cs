using CampusPlacementManagement.DTOs.CompanyHRs;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class CompanyHRService : ICompanyHRService
    {
        private readonly ICompanyHRRepository _companyHRRepository;

        public CompanyHRService(
            ICompanyHRRepository companyHRRepository)
        {
            _companyHRRepository = companyHRRepository;
        }

        public async Task<IEnumerable<CompanyHRResponseDto>> GetAllAsync()
        {
            var companyHRs = await _companyHRRepository.GetAllAsync();

            return companyHRs.Select(MapToDto);
        }

        public async Task<CompanyHRResponseDto?> GetByIdAsync(int id)
        {
            var companyHR = await _companyHRRepository.GetByIdAsync(id);

            return companyHR == null ? null : MapToDto(companyHR);
        }

        public async Task<CompanyHRResponseDto?> GetByUserIdAsync(
            string userId)
        {
            var companyHR = await _companyHRRepository
                .GetByUserIdAsync(userId);

            return companyHR == null ? null : MapToDto(companyHR);
        }

        public async Task<IEnumerable<CompanyHRResponseDto>> GetByCompanyIdAsync(
            int companyId)
        {
            var companyHRs = await _companyHRRepository
                .GetByCompanyIdAsync(companyId);

            return companyHRs.Select(MapToDto);
        }

        private static CompanyHRResponseDto MapToDto(
            Models.CompanyHR companyHR)
        {
            return new CompanyHRResponseDto
            {
                CompanyHRId = companyHR.CompanyHRId,
                UserId = companyHR.UserId,
                FullName = companyHR.User.FullName,
                Email = companyHR.User.Email ?? string.Empty,
                PhoneNumber = companyHR.User.PhoneNumber ?? string.Empty,
                CompanyId = companyHR.CompanyId,
                CompanyName = companyHR.Company.CompanyName,
                Designation = companyHR.Designation,
                CreatedAt = companyHR.CreatedAt
            };
        }
    }
}