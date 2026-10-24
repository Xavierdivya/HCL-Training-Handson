
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Api.DTOs;

public class CreateEnrollmentDto
{
    [Required]
    [StringLength(20, MinimumLength = 1)]
    public string LearnerId { get; set; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 1)]
    public string CourseCode { get; set; } = string.Empty;
}
