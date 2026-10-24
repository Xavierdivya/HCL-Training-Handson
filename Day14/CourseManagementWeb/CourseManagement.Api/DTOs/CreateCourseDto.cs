
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Api.DTOs;

public class CreateCourseDto
{
    [Required]
    [StringLength(20, MinimumLength = 1)]
    public string CourseCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string CourseTitle { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "99999999.99")]
    public decimal Fee { get; set; }

    [Range(1, int.MaxValue)]
    public int TotalSeats { get; set; }

    [Range(1, int.MaxValue)]
    public int DurationHours { get; set; }
}
