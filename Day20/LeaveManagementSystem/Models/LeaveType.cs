
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Models
{
    public class LeaveType
    {
        public int LeaveTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LeaveTypeName { get; set; } = string.Empty;

        public int MaximumDays { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
            = new List<LeaveRequest>();
    }
}
