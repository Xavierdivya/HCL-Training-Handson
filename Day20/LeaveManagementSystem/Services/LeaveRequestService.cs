
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services
{
    public class LeaveRequestService
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE: Submit a leave request.
        public async Task<(bool Success, string Message)> AddLeaveRequestAsync(
            int employeeId,
            int leaveTypeId,
            DateOnly startDate,
            DateOnly endDate,
            string reason)
        {
            if (endDate < startDate)
            {
                return (false, "End date cannot be before start date.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                return (false, "Reason is required.");
            }

            var employee = await _context.Employees
                .FindAsync(employeeId);

            if (employee == null || !employee.IsActive)
            {
                return (false, "Employee does not exist or is inactive.");
            }

            var leaveType = await _context.LeaveTypes
                .FindAsync(leaveTypeId);

            if (leaveType == null || !leaveType.IsActive)
            {
                return (false, "Leave type does not exist or is inactive.");
            }

            int requestedDays = endDate.DayNumber - startDate.DayNumber + 1;

            if (requestedDays > leaveType.MaximumDays)
            {
                return (false,
                    $"Requested {requestedDays} calendar days, but " +
                    $"this leave type allows at most {leaveType.MaximumDays} days.");
            }

            bool hasOverlappingLeave = await _context.LeaveRequests
    .AnyAsync(lr =>
        lr.EmployeeId == employeeId &&
        (lr.Status == LeaveStatus.Pending ||
         lr.Status == LeaveStatus.Approved) &&
        lr.StartDate <= endDate &&
        lr.EndDate >= startDate);

if (hasOverlappingLeave)
{
    return (false,
        "This employee already has a pending or approved leave " +
        "request that overlaps these dates.");
}

            var leaveRequest = new LeaveRequest
            {
                EmployeeId = employeeId,
                LeaveTypeId = leaveTypeId,
                StartDate = startDate,
                EndDate = endDate,
                Reason = reason.Trim(),
                Status = LeaveStatus.Pending,
                RequestedOn = DateTime.UtcNow
            };

            await _context.LeaveRequests.AddAsync(leaveRequest);
            await _context.SaveChangesAsync();

            return (true,
                $"Leave request submitted successfully. " +
                $"Request ID: {leaveRequest.LeaveRequestId}");
        }

        // READ: Get all leave requests with employee and leave type details.
        public async Task<List<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            return await _context.LeaveRequests
                .AsNoTracking()
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .OrderBy(lr => lr.LeaveRequestId)
                .ToListAsync();
        }

        // READ: Find one leave request by ID.
        public async Task<LeaveRequest?> GetLeaveRequestByIdAsync(int id)
        {
            return await _context.LeaveRequests
                .AsNoTracking()
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .FirstOrDefaultAsync(lr => lr.LeaveRequestId == id);
        }
    }
}
