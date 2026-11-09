using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Students;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentRepository _studentRepository;
        private readonly IPlacementOfficerRepository _placementOfficerRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentService(
            ApplicationDbContext context,
            IStudentRepository studentRepository,
            IPlacementOfficerRepository placementOfficerRepository,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _studentRepository = studentRepository;
            _placementOfficerRepository = placementOfficerRepository;
            _userManager = userManager;
        }

        // Register a student under the logged-in Placement Officer.
        public async Task<StudentResponseDto> RegisterAsync(
            string placementOfficerUserId,
            StudentRegistrationDto dto)
        {
            // Find the Placement Officer.
            var placementOfficer =
                await _placementOfficerRepository
                    .GetByUserIdAsync(placementOfficerUserId);

            if (placementOfficer == null)
            {
                throw new InvalidOperationException(
                    "Placement Officer profile not found.");
            }

            if (!await _userManager.IsInRoleAsync(
                placementOfficer.User, AppRoles.PlacementOfficer))
            {
                throw new UnauthorizedAccessException(
                    "Only Placement Officers can register students.");
            }

            // Verify that the Placement Officer account is approved.
            if (placementOfficer.User.AccountStatus != AccountStatus.Approved)
            {
                throw new InvalidOperationException(
                    "Placement Officer account is not approved.");
            }

            // Verify that the college is approved.
            var college = await _context.Colleges
                .FirstOrDefaultAsync(c =>
                    c.CollegeId == placementOfficer.CollegeId);

            if (college == null ||
                college.Status != CollegeStatus.Approved)
            {
                throw new InvalidOperationException(
                    "Students can only be registered under an approved college.");
            }

            var email = dto.Email.Trim();

            // Check duplicate email.
            var existingUser =
                await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "An account with this email already exists.");
            }

            var registerNumber = dto.RegisterNumber.Trim();

            // Check duplicate registration number within the college.
            var existingStudent =
                await _studentRepository.GetByRegisterNumberAsync(
                    placementOfficer.CollegeId,
                    registerNumber);

            if (existingStudent != null)
            {
                throw new InvalidOperationException(
                    "This registration number already exists in the college.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Create the Identity user.
                var studentUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = dto.PhoneNumber.Trim(),
                    FullName = dto.FullName.Trim(),

                    CollegeId = placementOfficer.CollegeId,

                    AccountStatus = AccountStatus.Approved,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createResult = await _userManager.CreateAsync(
                    studentUser,
                    dto.Password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(errors);
                }

                // Assign the Student role.
                var roleResult = await _userManager.AddToRoleAsync(
                    studentUser,
                    AppRoles.Student);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(errors);
                }

                // Create the Student profile.
                var student = new Student
                {
                    UserId = studentUser.Id,

                    CollegeId = placementOfficer.CollegeId,
                    PlacementOfficerId =
                        placementOfficer.PlacementOfficerId,

                    RegisterNumber = registerNumber,
                    Department = dto.Department.Trim(),
                    Degree = dto.Degree.Trim(),
                    GraduationYear = dto.GraduationYear,

                    CGPA = dto.CGPA,
                    ActiveBacklogs = dto.ActiveBacklogs,

                    ResumeUrl = dto.ResumeUrl?.Trim(),

                    Status = StudentStatus.Active,
                    CreatedAt = DateTime.UtcNow,

                    User = studentUser,
                    College = college,
                    PlacementOfficer = placementOfficer
                };

                await _studentRepository.AddAsync(student);

                await _studentRepository.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapToResponseDto(student);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Get all students.
        public async Task<IEnumerable<StudentResponseDto>> GetAllAsync()
        {
            var students = await _studentRepository.GetAllAsync();

            return students.Select(MapToResponseDto);
        }

        // Get student by ID.
        public async Task<StudentResponseDto?> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            return student == null
                ? null
                : MapToResponseDto(student);
        }

        // Get students belonging to a college.
        public async Task<IEnumerable<StudentResponseDto>>
            GetByCollegeIdAsync(int collegeId)
        {
            var students =
                await _studentRepository.GetByCollegeIdAsync(collegeId);

            return students.Select(MapToResponseDto);
        }

        // Get student by Identity user ID.
        public async Task<StudentResponseDto?> GetByUserIdAsync(
            string userId)
        {
            var student =
                await _studentRepository.GetByUserIdAsync(userId);

            return student == null
                ? null
                : MapToResponseDto(student);
        }

        // Map Student entity to response DTO.
        private static StudentResponseDto MapToResponseDto(
            Student student)
        {
            return new StudentResponseDto
            {
                StudentId = student.StudentId,
                UserId = student.UserId,

                FullName = student.User.FullName,
                Email = student.User.Email ?? string.Empty,
                PhoneNumber = student.User.PhoneNumber,

                CollegeId = student.CollegeId,
                CollegeName = student.College.CollegeName,

                PlacementOfficerId =
                    student.PlacementOfficerId,

                PlacementOfficerName =
                    student.PlacementOfficer.User.FullName,

                RegisterNumber = student.RegisterNumber,
                Department = student.Department,
                Degree = student.Degree,
                GraduationYear = student.GraduationYear,

                CGPA = student.CGPA,
                ActiveBacklogs = student.ActiveBacklogs,

                ResumeUrl = student.ResumeUrl,

                Status = student.Status.ToString(),
                CreatedAt = student.CreatedAt
            };
        }
    }
}