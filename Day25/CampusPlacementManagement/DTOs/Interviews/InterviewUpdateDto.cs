using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.Interviews
{
    public class InterviewUpdateDto
    {
        public DateTime ScheduledAt { get; set; }

        [Range(15, 480)]
        public int DurationMinutes { get; set; }

        [EnumDataType(typeof(InterviewMode))]
        public InterviewMode Mode { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string MeetingDetails { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Notes { get; set; }

        [EnumDataType(typeof(InterviewStatus))]
        public InterviewStatus Status { get; set; }

        [StringLength(2000)]
        public string? Feedback { get; set; }
    }
}