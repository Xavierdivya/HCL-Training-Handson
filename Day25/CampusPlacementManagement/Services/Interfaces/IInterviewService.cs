
using CampusPlacementManagement.DTOs.Interviews;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IInterviewService
    {
        Task<InterviewResponseDto> CreateAsync(
            string companyHRUserId,
            InterviewCreateDto dto);

        Task<InterviewResponseDto?> UpdateAsync(
            int interviewId,
            string companyHRUserId,
            InterviewUpdateDto dto);

        Task<IEnumerable<InterviewResponseDto>> GetAllAsync();

        Task<IEnumerable<InterviewResponseDto>>
            GetByCompanyHRUserIdAsync(string companyHRUserId);

        Task<IEnumerable<InterviewResponseDto>>
            GetByStudentUserIdAsync(string studentUserId);

        Task<InterviewResponseDto?> GetByIdAsync(int id);
    }
}