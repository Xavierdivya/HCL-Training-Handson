
using System.Security.Claims;
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.DTOs.Students;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IPlacementOfficerRepository _placementOfficerRepository;

        public StudentsController(
            IStudentService studentService,
            IPlacementOfficerRepository placementOfficerRepository)
        {
            _studentService = studentService;
            _placementOfficerRepository = placementOfficerRepository;
        }

        // Placement Officer registers a student for their own college.
        [HttpPost]
        [Authorize(Roles = AppRoles.PlacementOfficer)]
        public async Task<ActionResult<StudentResponseDto>> Register(
            StudentRegistrationDto dto)
        {
            var placementOfficerUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(placementOfficerUserId))
            {
                return Unauthorized();
            }

            var student = await _studentService.RegisterAsync(
                placementOfficerUserId,
                dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = student.StudentId },
                student);
        }

        // SuperAdmin can view all students.
        [HttpGet]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetAll()
        {
            var students = await _studentService.GetAllAsync();

            return Ok(students);
        }

        // Placement Officer can view students from their own college.
        [HttpGet("college")]
        [Authorize(Roles = AppRoles.PlacementOfficer)]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetCollegeStudents()
        {
            var placementOfficerUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(placementOfficerUserId))
            {
                return Unauthorized();
            }

            var placementOfficer =
                await _placementOfficerRepository.GetByUserIdAsync(
                    placementOfficerUserId);

            if (placementOfficer == null)
            {
                return NotFound("Placement Officer profile not found.");
            }

            var students = await _studentService.GetByCollegeIdAsync(
                placementOfficer.CollegeId);

            return Ok(students);
        }

        // SuperAdmin can view any student.
        // Placement Officer can view only students from their own college.
        [HttpGet("{id:int}")]
        [Authorize(Roles = AppRoles.SuperAdmin + "," + AppRoles.PlacementOfficer)]
        public async Task<ActionResult<StudentResponseDto>> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            if (User.IsInRole(AppRoles.SuperAdmin))
            {
                return Ok(student);
            }

            var placementOfficerUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(placementOfficerUserId))
            {
                return Unauthorized();
            }

            var placementOfficer =
                await _placementOfficerRepository.GetByUserIdAsync(
                    placementOfficerUserId);

            if (placementOfficer == null)
            {
                return NotFound("Placement Officer profile not found.");
            }

            if (student.CollegeId != placementOfficer.CollegeId)
            {
                return Forbid();
            }

            return Ok(student);
        }

        // A Student can view only their own profile.
        [HttpGet("me")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<ActionResult<StudentResponseDto>> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var student = await _studentService.GetByUserIdAsync(userId);

            if (student == null)
            {
                return NotFound("Student profile not found.");
            }

            return Ok(student);
        }
    }
}
