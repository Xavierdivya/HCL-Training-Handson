
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Api.DTOs
{
    public class CreateLearnerDto
    {
        [Required(ErrorMessage = "Learner ID is required.")]
        [StringLength(20, ErrorMessage = "Learner ID cannot exceed 20 characters.")]
        public string LearnerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Learner name is required.")]
        [StringLength(100, ErrorMessage = "Learner name cannot exceed 100 characters.")]
        public string LearnerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        public string Email { get; set; } = string.Empty;
    }
}
