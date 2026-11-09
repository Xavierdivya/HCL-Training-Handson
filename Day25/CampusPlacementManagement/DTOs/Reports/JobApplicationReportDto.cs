namespace CampusPlacementManagement.DTOs.Reports
{
    public class JobApplicationReportDto
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;

        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;

        public string JobStatus { get; set; } = string.Empty;

        public int TotalApplications { get; set; }
        public int SubmittedApplications { get; set; }
        public int UnderReviewApplications { get; set; }
        public int ShortlistedApplications { get; set; }
        public int RejectedApplications { get; set; }
        public int SelectedApplications { get; set; }
        public int WithdrawnApplications { get; set; }

        public int TotalInterviews { get; set; }
    }
}