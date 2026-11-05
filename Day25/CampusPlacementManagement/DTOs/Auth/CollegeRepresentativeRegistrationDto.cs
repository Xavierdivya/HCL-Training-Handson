using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Auth
{
    public class CollegeRepresentativeRegistrationDto
    {
        // Representative details
        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;

        // College details
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
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [Url]
        public string? Website { get; set; }
    }
}