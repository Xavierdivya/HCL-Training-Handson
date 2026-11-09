
using System.Security.Claims;
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.DTOs.Jobs;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ICompanyHRRepository _companyHRRepository;

        public JobsController(
            IJobService jobService,
            ICompanyHRRepository companyHRRepository)
        {
            _jobService = jobService;
            _companyHRRepository = companyHRRepository;
        }

        // Company HR creates a job for their own company.
        [HttpPost]
        [Authorize(Roles = AppRoles.CompanyHR)]
        public async Task<ActionResult<JobResponseDto>> Create(
            [FromBody] JobCreateDto dto)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var companyHR =
                await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                return Forbid();

            try
            {
                var job = await _jobService.CreateAsync(
                    companyHR.CompanyId,
                    dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = job.JobId },
                    job);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // SuperAdmin can view all jobs.
        [HttpGet]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<IEnumerable<JobResponseDto>>>
            GetAll()
        {
            var jobs = await _jobService.GetAllAsync();

            return Ok(jobs);
        }

        // Company HR can view only their company's jobs.
        [HttpGet("my-company")]
        [Authorize(Roles = AppRoles.CompanyHR)]
        public async Task<ActionResult<IEnumerable<JobResponseDto>>>
            GetMyCompanyJobs()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var companyHR =
                await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                return Forbid();

            var jobs = await _jobService.GetCompanyJobsAsync(
                companyHR.CompanyId);

            return Ok(jobs);
        }

        // Public endpoint: students and visitors can view open jobs.
        [HttpGet("open")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<JobResponseDto>>>
            GetOpenJobs()
        {
            var jobs = await _jobService.GetOpenJobsAsync();

            return Ok(jobs);
        }

        // SuperAdmin can view any job.
        // Company HR can view only jobs belonging to their company.
        [HttpGet("{id:int}")]
        [Authorize(Roles = AppRoles.SuperAdmin + "," + AppRoles.CompanyHR)]
        public async Task<ActionResult<JobResponseDto>> GetById(int id)
        {
            var job = await _jobService.GetByIdAsync(id);

            if (job == null)
                return NotFound();

            if (!User.IsInRole(AppRoles.SuperAdmin))
            {
                var userId = User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized();

                var companyHR =
                    await _companyHRRepository.GetByUserIdAsync(userId);

                if (companyHR == null ||
                    companyHR.CompanyId != job.CompanyId)
                {
                    return Forbid();
                }
            }

            return Ok(job);
        }

        // SuperAdmin can update any job.
        // Company HR can update only their company's jobs.
        [HttpPut("{id:int}")]
        [Authorize(Roles = AppRoles.SuperAdmin + "," + AppRoles.CompanyHR)]
        public async Task<ActionResult<JobResponseDto>> Update(
            int id,
            [FromBody] JobUpdateDto dto)
        {
            var existingJob = await _jobService.GetByIdAsync(id);

            if (existingJob == null)
                return NotFound();

            if (!User.IsInRole(AppRoles.SuperAdmin))
            {
                var userId = User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized();

                var companyHR =
                    await _companyHRRepository.GetByUserIdAsync(userId);

                if (companyHR == null ||
                    companyHR.CompanyId != existingJob.CompanyId)
                {
                    return Forbid();
                }
            }

            try
            {
                var updatedJob = await _jobService.UpdateAsync(
                    id,
                    existingJob.CompanyId,
                    dto);

                if (updatedJob == null)
                    return NotFound();

                return Ok(updatedJob);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
