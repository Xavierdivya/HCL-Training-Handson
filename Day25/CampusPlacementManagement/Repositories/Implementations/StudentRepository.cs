
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all students.
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.College)
                .Include(s => s.PlacementOfficer)
                    .ThenInclude(p => p.User)
                .AsNoTracking()
                .ToListAsync();
        }

        // Get student by StudentId.
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.College)
                .Include(s => s.PlacementOfficer)
                    .ThenInclude(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }

        // Get student by Identity UserId.
        public async Task<Student?> GetByUserIdAsync(string userId)
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.College)
                .Include(s => s.PlacementOfficer)
                    .ThenInclude(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        // Check registration number within a college.
        public async Task<Student?> GetByRegisterNumberAsync(
            int collegeId,
            string registerNumber)
        {
            return await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s =>
                    s.CollegeId == collegeId &&
                    s.RegisterNumber == registerNumber);
        }

        // Get all students belonging to a college.
        public async Task<IEnumerable<Student>> GetByCollegeIdAsync(
            int collegeId)
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.College)
                .Include(s => s.PlacementOfficer)
                    .ThenInclude(p => p.User)
                .AsNoTracking()
                .Where(s => s.CollegeId == collegeId)
                .ToListAsync();
        }

        // Add a new student.
        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        // Save database changes.
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
