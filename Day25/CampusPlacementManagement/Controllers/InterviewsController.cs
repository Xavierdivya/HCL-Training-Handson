using System.Security.Claims;
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.DTOs.Interviews;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InterviewsController : ControllerBase
    {
        private readonly IInterviewService _interviewService;
        private readonly ICompanyHRRepository _companyHRRepository;
        private readonly IStudentRepository _studentRepository;

        public InterviewsController(
            IInterviewService interviewService,
            ICompanyHRRepository companyHRRepository,
            IStudentRepository studentRepository)
        {
            _interviewService = interviewService;
            _companyHRRepository = companyHRRepository;
            _studentRepository = studentRepository;
        }

        // Company HR schedules an interview for a shortlisted application.
        [HttpPost]
        [Authorize(Roles = AppRoles.CompanyHR)]
        public async Task<ActionResult<InterviewResponseDto>> Create(
            [FromBody] InterviewCreateDto dto)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var interview =
                    await _interviewService.CreateAsync(userId, dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = interview.InterviewScheduleId },
                    interview);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // SuperAdmin views all interviews.
        [HttpGet]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<IEnumerable<InterviewResponseDto>>>
            GetAll()
        {
            var interviews = await _interviewService.GetAllAsync();

            return Ok(interviews);
        }

        // Company HR views interviews belonging to their company.
        [HttpGet("my-company")]
        [Authorize(Roles = AppRoles.CompanyHR)]
        public async Task<ActionResult<IEnumerable<InterviewResponseDto>>>
            GetMyCompanyInterviews()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var interviews =
                    await _interviewService.GetByCompanyHRUserIdAsync(userId);

                return Ok(interviews);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // Student views only their own interviews.
        [HttpGet("my-interviews")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<ActionResult<IEnumerable<InterviewResponseDto>>>
            GetMyInterviews()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var interviews =
                await _interviewService.GetByStudentUserIdAsync(userId);

            return Ok(interviews);
        }

        // SuperAdmin can view any interview.
        // Company HR can view interviews belonging to their company.
        // Students can view only their own interviews.
        [HttpGet("{id:int}")]
        [Authorize(Roles =
            AppRoles.SuperAdmin + "," +
            AppRoles.CompanyHR + "," +
            AppRoles.Student)]
        public async Task<ActionResult<InterviewResponseDto>> GetById(
            int id)
        {
            var interview =
                await _interviewService.GetByIdAsync(id);

            if (interview == null)
                return NotFound();

            if (User.IsInRole(AppRoles.SuperAdmin))
                return Ok(interview);

            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            if (User.IsInRole(AppRoles.CompanyHR))
            {
                var companyHR =
                    await _companyHRRepository.GetByUserIdAsync(userId);

                if (companyHR == null ||
                    companyHR.CompanyId != interview.CompanyId)
                {
                    return Forbid();
                }

                return Ok(interview);
            }

            if (User.IsInRole(AppRoles.Student))
            {
                var student =
                    await _studentRepository.GetByUserIdAsync(userId);

                if (student == null ||
                    student.StudentId != interview.StudentId)
                {
                    return Forbid();
                }

                return Ok(interview);
            }

            return Forbid();
        }

        // Company HR updates their own interview.
        [HttpPut("{id:int}")]
        [Authorize(Roles = AppRoles.CompanyHR)]
        public async Task<ActionResult<InterviewResponseDto>> Update(
            int id,
            [FromBody] InterviewUpdateDto dto)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var updatedInterview =
                    await _interviewService.UpdateAsync(
                        id,
                        userId,
                        dto);

                if (updatedInterview == null)
                    return NotFound();

                return Ok(updatedInterview);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}