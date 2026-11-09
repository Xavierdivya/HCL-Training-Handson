
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models
{
    public class JobApplication
    {
        public int JobApplicationId { get; set; }

        public int JobId { get; set; }

        public int StudentId { get; set; }

        public ApplicationStatus Status { get; set; }
            = ApplicationStatus.Submitted;

        public DateTime AppliedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public string? CoverLetter { get; set; }

        public string? Remarks { get; set; }

        public Job Job { get; set; } = null!;

        public Student Student { get; set; } = null!;
    }
}
