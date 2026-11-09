using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models
{
    public class Job
    {
        public int JobId { get; set; }

        // Company that posted the job
        public int CompanyId { get; set; }

        // Job details
        public string JobTitle { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string EmploymentType { get; set; } = string.Empty;

        // Student eligibility criteria
        public decimal MinimumCGPA { get; set; }

        public int MaximumActiveBacklogs { get; set; }

        public string EligibleDepartments { get; set; } = string.Empty;

        public string EligibleDegrees { get; set; } = string.Empty;

        public int GraduationYearFrom { get; set; }

        public int GraduationYearTo { get; set; }

        // Application details
        public DateTime ApplicationDeadline { get; set; }

        // Job lifecycle status
        public JobStatus Status { get; set; } = JobStatus.Draft;

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public Company Company { get; set; } = null!;
    }
}