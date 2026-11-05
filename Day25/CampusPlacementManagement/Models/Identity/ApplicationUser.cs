
using Microsoft.AspNetCore.Identity;
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public AccountStatus AccountStatus { get; set; }
            = AccountStatus.Pending;

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Foreign key: College associated with this representative.
        // Nullable because SuperAdmin does not belong to a college.
        public int? CollegeId { get; set; }

        // Navigation property.
        public College? College { get; set; }
    }
}
