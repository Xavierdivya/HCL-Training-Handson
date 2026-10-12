
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
            var query = _context.LeaveRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(l => l.Status == status);
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequest>> GetLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);

            if (leaveRequest == null)
                return NotFound();

            return leaveRequest;
        }

        [HttpPost]
        public async Task<ActionResult<LeaveRequest>> CreateLeaveRequest(
            LeaveRequest leaveRequest)
        {
            if (leaveRequest.EndDate < leaveRequest.StartDate)
            {
                ModelState.AddModelError(
                    nameof(leaveRequest.EndDate),
                    "End date cannot be earlier than start date.");

                return ValidationProblem(ModelState);
            }

            leaveRequest.Status = "Pending";

            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetLeaveRequest),
                new { id = leaveRequest.Id },
                leaveRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeaveRequest(
            int id, LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.Id)
                return BadRequest("URL ID and request ID do not match.");

            if (leaveRequest.EndDate < leaveRequest.StartDate)
            {
                ModelState.AddModelError(
                    nameof(leaveRequest.EndDate),
                    "End date cannot be earlier than start date.");

                return ValidationProblem(ModelState);
            }

            var existingRequest = await _context.LeaveRequests.FindAsync(id);

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);

            if (leaveRequest == null)
                return NotFound();

            _context.LeaveRequests.Remove(leaveRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
