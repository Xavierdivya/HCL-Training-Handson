
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Reports;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class PlacementReportRepository : IPlacementReportRepository
    {
        private readonly ApplicationDbContext _context;

        public PlacementReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PlacementSummaryDto> GetSummaryAsync()
        {
            var totalStudents = await _context.Students.CountAsync();

            var activeStudents = await _context.Students.CountAsync(
                s => s.Status == StudentStatus.Active);

            var placedStudents = await _context.JobApplications
                .Where(a => a.Status == ApplicationStatus.Selected)
                .Select(a => a.StudentId)
                .Distinct()
                .CountAsync();

            var placementPercentage = activeStudents == 0
                ? 0
                : Math.Round((decimal)placedStudents / activeStudents * 100, 2);

            return new PlacementSummaryDto
            {
                TotalColleges = await _context.Colleges.CountAsync(),
                TotalCompanies = await _context.Companies.CountAsync(),
                TotalStudents = totalStudents,
                ActiveStudents = activeStudents,
                TotalJobs = await _context.Jobs.CountAsync(),

                OpenJobs = await _context.Jobs.CountAsync(
                    j => j.Status == JobStatus.Open),

                TotalApplications = await _context.JobApplications.CountAsync(),

                SubmittedApplications = await _context.JobApplications.CountAsync(
                    a => a.Status == ApplicationStatus.Submitted),

                UnderReviewApplications = await _context.JobApplications.CountAsync(
                    a => a.Status == ApplicationStatus.UnderReview),

                ShortlistedApplications = await _context.JobApplications.CountAsync(
                    a => a.Status == ApplicationStatus.Shortlisted),

                RejectedApplications = await _context.JobApplications.CountAsync(
                    a => a.Status == ApplicationStatus.Rejected),

                SelectedApplications = await _context.JobApplications.CountAsync(
                    a => a.Status == ApplicationStatus.Selected),

                WithdrawnApplications = await _context.JobApplications.CountAsync(
                    a => a.Status == ApplicationStatus.Withdrawn),

                TotalPlacementDrives = await _context.PlacementDrives.CountAsync(),

                TotalInterviews = await _context.InterviewSchedules.CountAsync(),

                ScheduledInterviews = await _context.InterviewSchedules.CountAsync(
                    i => i.Status == InterviewStatus.Scheduled ||
                         i.Status == InterviewStatus.Rescheduled),

                CompletedInterviews = await _context.InterviewSchedules.CountAsync(
                    i => i.Status == InterviewStatus.Completed),

                PlacedStudents = placedStudents,
                PlacementPercentage = placementPercentage
            };
        }

        public async Task<CollegePlacementReportDto?> GetCollegeReportAsync(
            int collegeId)
        {
            var college = await _context.Colleges
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CollegeId == collegeId);

            if (college == null)
                return null;

            var students = _context.Students
                .Where(s => s.CollegeId == collegeId);

            var applications = _context.JobApplications
                .Where(a => a.Student.CollegeId == collegeId);

            var totalStudents = await students.CountAsync();

            var activeStudents = await students.CountAsync(
                s => s.Status == StudentStatus.Active);

            var studentsWhoApplied = await applications
                .Select(a => a.StudentId)
                .Distinct()
                .CountAsync();

            var placedStudents = await applications
                .Where(a => a.Status == ApplicationStatus.Selected)
                .Select(a => a.StudentId)
                .Distinct()
                .CountAsync();

            var percentage = activeStudents == 0
                ? 0
                : Math.Round((decimal)placedStudents / activeStudents * 100, 2);

            return new CollegePlacementReportDto
            {
                CollegeId = college.CollegeId,
                CollegeName = college.CollegeName,
                TotalStudents = totalStudents,
                ActiveStudents = activeStudents,
                StudentsWhoApplied = studentsWhoApplied,

                TotalApplications = await applications.CountAsync(),

                ShortlistedApplications = await applications.CountAsync(
                    a => a.Status == ApplicationStatus.Shortlisted),

                RejectedApplications = await applications.CountAsync(
                    a => a.Status == ApplicationStatus.Rejected),

                SelectedApplications = await applications.CountAsync(
                    a => a.Status == ApplicationStatus.Selected),

                PlacedStudents = placedStudents,

                TotalPlacementDrives = await _context.PlacementDrives.CountAsync(
                    d => d.CollegeId == collegeId),

                PlacementPercentage = percentage
            };
        }

        public async Task<CompanyRecruitmentReportDto?> GetCompanyReportAsync(
            int companyId)
        {
            var company = await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CompanyId == companyId);

            if (company == null)
                return null;

            var jobs = _context.Jobs
                .Where(j => j.CompanyId == companyId);

            var applications = _context.JobApplications
                .Where(a => a.Job.CompanyId == companyId);

            var interviews = _context.InterviewSchedules
                .Where(i => i.CompanyId == companyId);

            return new CompanyRecruitmentReportDto
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,

                TotalJobs = await jobs.CountAsync(),

                OpenJobs = await jobs.CountAsync(
                    j => j.Status == JobStatus.Open),

                TotalApplications = await applications.CountAsync(),

                SubmittedApplications = await applications.CountAsync(
                    a => a.Status == ApplicationStatus.Submitted),

                UnderReviewApplications = await applications.CountAsync(
                    a => a.Status == ApplicationStatus.UnderReview),

                ShortlistedApplications = await applications.CountAsync(
                    a => a.Status == ApplicationStatus.Shortlisted),

                RejectedApplications = await applications.CountAsync(
                    a => a.Status == ApplicationStatus.Rejected),

                SelectedApplications = await applications.CountAsync(
                    a => a.Status == ApplicationStatus.Selected),

                TotalInterviews = await interviews.CountAsync(),

                CompletedInterviews = await interviews.CountAsync(
                    i => i.Status == InterviewStatus.Completed)
            };
        }

        public async Task<IEnumerable<CollegePlacementReportDto>>
            GetAllCollegeReportsAsync()
        {
            var collegeIds = await _context.Colleges
                .AsNoTracking()
                .Select(c => c.CollegeId)
                .ToListAsync();

            var reports = new List<CollegePlacementReportDto>();

            foreach (var collegeId in collegeIds)
            {
                var report = await GetCollegeReportAsync(collegeId);

                if (report != null)
                    reports.Add(report);
            }

            return reports;
        }

        public async Task<IEnumerable<CompanyRecruitmentReportDto>>
            GetAllCompanyReportsAsync()
        {
            var companyIds = await _context.Companies
                .AsNoTracking()
                .Select(c => c.CompanyId)
                .ToListAsync();

            var reports = new List<CompanyRecruitmentReportDto>();

            foreach (var companyId in companyIds)
            {
                var report = await GetCompanyReportAsync(companyId);

                if (report != null)
                    reports.Add(report);
            }

            return reports;
        }

        public async Task<IEnumerable<JobApplicationReportDto>>
            GetJobApplicationReportsAsync()
        {
            return await _context.Jobs
                .AsNoTracking()
                .Select(job => new JobApplicationReportDto
                {
                    JobId = job.JobId,
                    JobTitle = job.JobTitle,
                    CompanyId = job.CompanyId,
                    CompanyName = job.Company.CompanyName,

                    TotalApplications = _context.JobApplications.Count(
                        a => a.JobId == job.JobId),

                    SubmittedApplications = _context.JobApplications.Count(
                        a => a.JobId == job.JobId &&
                             a.Status == ApplicationStatus.Submitted),

                    UnderReviewApplications = _context.JobApplications.Count(
                        a => a.JobId == job.JobId &&
                             a.Status == ApplicationStatus.UnderReview),

                    ShortlistedApplications = _context.JobApplications.Count(
                        a => a.JobId == job.JobId &&
                             a.Status == ApplicationStatus.Shortlisted),

                    RejectedApplications = _context.JobApplications.Count(
                        a => a.JobId == job.JobId &&
                             a.Status == ApplicationStatus.Rejected),

                    SelectedApplications = _context.JobApplications.Count(
                        a => a.JobId == job.JobId &&
                             a.Status == ApplicationStatus.Selected),

                    WithdrawnApplications = _context.JobApplications.Count(
                        a => a.JobId == job.JobId &&
                             a.Status == ApplicationStatus.Withdrawn)
                })
                .OrderBy(r => r.JobTitle)
                .ToListAsync();
        }
    }
}
