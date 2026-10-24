
using CourseManagement.Api.Data;
using CourseManagement.Api.DTOs;
using CourseManagement.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EnrollmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/enrollments
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var enrollments = await _context.Enrollments
            .AsNoTracking()
            .OrderByDescending(e => e.EnrollmentDate)
            .Select(e => new
            {
                e.EnrollmentId,
                e.LearnerId,
                LearnerName = e.Learner!.LearnerName,
                e.CourseCode,
                CourseTitle = e.Course!.CourseTitle,
                e.EnrollmentDate,
                e.BaseFee,
                e.DiscountAmount,
                e.FinalFee,
                Status = e.Status.ToString()
            })
            .ToListAsync();

        return Ok(enrollments);
    }

    // GET: api/enrollments/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Enrollment ID must be greater than zero.");

        var enrollment = await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.EnrollmentId == id)
            .Select(e => new
            {
                e.EnrollmentId,
                e.LearnerId,
                LearnerName = e.Learner!.LearnerName,
                e.CourseCode,
                CourseTitle = e.Course!.CourseTitle,
                e.EnrollmentDate,
                e.BaseFee,
                e.DiscountAmount,
                e.FinalFee,
                Status = e.Status.ToString()
            })
            .FirstOrDefaultAsync();

        if (enrollment is null)
            return NotFound("Enrollment not found.");

        return Ok(enrollment);
    }

    // POST: api/enrollments
    [HttpPost]
    public async Task<IActionResult> Create(CreateEnrollmentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.LearnerId))
            return BadRequest("Learner ID is required.");

        if (string.IsNullOrWhiteSpace(dto.CourseCode))
            return BadRequest("Course code is required.");

        var learnerId = dto.LearnerId.Trim();
        var courseCode = dto.CourseCode.Trim();

        var learnerExists = await _context.Learners
            .AnyAsync(l => l.LearnerId == learnerId);

        if (!learnerExists)
            return BadRequest("The specified learner does not exist.");

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.CourseCode == courseCode);

        if (course is null)
            return BadRequest("The specified course does not exist.");

        var alreadyEnrolled = await _context.Enrollments
            .AnyAsync(e =>
                e.LearnerId == learnerId &&
                e.CourseCode == courseCode &&
                e.Status == EnrollmentStatus.Active);

        if (alreadyEnrolled)
            return Conflict("The learner is already enrolled in this course.");

        if (course.TotalSeats <= 0 ||
            course.EnrolledCount >= course.TotalSeats)
        {
            return Conflict("No seats are available for this course.");
        }

        const decimal discountRate = 0.10m;

        decimal baseFee = course.Fee;
        decimal discount = Math.Round(
            baseFee * discountRate,
            2,
            MidpointRounding.AwayFromZero);

        var enrollment = new Enrollment
        {
            LearnerId = learnerId,
            CourseCode = courseCode,
            EnrollmentDate = DateTime.Now,
            BaseFee = baseFee,
            DiscountAmount = discount,
            FinalFee = baseFee - discount,
            Status = EnrollmentStatus.Active
        };

        course.EnrolledCount++;
        _context.Enrollments.Add(enrollment);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = enrollment.EnrollmentId },
            new
            {
                enrollment.EnrollmentId,
                enrollment.LearnerId,
                enrollment.CourseCode,
                enrollment.EnrollmentDate,
                enrollment.BaseFee,
                enrollment.DiscountAmount,
                enrollment.FinalFee,
                Status = enrollment.Status.ToString()
            });
    }

    // PUT: api/enrollments/{id}/cancel
    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        if (id <= 0)
            return BadRequest("Enrollment ID must be greater than zero.");

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.EnrollmentId == id);

        if (enrollment is null)
            return NotFound("Enrollment not found.");

        if (enrollment.Status == EnrollmentStatus.Cancelled)
            return Conflict("This enrollment has already been cancelled.");

        if (enrollment.Status != EnrollmentStatus.Active)
            return Conflict("Only active enrollments can be cancelled.");

        var course = await _context.Courses
            .FirstOrDefaultAsync(c =>
                c.CourseCode == enrollment.CourseCode);

        if (course is null)
            return Conflict("The associated course no longer exists.");

        enrollment.Status = EnrollmentStatus.Cancelled;
        course.EnrolledCount = Math.Max(
            0, course.EnrolledCount - 1);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return Ok(new
        {
            message = "Enrollment cancelled successfully.",
            enrollment.EnrollmentId,
            Status = enrollment.Status.ToString(),
            course.EnrolledCount,
            course.TotalSeats
        });
    }
}
