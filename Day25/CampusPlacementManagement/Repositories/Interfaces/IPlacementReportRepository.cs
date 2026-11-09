
using CampusPlacementManagement.DTOs.Reports;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IPlacementReportRepository
    {
        Task<PlacementSummaryDto> GetSummaryAsync();

        Task<IEnumerable<CollegePlacementReportDto>>
            GetAllCollegeReportsAsync();

        Task<CollegePlacementReportDto?>
            GetCollegeReportAsync(int collegeId);

        Task<IEnumerable<CompanyRecruitmentReportDto>>
            GetAllCompanyReportsAsync();

        Task<CompanyRecruitmentReportDto?>
            GetCompanyReportAsync(int companyId);

        Task<IEnumerable<JobApplicationReportDto>>
            GetJobApplicationReportsAsync();
    }
}
