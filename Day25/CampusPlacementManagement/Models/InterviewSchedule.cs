using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models
{
    public class InterviewSchedule
    {
        public int InterviewScheduleId { get; set; }

        public int JobApplicationId { get; set; }

        public int CompanyId { get; set; }

        public int CompanyHRId { get; set; }

        public DateTime ScheduledAt { get; set; }

        public int DurationMinutes { get; set; }

        public InterviewMode Mode { get; set; }

        public string MeetingDetails { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public InterviewStatus Status { get; set; }
            = InterviewStatus.Scheduled;

        public string? Feedback { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public JobApplication JobApplication { get; set; } = null!;

        public Company Company { get; set; } = null!;

        public CompanyHR CompanyHR { get; set; } = null!;
    }
}