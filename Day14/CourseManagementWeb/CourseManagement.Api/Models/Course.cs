using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagement.Api.Models;

public class Course
{
    [Key]
    [Required]
    [MaxLength(20)]
    public string CourseCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string CourseTitle { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Fee { get; set; }

    public int TotalSeats { get; set; }

    public int EnrolledCount { get; set; }

    public int DurationHours { get; set; }

    [NotMapped]
    public int AvailableSeats => TotalSeats - EnrolledCount;

    public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();
}