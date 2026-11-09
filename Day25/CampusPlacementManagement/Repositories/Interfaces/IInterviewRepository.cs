using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IInterviewRepository
    {
        Task<IEnumerable<InterviewSchedule>> GetAllAsync();

        Task<InterviewSchedule?> GetByIdAsync(int id);

        Task<IEnumerable<InterviewSchedule>> GetByCompanyIdAsync(
            int companyId);

        Task<IEnumerable<InterviewSchedule>> GetByStudentIdAsync(
            int studentId);

        Task AddAsync(InterviewSchedule interview);

        Task SaveChangesAsync();
    }
}