
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        // READ: Get all employees
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .OrderBy(e => e.EmployeeId)
                .ToListAsync();
        }

        // READ: Get one employee by ID
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        // UPDATE
        public async Task<bool> UpdateEmployeeAsync(
            int id, string fullName, string email, string? department)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return false;

            employee.FullName = fullName;
            employee.Email = email;
            employee.Department = department;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return false;

            // Preserve employees who have existing leave requests.
            bool hasLeaveRequests = await _context.LeaveRequests
                .AnyAsync(lr => lr.EmployeeId == id);

            if (hasLeaveRequests)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
