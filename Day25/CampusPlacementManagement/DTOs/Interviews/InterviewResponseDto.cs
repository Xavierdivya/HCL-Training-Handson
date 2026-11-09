using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.Interviews
{
    public class InterviewResponseDto
    {
        public int InterviewScheduleId { get; set; }

        public int JobApplicationId { get; set; }

        public int JobId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public int CompanyHRId { get; set; }

        public string HRName { get; set; } = string.Empty;

        public int StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public string StudentEmail { get; set; } = string.Empty;

        public string RegisterNumber { get; set; } = string.Empty;

        public int CollegeId { get; set; }

        public DateTime ScheduledAt { get; set; }

        public int DurationMinutes { get; set; }

        public InterviewMode Mode { get; set; }

        public string MeetingDetails { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public InterviewStatus Status { get; set; }

        public string? Feedback { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}