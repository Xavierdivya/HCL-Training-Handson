using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.Jobs
{
    public class JobResponseDto
    {
        public int JobId { get; set; }

        // Company details
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;

        // Job details
        public string JobTitle { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;

        // Eligibility criteria
        public decimal MinimumCGPA { get; set; }
        public int MaximumActiveBacklogs { get; set; }
        public string EligibleDepartments { get; set; } = string.Empty;
        public string EligibleDegrees { get; set; } = string.Empty;
        public int GraduationYearFrom { get; set; }
        public int GraduationYearTo { get; set; }

        // Application details
        public DateTime ApplicationDeadline { get; set; }
        public JobStatus Status { get; set; }

        // Audit details
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}