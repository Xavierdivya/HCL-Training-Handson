
namespace CampusPlacementManagement.DTOs.Reports
{
    public class CompanyRecruitmentReportDto
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public int TotalJobs { get; set; }

        public int OpenJobs { get; set; }

        public int TotalApplications { get; set; }

        public int SubmittedApplications { get; set; }

        public int UnderReviewApplications { get; set; }

        public int ShortlistedApplications { get; set; }

        public int RejectedApplications { get; set; }

        public int SelectedApplications { get; set; }

        public int TotalInterviews { get; set; }

        public int CompletedInterviews { get; set; }
    }
}
