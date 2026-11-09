
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.PlacementDrives
{
    public class PlacementDriveResponseDto
    {
        public int PlacementDriveId { get; set; }

        public int CollegeId { get; set; }

        public string CollegeName { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public int PlacementOfficerId { get; set; }

        public string PlacementOfficerName { get; set; } = string.Empty;

        public string DriveName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Venue { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime RegistrationDeadline { get; set; }

        public decimal MinimumCGPA { get; set; }

        public int MaximumActiveBacklogs { get; set; }

        public string EligibleDepartments { get; set; } = string.Empty;

        public string EligibleDegrees { get; set; } = string.Empty;

        public int GraduationYearFrom { get; set; }

        public int GraduationYearTo { get; set; }

        public PlacementDriveStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
