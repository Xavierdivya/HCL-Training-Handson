
using System.Security.Claims;
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacementOfficersController : ControllerBase
    {
        private readonly IPlacementOfficerService _service;

        public PlacementOfficersController(
            IPlacementOfficerService service)
        {
            _service = service;
        }

        // Get all placement officers (SuperAdmin only).
        [HttpGet]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> GetAll()
        {
            var officers = await _service.GetAllAsync();

            return Ok(officers);
        }

        // Get placement officer by ID (SuperAdmin only).
        [HttpGet("{id:int}")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> GetById(int id)
        {
            var officer = await _service.GetByIdAsync(id);

            if (officer == null)
            {
                return NotFound(new
                {
                    message = "Placement officer not found."
                });
            }

            return Ok(officer);
        }

        // Get the logged-in placement officer's profile.
        [HttpGet("me")]
        [Authorize(Roles = AppRoles.PlacementOfficer)]
        public async Task<IActionResult> GetCurrentOfficer()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var officer = await _service.GetByUserIdAsync(userId);

            if (officer == null)
            {
                return NotFound(new
                {
                    message = "Placement officer profile not found."
                });
            }

            return Ok(officer);
        }
    }
}
