
namespace CampusPlacementManagement.DTOs.Reports
{
    public class CollegePlacementReportDto
    {
        public int CollegeId { get; set; }

        public string CollegeName { get; set; } = string.Empty;

        public int TotalStudents { get; set; }

        public int ActiveStudents { get; set; }

        public int StudentsWhoApplied { get; set; }

        public int TotalApplications { get; set; }

        public int ShortlistedApplications { get; set; }

        public int SelectedApplications { get; set; }

        public int RejectedApplications { get; set; }

        public int TotalPlacementDrives { get; set; }

        public int PlacedStudents { get; set; }

        public decimal PlacementPercentage { get; set; }
    }
}
