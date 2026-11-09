
using CampusPlacementManagement.DTOs.Reports;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class PlacementReportService : IPlacementReportService
    {
        private readonly IPlacementReportRepository _reportRepository;
        private readonly IPlacementOfficerRepository _placementOfficerRepository;
        private readonly ICompanyHRRepository _companyHRRepository;

        public PlacementReportService(
            IPlacementReportRepository reportRepository,
            IPlacementOfficerRepository placementOfficerRepository,
            ICompanyHRRepository companyHRRepository)
        {
            _reportRepository = reportRepository;
            _placementOfficerRepository = placementOfficerRepository;
            _companyHRRepository = companyHRRepository;
        }

        public async Task<PlacementSummaryDto> GetSummaryAsync()
        {
            return await _reportRepository.GetSummaryAsync();
        }

        public async Task<IEnumerable<CollegePlacementReportDto>>
            GetAllCollegeReportsAsync()
        {
            return await _reportRepository.GetAllCollegeReportsAsync();
        }

        public async Task<CollegePlacementReportDto?>
            GetCollegeReportAsync(int collegeId)
        {
            return await _reportRepository.GetCollegeReportAsync(collegeId);
        }

        public async Task<CollegePlacementReportDto?>
            GetMyCollegeReportAsync(string placementOfficerUserId)
        {
            var officer =
                await _placementOfficerRepository.GetByUserIdAsync(
                    placementOfficerUserId);

            if (officer == null)
            {
                throw new UnauthorizedAccessException(
                    "Placement Officer profile not found.");
            }

            if (officer.User.AccountStatus != AccountStatus.Approved ||
                officer.College.Status != CollegeStatus.Approved)
            {
                throw new UnauthorizedAccessException(
                    "Your account or college is not approved.");
            }

            return await _reportRepository.GetCollegeReportAsync(
                officer.CollegeId);
        }

        public async Task<IEnumerable<CompanyRecruitmentReportDto>>
            GetAllCompanyReportsAsync()
        {
            return await _reportRepository.GetAllCompanyReportsAsync();
        }

        public async Task<CompanyRecruitmentReportDto?>
            GetCompanyReportAsync(int companyId)
        {
            return await _reportRepository.GetCompanyReportAsync(companyId);
        }

        public async Task<CompanyRecruitmentReportDto?>
            GetMyCompanyReportAsync(string companyHRUserId)
        {
            var companyHR =
                await _companyHRRepository.GetByUserIdAsync(
                    companyHRUserId);

            if (companyHR == null)
            {
                throw new UnauthorizedAccessException(
                    "Company HR profile not found.");
            }

            if (companyHR.User.AccountStatus != AccountStatus.Approved ||
                companyHR.Company.Status != CompanyStatus.Approved)
            {
                throw new UnauthorizedAccessException(
                    "Your account or company is not approved.");
            }

            return await _reportRepository.GetCompanyReportAsync(
                companyHR.CompanyId);
        }

        public async Task<IEnumerable<JobApplicationReportDto>>
            GetJobReportsAsync()
        {
            return await _reportRepository.GetJobApplicationReportsAsync();
        }
    }
}
