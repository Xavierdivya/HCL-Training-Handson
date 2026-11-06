
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // Public company registration with initial HR account.
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] CompanyRegistrationDto dto)
        {
            try
            {
                var result = await _companyService.RegisterAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.CompanyId },
                    result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Retrieve all companies.
        [Authorize(Roles = AppRoles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyService.GetAllAsync();

            return Ok(companies);
        }

        // Retrieve a company by ID.
        [Authorize(Roles = AppRoles.SuperAdmin)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _companyService.GetByIdAsync(id);

            if (company == null)
            {
                return NotFound(new
                {
                    message = "Company not found."
                });
            }

            return Ok(company);
        }

        // Retrieve pending company registrations.
        [Authorize(Roles = AppRoles.SuperAdmin)]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var companies = await _companyService.GetPendingAsync();

            return Ok(companies);
        }

        // Approve a pending company.
        [Authorize(Roles = AppRoles.SuperAdmin)]
        [HttpPut("{id:int}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var result = await _companyService.ApproveAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message = "Company not found."
                    });
                }

                return Ok(new
                {
                    message = "Company approved successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Reject a pending company.
        [Authorize(Roles = AppRoles.SuperAdmin)]
        [HttpPut("{id:int}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                var result = await _companyService.RejectAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message = "Company not found."
                    });
                }

                return Ok(new
                {
                    message = "Company rejected successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Suspend an approved company.
        [Authorize(Roles = AppRoles.SuperAdmin)]
        [HttpPut("{id:int}/suspend")]
        public async Task<IActionResult> Suspend(int id)
        {
            try
            {
                var result = await _companyService.SuspendAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message = "Company not found."
                    });
                }

                return Ok(new
                {
                    message = "Company suspended successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
