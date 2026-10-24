
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagement.Api.Models;

public class Enrollment
{
    [Key]
    public int EnrollmentId { get; set; }

    [Required]
    [MaxLength(20)]
    public string LearnerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string CourseCode { get; set; } = string.Empty;

    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(10,2)")]
    [Range(typeof(decimal), "0", "99999999.99")]
    public decimal BaseFee { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Range(typeof(decimal), "0", "99999999.99")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Range(typeof(decimal), "0", "99999999.99")]
    public decimal FinalFee { get; set; }

    public EnrollmentStatus Status { get; set; }
        = EnrollmentStatus.Active;

    [ForeignKey(nameof(LearnerId))]
    public Learner? Learner { get; set; }

    [ForeignKey(nameof(CourseCode))]
    public Course? Course { get; set; }
}
