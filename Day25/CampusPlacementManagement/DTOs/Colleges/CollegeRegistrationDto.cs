
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Colleges
{
    public class CollegeRegistrationDto
    {
        [Required]
        [StringLength(150)]
        public string CollegeName { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string CollegeCode { get; set; } = string.Empty;

        [StringLength(150)]
        public string? UniversityName { get; set; }

        [Required]
        [EmailAddress]
        public string OfficialEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [Url]
        public string? Website { get; set; }
    }
}
