using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();

        Task<Student?> GetByIdAsync(int id);

        Task<Student?> GetByUserIdAsync(string userId);

        Task<Student?> GetByRegisterNumberAsync(
            int collegeId,
            string registerNumber);

        Task<IEnumerable<Student>> GetByCollegeIdAsync(int collegeId);

        Task AddAsync(Student student);

        Task SaveChangesAsync();
    }
}