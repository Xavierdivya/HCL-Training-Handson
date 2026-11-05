namespace CampusPlacementManagement.DTOs.Auth
{
    public class UserResponseDto
    {
        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string AccountStatus { get; set; } = string.Empty;

        public int? CollegeId { get; set; }

        public string? CollegeName { get; set; }

        public List<string> Roles { get; set; } = new();
    }
}