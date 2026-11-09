using CampusPlacementManagement.DTOs.Jobs;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IJobService
    {
        Task<JobResponseDto> CreateAsync(
            int companyId,
            JobCreateDto dto);

        Task<JobResponseDto?> UpdateAsync(
            int jobId,
            int companyId,
            JobUpdateDto dto);

        Task<IEnumerable<JobResponseDto>> GetAllAsync();

        Task<IEnumerable<JobResponseDto>> GetCompanyJobsAsync(
            int companyId);

        Task<IEnumerable<JobResponseDto>> GetOpenJobsAsync();

        Task<JobResponseDto?> GetByIdAsync(int jobId);
    }
}