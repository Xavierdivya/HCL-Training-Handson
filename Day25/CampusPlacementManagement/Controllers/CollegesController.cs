
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegesController : ControllerBase
    {
        private readonly ICollegeService _collegeService;

        public CollegesController(ICollegeService collegeService)
        {
            _collegeService = collegeService;
        }

        // Register a new college
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            CollegeRegistrationDto registrationDto)
        {
            try
            {
                var college = await _collegeService
                    .RegisterAsync(registrationDto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = college.CollegeId },
                    college);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // Get all colleges
        [HttpGet]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> GetAll()
        {
            var colleges = await _collegeService.GetAllAsync();

            return Ok(colleges);
        }

        // Get college by ID
        [HttpGet("{id:int}")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> GetById(int id)
        {
            var college = await _collegeService.GetByIdAsync(id);

            if (college == null)
            {
                return NotFound(new
                {
                    message = "College not found."
                });
            }

            return Ok(college);
        }

        // Get colleges awaiting approval
        [HttpGet("pending")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> GetPending()
        {
            var colleges = await _collegeService.GetPendingAsync();

            return Ok(colleges);
        }

        // Approve a college
        [HttpPut("{id:int}/approve")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var college = await _collegeService.ApproveAsync(id);

                if (college == null)
                {
                    return NotFound(new
                    {
                        message = "College not found."
                    });
                }

                return Ok(new
                {
                    message = "College approved successfully.",
                    college
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // Reject a college
        [HttpPut("{id:int}/reject")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                var college = await _collegeService.RejectAsync(id);

                if (college == null)
                {
                    return NotFound(new
                    {
                        message = "College not found."
                    });
                }

                return Ok(new
                {
                    message = "College rejected successfully.",
                    college
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // Suspend an approved college
        [HttpPut("{id:int}/suspend")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> Suspend(int id)
        {
            try
            {
                var college = await _collegeService.SuspendAsync(id);

                if (college == null)
                {
                    return NotFound(new
                    {
                        message = "College not found."
                    });
                }

                return Ok(new
                {
                    message = "College suspended successfully.",
                    college
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
