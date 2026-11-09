
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Jobs;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services.Implementations
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly ApplicationDbContext _context;

        public JobService(
            IJobRepository jobRepository,
            ApplicationDbContext context)
        {
            _jobRepository = jobRepository;
            _context = context;
        }

        public async Task<JobResponseDto> CreateAsync(
            int companyId,
            JobCreateDto dto)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.CompanyId == companyId);

            if (company == null)
                throw new InvalidOperationException("Company not found.");

            if (company.Status != CompanyStatus.Approved)
                throw new InvalidOperationException(
                    "Only approved companies can create job openings.");

            ValidateJobDetails(
                dto.MinimumCGPA,
                dto.MaximumActiveBacklogs,
                dto.GraduationYearFrom,
                dto.GraduationYearTo,
                dto.ApplicationDeadline);

            var job = new Job
            {
                CompanyId = companyId,
                JobTitle = dto.JobTitle.Trim(),
                JobDescription = dto.JobDescription.Trim(),
                Location = dto.Location.Trim(),
                EmploymentType = dto.EmploymentType.Trim(),
                MinimumCGPA = dto.MinimumCGPA,
                MaximumActiveBacklogs = dto.MaximumActiveBacklogs,
                EligibleDepartments = dto.EligibleDepartments.Trim(),
                EligibleDegrees = dto.EligibleDegrees.Trim(),
                GraduationYearFrom = dto.GraduationYearFrom,
                GraduationYearTo = dto.GraduationYearTo,
                ApplicationDeadline = dto.ApplicationDeadline,
                Status = JobStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            await _jobRepository.AddAsync(job);
            await _jobRepository.SaveChangesAsync();

            job.Company = company;

            return MapToDto(job);
        }

        public async Task<JobResponseDto?> UpdateAsync(
            int jobId,
            int companyId,
            JobUpdateDto dto)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
                return null;

            if (job.CompanyId != companyId)
                throw new UnauthorizedAccessException(
                    "You cannot update another company's job.");

            ValidateJobDetails(
                dto.MinimumCGPA,
                dto.MaximumActiveBacklogs,
                dto.GraduationYearFrom,
                dto.GraduationYearTo,
                dto.ApplicationDeadline);

            if (!Enum.IsDefined(typeof(JobStatus), dto.Status))
                throw new ArgumentException("Invalid job status.");

            job.JobTitle = dto.JobTitle.Trim();
            job.JobDescription = dto.JobDescription.Trim();
            job.Location = dto.Location.Trim();
            job.EmploymentType = dto.EmploymentType.Trim();
            job.MinimumCGPA = dto.MinimumCGPA;
            job.MaximumActiveBacklogs = dto.MaximumActiveBacklogs;
            job.EligibleDepartments = dto.EligibleDepartments.Trim();
            job.EligibleDegrees = dto.EligibleDegrees.Trim();
            job.GraduationYearFrom = dto.GraduationYearFrom;
            job.GraduationYearTo = dto.GraduationYearTo;
            job.ApplicationDeadline = dto.ApplicationDeadline;
            job.Status = dto.Status;
            job.UpdatedAt = DateTime.UtcNow;

            await _jobRepository.SaveChangesAsync();

            return MapToDto(job);
        }

        public async Task<IEnumerable<JobResponseDto>> GetAllAsync()
        {
            var jobs = await _jobRepository.GetAllAsync();

            return jobs.Select(MapToDto);
        }

        public async Task<IEnumerable<JobResponseDto>> GetCompanyJobsAsync(
            int companyId)
        {
            var jobs = await _jobRepository.GetByCompanyIdAsync(companyId);

            return jobs.Select(MapToDto);
        }

        public async Task<IEnumerable<JobResponseDto>> GetOpenJobsAsync()
        {
            var jobs = await _jobRepository.GetOpenJobsAsync();

            return jobs.Select(MapToDto);
        }

        public async Task<JobResponseDto?> GetByIdAsync(int jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            return job == null ? null : MapToDto(job);
        }

        private static void ValidateJobDetails(
            decimal minimumCGPA,
            int maximumActiveBacklogs,
            int graduationYearFrom,
            int graduationYearTo,
            DateTime applicationDeadline)
        {
            if (minimumCGPA < 0 || minimumCGPA > 10)
                throw new ArgumentException(
                    "Minimum CGPA must be between 0 and 10.");

            if (maximumActiveBacklogs < 0)
                throw new ArgumentException(
                    "Maximum active backlogs cannot be negative.");

            if (graduationYearFrom > graduationYearTo)
                throw new ArgumentException(
                    "Graduation year starting value cannot exceed ending value.");

            if (applicationDeadline <= DateTime.UtcNow)
                throw new ArgumentException(
                    "Application deadline must be in the future.");
        }

        private static JobResponseDto MapToDto(Job job)
        {
            return new JobResponseDto
            {
                JobId = job.JobId,
                CompanyId = job.CompanyId,
                CompanyName = job.Company?.CompanyName ?? string.Empty,
                JobTitle = job.JobTitle,
                JobDescription = job.JobDescription,
                Location = job.Location,
                EmploymentType = job.EmploymentType,
                MinimumCGPA = job.MinimumCGPA,
                MaximumActiveBacklogs = job.MaximumActiveBacklogs,
                EligibleDepartments = job.EligibleDepartments,
                EligibleDegrees = job.EligibleDegrees,
                GraduationYearFrom = job.GraduationYearFrom,
                GraduationYearTo = job.GraduationYearTo,
                ApplicationDeadline = job.ApplicationDeadline,
                Status = job.Status,
                CreatedAt = job.CreatedAt,
                UpdatedAt = job.UpdatedAt
            };
        }
    }
}
