
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.PlacementDrives
{
    public class PlacementDriveCreateDto
    {
        [Required]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(150)]
        public string DriveName { get; set; } = string.Empty;

        [Required]
        [StringLength(3000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Venue { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime RegistrationDeadline { get; set; }

        [Range(typeof(decimal), "0", "10")]
        public decimal MinimumCGPA { get; set; }

        [Range(0, 100)]
        public int MaximumActiveBacklogs { get; set; }

        [Required]
        [StringLength(500)]
        public string EligibleDepartments { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string EligibleDegrees { get; set; } = string.Empty;

        [Range(2020, 2100)]
        public int GraduationYearFrom { get; set; }

        [Range(2020, 2100)]
        public int GraduationYearTo { get; set; }
    }
}
