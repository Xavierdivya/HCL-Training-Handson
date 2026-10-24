using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Api.Models;

public class Learner
{
    [Key]
    [Required]
    [MaxLength(20)]
    public string LearnerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LearnerName { get; set; } = string.Empty;


    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();
}