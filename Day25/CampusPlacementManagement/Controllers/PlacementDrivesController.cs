
using System.Security.Claims;
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.DTOs.PlacementDrives;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlacementDrivesController : ControllerBase
    {
        private readonly IPlacementDriveService _placementDriveService;
        private readonly IPlacementOfficerRepository _placementOfficerRepository;

        public PlacementDrivesController(
            IPlacementDriveService placementDriveService,
            IPlacementOfficerRepository placementOfficerRepository)
        {
            _placementDriveService = placementDriveService;
            _placementOfficerRepository = placementOfficerRepository;
        }

        // Placement Officer creates a drive for their own college.
        [HttpPost]
        [Authorize(Roles = AppRoles.PlacementOfficer)]
        public async Task<ActionResult<PlacementDriveResponseDto>> Create(
            [FromBody] PlacementDriveCreateDto dto)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var drive = await _placementDriveService.CreateAsync(
                    userId,
                    dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = drive.PlacementDriveId },
                    drive);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // SuperAdmin can view all placement drives.
        [HttpGet]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<ActionResult<IEnumerable<PlacementDriveResponseDto>>>
            GetAll()
        {
            var drives = await _placementDriveService.GetAllAsync();

            return Ok(drives);
        }

        // Placement Officer can view drives belonging to their college.
        [HttpGet("college")]
        [Authorize(Roles = AppRoles.PlacementOfficer)]
        public async Task<ActionResult<IEnumerable<PlacementDriveResponseDto>>>
            GetCollegeDrives()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var officer =
                await _placementOfficerRepository.GetByUserIdAsync(userId);

            if (officer == null)
                return Forbid();

            var drives = await _placementDriveService.GetByCollegeIdAsync(
                officer.CollegeId);

            return Ok(drives);
        }

        // Placement Officer can view drives they personally created.
        [HttpGet("mine")]
        [Authorize(Roles = AppRoles.PlacementOfficer)]
        public async Task<ActionResult<IEnumerable<PlacementDriveResponseDto>>>
            GetMyDrives()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var drives = await _placementDriveService.GetMyDrivesAsync(
                userId);

            return Ok(drives);
        }

        // Students and visitors can view currently open placement drives.
        [HttpGet("open")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PlacementDriveResponseDto>>>
            GetOpenDrives()
        {
            var drives = await _placementDriveService.GetOpenDrivesAsync();

            return Ok(drives);
        }

        // SuperAdmin can view any drive.
        // Placement Officers can view drives belonging to their college.
        // Students can view only open drives.
        [HttpGet("{id:int}")]
        [Authorize(Roles =
            AppRoles.SuperAdmin + "," +
            AppRoles.PlacementOfficer + "," +
            AppRoles.Student)]
        public async Task<ActionResult<PlacementDriveResponseDto>> GetById(
            int id)
        {
            var drive = await _placementDriveService.GetByIdAsync(id);

            if (drive == null)
                return NotFound();

            if (User.IsInRole(AppRoles.SuperAdmin))
                return Ok(drive);

            if (User.IsInRole(AppRoles.PlacementOfficer))
            {
                var userId = User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized();

                var officer =
                    await _placementOfficerRepository.GetByUserIdAsync(userId);

                if (officer == null ||
                    officer.CollegeId != drive.CollegeId)
                {
                    return Forbid();
                }

                return Ok(drive);
            }

            if (User.IsInRole(AppRoles.Student))
            {
                var openDrives =
                    await _placementDriveService.GetOpenDrivesAsync();

                var isOpen = openDrives.Any(
                    d => d.PlacementDriveId == id);

                if (!isOpen)
                    return Forbid();

                return Ok(drive);
            }

            return Forbid();
        }

        // Only the Placement Officer who created the drive can update it.
        [HttpPut("{id:int}")]
        [Authorize(Roles = AppRoles.PlacementOfficer)]
        public async Task<ActionResult<PlacementDriveResponseDto>> Update(
            int id,
            [FromBody] PlacementDriveUpdateDto dto)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var updatedDrive =
                    await _placementDriveService.UpdateAsync(
                        id,
                        userId,
                        dto);

                return Ok(updatedDrive);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
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
