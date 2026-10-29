
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services
{
    public class LeaveTypeService
    {
        private readonly ApplicationDbContext _context;

        public LeaveTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task AddLeaveTypeAsync(LeaveType leaveType)
        {
            await _context.LeaveTypes.AddAsync(leaveType);
            await _context.SaveChangesAsync();
        }

        // READ ALL
        public async Task<List<LeaveType>> GetAllLeaveTypesAsync()
        {
            return await _context.LeaveTypes
                .AsNoTracking()
                .OrderBy(lt => lt.LeaveTypeId)
                .ToListAsync();
        }

        // READ BY ID
        public async Task<LeaveType?> GetLeaveTypeByIdAsync(int id)
        {
            return await _context.LeaveTypes.FindAsync(id);
        }

        // UPDATE
        public async Task<bool> UpdateLeaveTypeAsync(
            int id, string name, int maximumDays)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType == null)
                return false;

            leaveType.LeaveTypeName = name;
            leaveType.MaximumDays = maximumDays;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> DeleteLeaveTypeAsync(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType == null)
                return false;

            bool hasLeaveRequests = await _context.LeaveRequests
                .AnyAsync(lr => lr.LeaveTypeId == id);

            if (hasLeaveRequests)
                return false;

            _context.LeaveTypes.Remove(leaveType);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
