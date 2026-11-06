
using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.DTOs.CompanyHRs;

namespace CampusPlacementManagement.DTOs.Companies
{
    public class CompanyRegistrationDto
    {
        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string CompanyCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Industry { get; set; } = string.Empty;

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
        [StringLength(250)]
        public string? Website { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public CompanyHRRegistrationDto HR { get; set; } = new();
    }
}
