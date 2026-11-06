using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;

namespace CampusPlacementManagement.Models
{
    public class Company
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

        public CompanyStatus Status { get; set; }
            = CompanyStatus.Pending;

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // One company can have multiple HR user accounts.
        public ICollection<ApplicationUser> HRAccounts { get; set; }
            = new List<ApplicationUser>();
    }
}