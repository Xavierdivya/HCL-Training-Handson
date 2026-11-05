
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

        // Public college registration
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            CollegeRegistrationDto registrationDto)
        {
            try
            {
                var college = await _collegeService.RegisterAsync(
                    registrationDto);

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

        // SuperAdmin: Get all colleges
        [HttpGet]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> GetAll()
        {
            var colleges = await _collegeService.GetAllAsync();

            return Ok(colleges);
        }

        // SuperAdmin: Get college by ID
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
    }
}
