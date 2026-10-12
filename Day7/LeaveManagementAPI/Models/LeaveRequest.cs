
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementAPI.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(100)]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Leave type is required.")]
        [StringLength(50)]
        public string LeaveType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Reason is required.")]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            "^(Pending|Approved|Rejected)$",
            ErrorMessage = "Status must be Pending, Approved, or Rejected."
        )]
        public string Status { get; set; } = "Pending";
    }
}
