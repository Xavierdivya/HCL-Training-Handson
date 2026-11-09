
using CampusPlacementManagement.DTOs.JobApplications;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IJobApplicationService
    {
        // Student applies for a job.
        Task<JobApplicationResponseDto> ApplyAsync(
            string studentUserId,
            JobApplicationCreateDto dto);

        // SuperAdmin views all applications.
        Task<IEnumerable<JobApplicationResponseDto>> GetAllAsync();

        // Student views their own applications.
        Task<IEnumerable<JobApplicationResponseDto>> GetByStudentUserIdAsync(
            string studentUserId);

        // Company HR views applications for their company.
        Task<IEnumerable<JobApplicationResponseDto>> GetByCompanyIdAsync(
            int companyId);

        // Placement Officer views applications from their college.
        Task<IEnumerable<JobApplicationResponseDto>> GetByCollegeIdAsync(
            int collegeId);

        // Retrieve one application.
        Task<JobApplicationResponseDto?> GetByIdAsync(
            int applicationId);

        // Company HR updates application status.
        Task<JobApplicationResponseDto?> UpdateStatusAsync(
            int applicationId,
            int companyId,
            JobApplicationStatusUpdateDto dto);
    }
}
