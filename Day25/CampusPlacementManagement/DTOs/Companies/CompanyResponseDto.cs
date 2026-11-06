
namespace CampusPlacementManagement.DTOs.Companies
{
    public class CompanyResponseDto
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string CompanyCode { get; set; } = string.Empty;

        public string Industry { get; set; } = string.Empty;

        public string OfficialEmail { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string? Website { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int HRCount { get; set; }
    }
}
