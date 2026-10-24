
using CourseManagement.Api.Data;
using CourseManagement.Api.DTOs;
using CourseManagement.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CoursesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/courses
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _context.Courses
            .AsNoTracking()
            .OrderBy(c => c.CourseTitle)
            .ToListAsync();

        return Ok(courses);
    }

    // GET: api/courses/available
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable()
    {
        var courses = await _context.Courses
            .AsNoTracking()
            .Where(c => c.EnrolledCount < c.TotalSeats)
            .OrderBy(c => c.CourseTitle)
            .ToListAsync();

        return Ok(courses);
    }

    // GET: api/courses/{code}
    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest("Course code is required.");

        code = code.Trim();

        var course = await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CourseCode == code);

        if (course is null)
            return NotFound("Course not found.");

        return Ok(course);
    }

    // POST: api/courses
    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CourseCode))
            return BadRequest("Course code is required.");

        if (string.IsNullOrWhiteSpace(dto.CourseTitle))
            return BadRequest("Course title is required.");

        var code = dto.CourseCode.Trim();
        var title = dto.CourseTitle.Trim();

        if (await _context.Courses.AnyAsync(
            c => c.CourseCode == code))
        {
            return Conflict("A course with this code already exists.");
        }

        if (dto.Fee < 0)
            return BadRequest("Course fee cannot be negative.");

        if (dto.TotalSeats <= 0)
            return BadRequest("Total seats must be greater than zero.");

        if (dto.DurationHours <= 0)
            return BadRequest("Duration must be greater than zero.");

        var course = new Course
        {
            CourseCode = code,
            CourseTitle = title,
            Fee = dto.Fee,
            TotalSeats = dto.TotalSeats,
            EnrolledCount = 0,
            DurationHours = dto.DurationHours
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetByCode),
            new { code = course.CourseCode },
            course);
    }

    // PUT: api/courses/{code}
    [HttpPut("{code}")]
    public async Task<IActionResult> Update(
        string code,
        CreateCourseDto dto)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest("Course code is required.");

        if (string.IsNullOrWhiteSpace(dto.CourseCode))
            return BadRequest("Course code is required.");

        if (string.IsNullOrWhiteSpace(dto.CourseTitle))
            return BadRequest("Course title is required.");

        code = code.Trim();

        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.CourseCode == code);

        if (course is null)
            return NotFound("Course not found.");

        if (!string.Equals(
            code,
            dto.CourseCode.Trim(),
            StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(
                "CourseCode cannot be changed during an update.");
        }

        if (dto.Fee < 0)
            return BadRequest("Course fee cannot be negative.");

        if (dto.TotalSeats <= 0 ||
            dto.TotalSeats < course.EnrolledCount)
        {
            return BadRequest(
                "Total seats must be positive and cannot be less than the current enrollment count.");
        }

        if (dto.DurationHours <= 0)
            return BadRequest("Duration must be greater than zero.");

        course.CourseTitle = dto.CourseTitle.Trim();
        course.Fee = dto.Fee;
        course.TotalSeats = dto.TotalSeats;
        course.DurationHours = dto.DurationHours;

        await _context.SaveChangesAsync();

        return Ok(course);
    }

    // DELETE: api/courses/{code}
    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest("Course code is required.");

        code = code.Trim();

        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.CourseCode == code);

        if (course is null)
            return NotFound("Course not found.");

        bool hasEnrollments =
            await _context.Enrollments
                .AnyAsync(e => e.CourseCode == code);

        if (hasEnrollments)
        {
            return Conflict(
                "This course has enrollment history and cannot be deleted.");
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
