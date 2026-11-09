
namespace CampusPlacementManagement.DTOs.Reports
{
    public class PlacementSummaryDto
    {
        public int TotalColleges { get; set; }

        public int TotalCompanies { get; set; }

        public int TotalStudents { get; set; }

        public int ActiveStudents { get; set; }

        public int TotalJobs { get; set; }

        public int OpenJobs { get; set; }

        public int TotalApplications { get; set; }

        public int SubmittedApplications { get; set; }

        public int UnderReviewApplications { get; set; }

        public int ShortlistedApplications { get; set; }

        public int RejectedApplications { get; set; }

        public int SelectedApplications { get; set; }

        public int WithdrawnApplications { get; set; }

        public int TotalPlacementDrives { get; set; }

        public int TotalInterviews { get; set; }

        public int ScheduledInterviews { get; set; }

        public int CompletedInterviews { get; set; }

        public int PlacedStudents { get; set; }

        public decimal PlacementPercentage { get; set; }
    }
}
