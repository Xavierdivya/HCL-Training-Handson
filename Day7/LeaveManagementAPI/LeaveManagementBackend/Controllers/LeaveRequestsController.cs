
using LeaveManagementAPI.Data;
using LeaveManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetLeaveRequests(
            [FromQuery] string? status)
        {
            IQueryable<LeaveRequest> query =
                _context.LeaveRequests.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (!new[] { "Pending", "Approved", "Rejected" }
                    .Contains(status, StringComparer.OrdinalIgnoreCase))
                {
                    return BadRequest(
                        "Status must be Pending, Approved, or Rejected.");
                }

                query = query.Where(l => l.Status == status);
            }

            var requests = await query
                .OrderByDescending(l => l.Id)
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LeaveRequest>> GetLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leaveRequest == null)
                return NotFound();

            return Ok(leaveRequest);
        }

        [HttpPost]
        public async Task<ActionResult<LeaveRequest>> CreateLeaveRequest(
            [FromBody] LeaveRequest leaveRequest)
        {
            if (!IsValidDateRange(leaveRequest))
                return BadRequest(
                    "End date cannot be earlier than start date.");

            // New requests always start as Pending.
            leaveRequest.Status = "Pending";

            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetLeaveRequest),
                new { id = leaveRequest.Id },
                leaveRequest);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLeaveRequest(
            int id,
            [FromBody] LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.Id)
                return BadRequest(
                    "URL ID and request ID do not match.");

            if (!IsValidDateRange(leaveRequest))
                return BadRequest(
                    "End date cannot be earlier than start date.");

            var existingRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(l => l.Id == id);

            if (existingRequest == null)
                return NotFound();

            existingRequest.EmployeeName = leaveRequest.EmployeeName;
            existingRequest.LeaveType = leaveRequest.LeaveType;
            existingRequest.StartDate = leaveRequest.StartDate;
            existingRequest.EndDate = leaveRequest.EndDate;
            existingRequest.Reason = leaveRequest.Reason;
            existingRequest.Status = leaveRequest.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leaveRequest == null)
                return NotFound();

            _context.LeaveRequests.Remove(leaveRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static bool IsValidDateRange(LeaveRequest request)
        {
            return request.StartDate.HasValue
                && request.EndDate.HasValue
                && request.EndDate.Value >= request.StartDate.Value;
        }
    }
}
