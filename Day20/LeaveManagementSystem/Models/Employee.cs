
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Department { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
            = new List<LeaveRequest>();
    }
}
