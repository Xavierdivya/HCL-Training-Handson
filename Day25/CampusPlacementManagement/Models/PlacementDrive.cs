
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models
{
    public class PlacementDrive
    {
        public int PlacementDriveId { get; set; }

        public int CollegeId { get; set; }

        public int CompanyId { get; set; }

        public int PlacementOfficerId { get; set; }

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
            = PlacementDriveStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public College College { get; set; } = null!;

        public Company Company { get; set; } = null!;

        public PlacementOfficer PlacementOfficer { get; set; } = null!;
    }
}
