using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;

namespace CampusPlacementManagement.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        // Identity account
        public string UserId { get; set; } = string.Empty;

        // College and Placement Officer
        public int CollegeId { get; set; }
        public int PlacementOfficerId { get; set; }

        // Student details
        public string RegisterNumber { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public int GraduationYear { get; set; }

        // Academic details
        public decimal CGPA { get; set; }
        public int ActiveBacklogs { get; set; }

        // Optional resume
        public string? ResumeUrl { get; set; }

        public StudentStatus Status { get; set; } = StudentStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public College College { get; set; } = null!;
        public PlacementOfficer PlacementOfficer { get; set; } = null!;
    }
}