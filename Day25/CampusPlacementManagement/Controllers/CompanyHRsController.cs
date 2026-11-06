
using System.Security.Claims;
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.Authorization.Requirements;
using CampusPlacementManagement.DTOs.CompanyHRs;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyHRsController : ControllerBase
    {
        private readonly ICompanyHRService _companyHRService;
        private readonly ICompanyService _companyService;
        private readonly IAuthorizationService _authorizationService;

        public CompanyHRsController(
            ICompanyHRService companyHRService,
            ICompanyService companyService,
            IAuthorizationService authorizationService)
        {
            _companyHRService = companyHRService;
            _companyService = companyService;
            _authorizationService = authorizationService;
        }

        // SuperAdmin can retrieve all Company HR profiles.
        [Authorize(Roles = AppRoles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companyHRs = await _companyHRService.GetAllAsync();

            return Ok(companyHRs);
        }

        // SuperAdmin can retrieve a specific HR profile.
        [Authorize(Roles = AppRoles.SuperAdmin)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var companyHR = await _companyHRService.GetByIdAsync(id);

            if (companyHR == null)
            {
                return NotFound(new
                {
                    message = "Company HR profile not found."
                });
            }

            return Ok(companyHR);
        }

        // Company HR can retrieve their own profile.
        [Authorize(Roles = AppRoles.CompanyHR)]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var companyHR = await _companyHRService
                .GetByUserIdAsync(userId);

            if (companyHR == null)
            {
                return NotFound(new
                {
                    message = "Company HR profile not found."
                });
            }

            var authorizationResult =
                await _authorizationService.AuthorizeAsync(
                    User,
                    companyHR.CompanyId,
                    new CompanyAccessRequirement());

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            return Ok(companyHR);
        }

        // Company HR can retrieve profiles belonging
        // only to their own company.
        [Authorize(Roles = AppRoles.CompanyHR)]
        [HttpGet("company")]
        public async Task<IActionResult> GetCompanyHRs()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var currentHR = await _companyHRService
                .GetByUserIdAsync(userId);

            if (currentHR == null)
            {
                return NotFound(new
                {
                    message = "Company HR profile not found."
                });
            }

            var authorizationResult =
                await _authorizationService.AuthorizeAsync(
                    User,
                    currentHR.CompanyId,
                    new CompanyAccessRequirement());

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var companyHRs = await _companyHRService
                .GetByCompanyIdAsync(currentHR.CompanyId);

            return Ok(companyHRs);
        }

        // An approved Company HR account can register
        // another HR account for the same company.
        [Authorize(Roles = AppRoles.CompanyHR)]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] CompanyHRRegistrationDto dto)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var currentHR = await _companyHRService
                .GetByUserIdAsync(userId);

            if (currentHR == null)
            {
                return NotFound(new
                {
                    message = "Company HR profile not found."
                });
            }

            var authorizationResult =
                await _authorizationService.AuthorizeAsync(
                    User,
                    currentHR.CompanyId,
                    new CompanyAccessRequirement());

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            try
            {
                var result = await _companyService.RegisterHRAsync(
                    userId,
                    dto);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
