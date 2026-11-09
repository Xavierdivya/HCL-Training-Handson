using CampusPlacementManagement.DTOs.Interviews;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IJobApplicationRepository _applicationRepository;
        private readonly ICompanyHRRepository _companyHRRepository;
        private readonly IStudentRepository _studentRepository;

        public InterviewService(
            IInterviewRepository interviewRepository,
            IJobApplicationRepository applicationRepository,
            ICompanyHRRepository companyHRRepository,
            IStudentRepository studentRepository)
        {
            _interviewRepository = interviewRepository;
            _applicationRepository = applicationRepository;
            _companyHRRepository = companyHRRepository;
            _studentRepository = studentRepository;
        }

        public async Task<InterviewResponseDto> CreateAsync(
            string companyHRUserId,
            InterviewCreateDto dto)
        {
            var companyHR =
                await _companyHRRepository.GetByUserIdAsync(
                    companyHRUserId);

            if (companyHR == null)
            {
                throw new UnauthorizedAccessException(
                    "Company HR profile not found.");
            }

            if (companyHR.User.AccountStatus != AccountStatus.Approved)
            {
                throw new InvalidOperationException(
                    "Your account is not approved.");
            }

            if (companyHR.Company.Status != CompanyStatus.Approved)
            {
                throw new InvalidOperationException(
                    "Your company is not approved.");
            }

            var application =
                await _applicationRepository.GetByIdAsync(
                    dto.JobApplicationId);

            if (application == null)
            {
                throw new KeyNotFoundException(
                    "Job application not found.");
            }

            if (application.Job.CompanyId != companyHR.CompanyId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot schedule interviews for another company's applications.");
            }

            if (application.Status != ApplicationStatus.Shortlisted)
            {
                throw new InvalidOperationException(
                    "An interview can only be scheduled for a shortlisted application.");
            }

            if (application.Job.Company.Status != CompanyStatus.Approved)
            {
                throw new InvalidOperationException(
                    "The company is not approved.");
            }

            ValidateInterviewDetails(
                dto.ScheduledAt,
                dto.DurationMinutes,
                dto.Mode,
                dto.MeetingDetails);

            var interview = new InterviewSchedule
            {
                JobApplicationId = application.JobApplicationId,
                CompanyId = companyHR.CompanyId,
                CompanyHRId = companyHR.CompanyHRId,
                ScheduledAt = dto.ScheduledAt,
                DurationMinutes = dto.DurationMinutes,
                Mode = dto.Mode,
                MeetingDetails = dto.MeetingDetails.Trim(),
                Notes = dto.Notes?.Trim(),
                Status = InterviewStatus.Scheduled,
                CreatedAt = DateTime.UtcNow
            };

            await _interviewRepository.AddAsync(interview);
            await _interviewRepository.SaveChangesAsync();

            var savedInterview =
                await _interviewRepository.GetByIdAsync(
                    interview.InterviewScheduleId);

            return MapToDto(savedInterview!);
        }

        public async Task<InterviewResponseDto?> UpdateAsync(
            int interviewId,
            string companyHRUserId,
            InterviewUpdateDto dto)
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
                throw new InvalidOperationException(
                    "Your account or company is not approved.");
            }

            var interview =
                await _interviewRepository.GetByIdAsync(interviewId);

            if (interview == null)
                return null;

            if (interview.CompanyHRId != companyHR.CompanyHRId ||
                interview.CompanyId != companyHR.CompanyId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot update another HR's interview.");
            }

            if (interview.Status == InterviewStatus.Completed ||
                interview.Status == InterviewStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "Completed or cancelled interviews cannot be modified.");
            }

            if (!Enum.IsDefined(typeof(InterviewStatus), dto.Status) ||
                !Enum.IsDefined(typeof(InterviewMode), dto.Mode))
            {
                throw new InvalidOperationException(
                    "Invalid interview status or mode.");
            }

            ValidateInterviewDetails(
                dto.ScheduledAt,
                dto.DurationMinutes,
                dto.Mode,
                dto.MeetingDetails);

            interview.ScheduledAt = dto.ScheduledAt;
            interview.DurationMinutes = dto.DurationMinutes;
            interview.Mode = dto.Mode;
            interview.MeetingDetails = dto.MeetingDetails.Trim();
            interview.Notes = dto.Notes?.Trim();
            interview.Status = dto.Status;
            interview.Feedback = dto.Feedback?.Trim();
            interview.UpdatedAt = DateTime.UtcNow;

            await _interviewRepository.SaveChangesAsync();

            return MapToDto(interview);
        }

        public async Task<IEnumerable<InterviewResponseDto>> GetAllAsync()
        {
            var interviews =
                await _interviewRepository.GetAllAsync();

            return interviews.Select(MapToDto);
        }

        public async Task<IEnumerable<InterviewResponseDto>>
            GetByCompanyHRUserIdAsync(string companyHRUserId)
        {
            var companyHR =
                await _companyHRRepository.GetByUserIdAsync(
                    companyHRUserId);

            if (companyHR == null)
            {
                throw new UnauthorizedAccessException(
                    "Company HR profile not found.");
            }

            var interviews =
                await _interviewRepository.GetByCompanyIdAsync(
                    companyHR.CompanyId);

            return interviews.Select(MapToDto);
        }

        public async Task<IEnumerable<InterviewResponseDto>>
            GetByStudentUserIdAsync(string studentUserId)
        {
            var student =
                await _studentRepository.GetByUserIdAsync(studentUserId);

            if (student == null)
            {
                return Enumerable.Empty<InterviewResponseDto>();
            }

            var interviews =
                await _interviewRepository.GetByStudentIdAsync(
                    student.StudentId);

            return interviews.Select(MapToDto);
        }

        public async Task<InterviewResponseDto?> GetByIdAsync(int id)
        {
            var interview =
                await _interviewRepository.GetByIdAsync(id);

            return interview == null
                ? null
                : MapToDto(interview);
        }

        private static void ValidateInterviewDetails(
            DateTime scheduledAt,
            int durationMinutes,
            InterviewMode mode,
            string meetingDetails)
        {
            if (scheduledAt <= DateTime.UtcNow)
            {
                throw new InvalidOperationException(
                    "Interview date and time must be in the future.");
            }

            if (durationMinutes < 15 || durationMinutes > 480)
            {
                throw new InvalidOperationException(
                    "Interview duration must be between 15 and 480 minutes.");
            }

            if (!Enum.IsDefined(typeof(InterviewMode), mode))
            {
                throw new InvalidOperationException(
                    "Invalid interview mode.");
            }

            if (string.IsNullOrWhiteSpace(meetingDetails))
            {
                throw new InvalidOperationException(
                    "Meeting details are required.");
            }
        }

        private static InterviewResponseDto MapToDto(
            InterviewSchedule interview)
        {
            return new InterviewResponseDto
            {
                InterviewScheduleId = interview.InterviewScheduleId,
                JobApplicationId = interview.JobApplicationId,

                JobId = interview.JobApplication.JobId,
                JobTitle = interview.JobApplication.Job.JobTitle,

                CompanyId = interview.CompanyId,
                CompanyName = interview.Company.CompanyName,

                CompanyHRId = interview.CompanyHRId,
                HRName = interview.CompanyHR.User.FullName,

                StudentId = interview.JobApplication.StudentId,
                StudentName = interview.JobApplication.Student.User.FullName,
                StudentEmail =
                    interview.JobApplication.Student.User.Email ?? string.Empty,
                RegisterNumber =
                    interview.JobApplication.Student.RegisterNumber,
                CollegeId =
                    interview.JobApplication.Student.CollegeId,

                ScheduledAt = interview.ScheduledAt,
                DurationMinutes = interview.DurationMinutes,
                Mode = interview.Mode,
                MeetingDetails = interview.MeetingDetails,
                Notes = interview.Notes,
                Status = interview.Status,
                Feedback = interview.Feedback,
                CreatedAt = interview.CreatedAt,
                UpdatedAt = interview.UpdatedAt
            };
        }
    }
}