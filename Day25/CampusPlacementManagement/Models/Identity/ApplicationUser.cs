
using Microsoft.AspNetCore.Identity;
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public AccountStatus AccountStatus { get; set; } = AccountStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
