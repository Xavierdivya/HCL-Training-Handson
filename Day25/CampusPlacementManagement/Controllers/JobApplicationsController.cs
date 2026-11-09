
using System.Security.Claims;
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.DTOs.JobApplications;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationService _applicationService;

        public JobApplicationsController(
            IJobApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        // Student submits a job application.
        [HttpPost]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<ActionResult<JobApplicationResponseDto>> Apply(
            JobApplicationCreateDto dto)
        {
            var studentUserId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(studentUserId))
            {
                return Unauthorized();
            }

            try
            {
                var application = await _applicationService.ApplyAsync(
                    studentUserId,
                    dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = application.JobApplicationId },
                    application);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // SuperAdmin views all applications.
        [HttpGet]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<IEnumerable<JobApplicationResponseDto>>>
            GetAll()
        {
            var applications =
                await _applicationService.GetAllAsync();

            return Ok(applications);
        }

        // Student views their own applications.
        [HttpGet("my")]
        [Authorize(Roles = AppRoles.Student)]
        public async Task<ActionResult<IEnumerable<JobApplicationResponseDto>>>
            GetMyApplications()
        {
            var studentUserId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(studentUserId))
            {
                return Unauthorized();
            }

            var applications =
                await _applicationService.GetByStudentUserIdAsync(
                    studentUserId);

            return Ok(applications);
        }

        // Company HR views applications for their company.
        [HttpGet("company")]
        [Authorize(Roles = AppRoles.CompanyHR)]
        public async Task<ActionResult<IEnumerable<JobApplicationResponseDto>>>
            GetCompanyApplications()
        {
            if (!TryGetClaimId("CompanyId", out int companyId))
            {
                return Forbid();
            }

            var applications =
                await _applicationService.GetByCompanyIdAsync(companyId);

            return Ok(applications);
        }

        // Placement Officer views applications from their college.
        [HttpGet("college")]
        [Authorize(Roles = AppRoles.PlacementOfficer)]
        public async Task<ActionResult<IEnumerable<JobApplicationResponseDto>>>
            GetCollegeApplications()
        {
            if (!TryGetClaimId("CollegeId", out int collegeId))
            {
                return Forbid();
            }

            var applications =
                await _applicationService.GetByCollegeIdAsync(collegeId);

            return Ok(applications);
        }

        // SuperAdmin, Company HR or Placement Officer
        // can retrieve an individual application.
        [HttpGet("{id:int}")]
        [Authorize(Roles = AppRoles.SuperAdmin + "," +
                          AppRoles.CompanyHR + "," +
                          AppRoles.PlacementOfficer)]
        public async Task<ActionResult<JobApplicationResponseDto>>
            GetById(int id)
        {
            var application =
                await _applicationService.GetByIdAsync(id);

            if (application == null)
            {
                return NotFound(new
                {
                    message = "Job application not found."
                });
            }

            if (User.IsInRole(AppRoles.SuperAdmin))
            {
                return Ok(application);
            }

            if (User.IsInRole(AppRoles.CompanyHR))
            {
                if (!TryGetClaimId("CompanyId", out int companyId) ||
                    application.CompanyId != companyId)
                {
                    return Forbid();
                }

                return Ok(application);
            }

            if (User.IsInRole(AppRoles.PlacementOfficer))
            {
                if (!TryGetClaimId("CollegeId", out int collegeId) ||
                    application.CollegeId != collegeId)
                {
                    return Forbid();
                }

                return Ok(application);
            }

            return Forbid();
        }

        // Company HR updates an application's status.
        [HttpPut("{id:int}/status")]
        [Authorize(Roles = AppRoles.CompanyHR)]
        public async Task<ActionResult<JobApplicationResponseDto>>
            UpdateStatus(
                int id,
                JobApplicationStatusUpdateDto dto)
        {
            if (!TryGetClaimId("CompanyId", out int companyId))
            {
                return Forbid();
            }

            try
            {
                var application =
                    await _applicationService.UpdateStatusAsync(
                        id,
                        companyId,
                        dto);

                if (application == null)
                {
                    return NotFound(new
                    {
                        message = "Job application not found."
                    });
                }

                return Ok(application);
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

        // Helper method to read integer JWT claims.
        private bool TryGetClaimId(
            string claimType,
            out int id)
        {
            var claimValue = User.FindFirst(claimType)?.Value;

            return int.TryParse(claimValue, out id);
        }
    }
}
