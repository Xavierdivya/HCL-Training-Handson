
using CampusPlacementManagement.DTOs.Reports;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IPlacementReportService
    {
        Task<PlacementSummaryDto> GetSummaryAsync();

        Task<IEnumerable<CollegePlacementReportDto>>
            GetAllCollegeReportsAsync();

        Task<CollegePlacementReportDto?>
            GetCollegeReportAsync(int collegeId);

        Task<CollegePlacementReportDto?>
            GetMyCollegeReportAsync(string placementOfficerUserId);

        Task<IEnumerable<CompanyRecruitmentReportDto>>
            GetAllCompanyReportsAsync();

        Task<CompanyRecruitmentReportDto?>
            GetCompanyReportAsync(int companyId);

        Task<CompanyRecruitmentReportDto?>
            GetMyCompanyReportAsync(string companyHRUserId);

        Task<IEnumerable<JobApplicationReportDto>>
            GetJobReportsAsync();
    }
}
