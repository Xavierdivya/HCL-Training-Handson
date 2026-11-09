
using CampusPlacementManagement.DTOs.JobApplications;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _applicationRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IStudentRepository _studentRepository;

        public JobApplicationService(
            IJobApplicationRepository applicationRepository,
            IJobRepository jobRepository,
            IStudentRepository studentRepository)
        {
            _applicationRepository = applicationRepository;
            _jobRepository = jobRepository;
            _studentRepository = studentRepository;
        }

        // Student applies for a job.
        public async Task<JobApplicationResponseDto> ApplyAsync(
            string studentUserId,
            JobApplicationCreateDto dto)
        {
            // Find student profile.
            var student = await _studentRepository.GetByUserIdAsync(
                studentUserId);

            if (student == null)
            {
                throw new InvalidOperationException(
                    "Student profile not found.");
            }

            // Verify student account approval.
            if (student.User.AccountStatus != AccountStatus.Approved)
            {
                throw new InvalidOperationException(
                    "Your account is not approved.");
            }

            if (student.Status != StudentStatus.Active)
            {
                throw new InvalidOperationException(
                    "Only active students can apply for jobs.");
            }

            // Find job.
            var job = await _jobRepository.GetByIdAsync(dto.JobId);

            if (job == null)
            {
                throw new InvalidOperationException(
                    "Job not found.");
            }

            // Verify job availability.
            if (job.Status != JobStatus.Open)
            {
                throw new InvalidOperationException(
                    "Applications are not open for this job.");
            }

            if (job.ApplicationDeadline <= DateTime.UtcNow)
            {
                throw new InvalidOperationException(
                    "The application deadline has passed.");
            }

            // Verify company approval.
            if (job.Company.Status != CompanyStatus.Approved)
            {
                throw new InvalidOperationException(
                    "This company's jobs are not currently available.");
            }

            // Check CGPA eligibility.
            if (student.CGPA < job.MinimumCGPA)
            {
                throw new InvalidOperationException(
                    $"Minimum required CGPA is {job.MinimumCGPA}.");
            }

            // Check active backlogs.
            if (student.ActiveBacklogs > job.MaximumActiveBacklogs)
            {
                throw new InvalidOperationException(
                    "You do not meet the active backlog requirement.");
            }

            // Check department eligibility.
            var eligibleDepartments = job.EligibleDepartments
                .Split(',', StringSplitOptions.RemoveEmptyEntries |
                            StringSplitOptions.TrimEntries);

            if (!eligibleDepartments.Any(d =>
                d.Equals(
                    student.Department,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "Your department is not eligible for this job.");
            }

            // Check degree eligibility.
            var eligibleDegrees = job.EligibleDegrees
                .Split(',', StringSplitOptions.RemoveEmptyEntries |
                            StringSplitOptions.TrimEntries);

            if (!eligibleDegrees.Any(d =>
                d.Equals(
                    student.Degree,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "Your degree is not eligible for this job.");
            }

            // Check graduation year.
            if (student.GraduationYear < job.GraduationYearFrom ||
                student.GraduationYear > job.GraduationYearTo)
            {
                throw new InvalidOperationException(
                    "Your graduation year is not eligible for this job.");
            }

            // Prevent duplicate applications.
            var alreadyApplied =
                await _applicationRepository.ExistsAsync(
                    dto.JobId,
                    student.StudentId);

            if (alreadyApplied)
            {
                throw new InvalidOperationException(
                    "You have already applied for this job.");
            }

            // Create application.
            var application = new JobApplication
            {
                JobId = job.JobId,
                StudentId = student.StudentId,
                CoverLetter = dto.CoverLetter?.Trim(),
                Status = ApplicationStatus.Submitted,
                AppliedAt = DateTime.UtcNow
            };

            await _applicationRepository.AddAsync(application);
            await _applicationRepository.SaveChangesAsync();

            // Return response.
            return MapToDto(application, job, student);
        }

        // SuperAdmin views all applications.
        public async Task<IEnumerable<JobApplicationResponseDto>> GetAllAsync()
        {
            var applications =
                await _applicationRepository.GetAllAsync();

            return applications.Select(MapToDto);
        }

        // Student views their own applications.
        public async Task<IEnumerable<JobApplicationResponseDto>>
            GetByStudentUserIdAsync(string studentUserId)
        {
            var student = await _studentRepository.GetByUserIdAsync(
                studentUserId);

            if (student == null)
            {
                return Enumerable.Empty<JobApplicationResponseDto>();
            }

            var applications =
                await _applicationRepository.GetByStudentIdAsync(
                    student.StudentId);

            return applications.Select(MapToDto);
        }

        // Company HR views applications for their company.
        public async Task<IEnumerable<JobApplicationResponseDto>>
            GetByCompanyIdAsync(int companyId)
        {
            var applications =
                await _applicationRepository.GetByCompanyIdAsync(companyId);

            return applications.Select(MapToDto);
        }

        // Placement Officer views applications from their college.
        public async Task<IEnumerable<JobApplicationResponseDto>>
            GetByCollegeIdAsync(int collegeId)
        {
            var applications =
                await _applicationRepository.GetByCollegeIdAsync(collegeId);

            return applications.Select(MapToDto);
        }

        // Retrieve a specific application.
        public async Task<JobApplicationResponseDto?> GetByIdAsync(
            int applicationId)
        {
            var application =
                await _applicationRepository.GetByIdAsync(applicationId);

            return application == null
                ? null
                : MapToDto(application);
        }

        // Company HR updates application status.
        public async Task<JobApplicationResponseDto?> UpdateStatusAsync(
            int applicationId,
            int companyId,
            JobApplicationStatusUpdateDto dto)
        {
            var application =
                await _applicationRepository.GetByIdAsync(applicationId);

            if (application == null)
            {
                return null;
            }

            // Verify company ownership.
            if (application.Job.CompanyId != companyId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot update applications for another company.");
            }

            // HR cannot set Submitted or Withdrawn.
            if (!Enum.IsDefined(typeof(ApplicationStatus), dto.Status) ||
                dto.Status == ApplicationStatus.Submitted ||
                dto.Status == ApplicationStatus.Withdrawn)
            {
                throw new InvalidOperationException(
                    "Invalid application status update.");
            }

            // Do not modify withdrawn applications.
            if (application.Status == ApplicationStatus.Withdrawn)
            {
                throw new InvalidOperationException(
                    "Withdrawn applications cannot be updated.");
            }

            application.Status = dto.Status;
            application.Remarks = dto.Remarks?.Trim();
            application.UpdatedAt = DateTime.UtcNow;

            await _applicationRepository.SaveChangesAsync();

            return MapToDto(application);
        }

        // Map entity to response DTO.
        private static JobApplicationResponseDto MapToDto(
            JobApplication application)
        {
            return new JobApplicationResponseDto
            {
                JobApplicationId = application.JobApplicationId,

                JobId = application.JobId,
                JobTitle = application.Job.JobTitle,

                CompanyId = application.Job.CompanyId,
                CompanyName = application.Job.Company.CompanyName,

                StudentId = application.StudentId,
                StudentName = application.Student.User.FullName,
                StudentEmail = application.Student.User.Email ?? string.Empty,
                RegisterNumber = application.Student.RegisterNumber,
                Department = application.Student.Department,
                Degree = application.Student.Degree,
                CGPA = application.Student.CGPA,
                GraduationYear = application.Student.GraduationYear,

                Status = application.Status,
                AppliedAt = application.AppliedAt,
                UpdatedAt = application.UpdatedAt,
                CoverLetter = application.CoverLetter,
                Remarks = application.Remarks
            };
        }

        // Map newly created application.
        private static JobApplicationResponseDto MapToDto(
            JobApplication application,
            Job job,
            Student student)
        {
            return new JobApplicationResponseDto
            {
                JobApplicationId = application.JobApplicationId,

                JobId = job.JobId,
                JobTitle = job.JobTitle,

                CompanyId = job.CompanyId,
                CompanyName = job.Company.CompanyName,

                StudentId = student.StudentId,
                StudentName = student.User.FullName,
                StudentEmail = student.User.Email ?? string.Empty,
                RegisterNumber = student.RegisterNumber,
                Department = student.Department,
                Degree = student.Degree,
                CGPA = student.CGPA,
                GraduationYear = student.GraduationYear,

                Status = application.Status,
                AppliedAt = application.AppliedAt,
                UpdatedAt = application.UpdatedAt,
                CoverLetter = application.CoverLetter,
                Remarks = application.Remarks,
                CollegeId = application.Student.CollegeId,
            };
        }
    }
}
