
using CampusPlacementManagement.Models.Identity;

namespace CampusPlacementManagement.Models
{
    public class PlacementOfficer
    {
        public int PlacementOfficerId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int CollegeId { get; set; }

        public string Designation { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ApplicationUser User { get; set; } = null!;

        public College College { get; set; } = null!;
    }
}
