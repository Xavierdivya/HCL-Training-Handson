using CampusPlacementManagement.Models.Identity;

namespace CampusPlacementManagement.Models
{
    public class CompanyHR
    {
        public int CompanyHRId { get; set; }

        // Foreign key to ASP.NET Core Identity user.
        public string UserId { get; set; } = string.Empty;

        // Foreign key to the associated company.
        public int CompanyId { get; set; }

        public string Designation { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation property to Identity user.
        public ApplicationUser User { get; set; } = null!;

        // Navigation property to Company.
        public Company Company { get; set; } = null!;
    }
}