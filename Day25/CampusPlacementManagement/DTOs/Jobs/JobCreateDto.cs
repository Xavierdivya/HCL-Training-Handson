using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Jobs
{
    public class JobCreateDto
    {
        [Required]
        [StringLength(150)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(3000)]
        public string JobDescription { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string EmploymentType { get; set; } = string.Empty;

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

        [Required]
        public DateTime ApplicationDeadline { get; set; }
    }
}