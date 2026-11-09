
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.JobApplications
{
    public class JobApplicationResponseDto
    {
        public int JobApplicationId { get; set; }

        public int JobId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public int StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public string StudentEmail { get; set; } = string.Empty;

        public string RegisterNumber { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Degree { get; set; } = string.Empty;

        public decimal CGPA { get; set; }

        public int GraduationYear { get; set; }

        public ApplicationStatus Status { get; set; }

        public DateTime AppliedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CoverLetter { get; set; }

        public string? Remarks { get; set; }

        public int CollegeId { get; set; }

    }

}
