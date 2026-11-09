
using System.Security.Claims;
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.DTOs.Reports;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlacementReportsController : ControllerBase
    {
        private readonly IPlacementReportService _reportService;

        public PlacementReportsController(
            IPlacementReportService reportService)
        {
            _reportService = reportService;
        }

        // SuperAdmin: Overall placement statistics.
        [HttpGet("summary")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<PlacementSummaryDto>> GetSummary()
        {
            var report = await _reportService.GetSummaryAsync();

            return Ok(report);
        }

        // SuperAdmin: All college placement reports.
        [HttpGet("colleges")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<IEnumerable<CollegePlacementReportDto>>>
            GetCollegeReports()
        {
            var reports =
                await _reportService.GetAllCollegeReportsAsync();

            return Ok(reports);
        }

        // SuperAdmin: One college report.
        [HttpGet("colleges/{collegeId:int}")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<CollegePlacementReportDto>>
            GetCollegeReport(int collegeId)
        {
            if (collegeId <= 0)
                return BadRequest(new { message = "Invalid college ID." });

            var report =
                await _reportService.GetCollegeReportAsync(collegeId);

            if (report == null)
                return NotFound(new { message = "College not found." });

            return Ok(report);
        }

        // Placement Officer: Own college report.
        [HttpGet("my-college")]
        [Authorize(Roles = AppRoles.PlacementOfficer)]
        public async Task<ActionResult<CollegePlacementReportDto>>
            GetMyCollegeReport()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var report =
                    await _reportService.GetMyCollegeReportAsync(userId);

                if (report == null)
                    return NotFound(new { message = "College report not found." });

                return Ok(report);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // SuperAdmin: All company recruitment reports.
        [HttpGet("companies")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<IEnumerable<CompanyRecruitmentReportDto>>>
            GetCompanyReports()
        {
            var reports =
                await _reportService.GetAllCompanyReportsAsync();

            return Ok(reports);
        }

        // SuperAdmin: One company report.
        [HttpGet("companies/{companyId:int}")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<CompanyRecruitmentReportDto>>
            GetCompanyReport(int companyId)
        {
            if (companyId <= 0)
                return BadRequest(new { message = "Invalid company ID." });

            var report =
                await _reportService.GetCompanyReportAsync(companyId);

            if (report == null)
                return NotFound(new { message = "Company not found." });

            return Ok(report);
        }

        // Company HR: Own company report.
        [HttpGet("my-company")]
        [Authorize(Roles = AppRoles.CompanyHR)]
        public async Task<ActionResult<CompanyRecruitmentReportDto>>
            GetMyCompanyReport()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var report =
                    await _reportService.GetMyCompanyReportAsync(userId);

                if (report == null)
                    return NotFound(new { message = "Company report not found." });

                return Ok(report);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // SuperAdmin: Job-wise application statistics.
        [HttpGet("jobs")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<IEnumerable<JobApplicationReportDto>>>
            GetJobReports()
        {
            var reports =
                await _reportService.GetJobReportsAsync();

            return Ok(reports);
        }
    }
}
