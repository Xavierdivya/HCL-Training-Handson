
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services
{
    public class LeaveApprovalService
    {
        private readonly ApplicationDbContext _context;

        public LeaveApprovalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveRequest>> GetPendingRequestsAsync()
        {
            return await _context.LeaveRequests
                .AsNoTracking()
                .Where(lr => lr.Status == LeaveStatus.Pending)
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .OrderBy(lr => lr.RequestedOn)
                .ToListAsync();
        }

        public async Task<bool> UpdateRequestStatusAsync(
            int requestId, LeaveStatus newStatus)
        {
            if (newStatus != LeaveStatus.Approved &&
                newStatus != LeaveStatus.Rejected)
            {
                return false;
            }

            var request = await _context.LeaveRequests
                .FirstOrDefaultAsync(lr =>
                    lr.LeaveRequestId == requestId);

            if (request == null || request.Status != LeaveStatus.Pending)
            {
                return false;
            }

            request.Status = newStatus;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
