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

        // College association for Placement Officers.
        // Nullable because SuperAdmin and Company HR do not belong to a college.
        public int? CollegeId { get; set; }

        public College? College { get; set; }

        // Company association for Company HR accounts.
        // Nullable because SuperAdmin and Placement Officers do not belong to a company.
        public int? CompanyId { get; set; }

        public Company? Company { get; set; }
    }
}