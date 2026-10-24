
using CourseManagement.Api.Data;
using CourseManagement.Api.DTOs;
using CourseManagement.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LearnersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LearnersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/learners
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var learners = await _context.Learners
            .AsNoTracking()
            .OrderBy(l => l.LearnerName)
            .ToListAsync();

        return Ok(learners);
    }

    // GET: api/learners/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Learner ID is required.");

        id = id.Trim();

        var learner = await _context.Learners
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.LearnerId == id);

        if (learner is null)
            return NotFound("Learner not found.");

        return Ok(learner);
    }

    // POST: api/learners
    [HttpPost]
    public async Task<IActionResult> Create(CreateLearnerDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.LearnerId))
            return BadRequest("Learner ID is required.");

        if (string.IsNullOrWhiteSpace(dto.LearnerName))
            return BadRequest("Learner name is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return BadRequest("Email is required.");

        var learnerId = dto.LearnerId.Trim();
        var learnerName = dto.LearnerName.Trim();
        var email = dto.Email.Trim();

        if (await _context.Learners.AnyAsync(
            l => l.LearnerId == learnerId))
        {
            return Conflict("This learner ID already exists.");
        }

        if (await _context.Learners.AnyAsync(
            l => l.Email == email))
        {
            return Conflict("This email address is already registered.");
        }

        var learner = new Learner
        {
            LearnerId = learnerId,
            LearnerName = learnerName,
            Email = email
        };

        _context.Learners.Add(learner);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(
                "The learner could not be saved. Check for a duplicate ID or email.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = learner.LearnerId },
            learner);
    }

    // PUT: api/learners/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        string id, CreateLearnerDto dto)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Learner ID is required.");

        if (string.IsNullOrWhiteSpace(dto.LearnerId) ||
            string.IsNullOrWhiteSpace(dto.LearnerName) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest(
                "Learner ID, learner name, and email are required.");
        }

        id = id.Trim();

        var learner = await _context.Learners
            .FirstOrDefaultAsync(l => l.LearnerId == id);

        if (learner is null)
            return NotFound("Learner not found.");

        if (!string.Equals(
            id,
            dto.LearnerId.Trim(),
            StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(
                "LearnerId cannot be changed during an update.");
        }

        var learnerName = dto.LearnerName.Trim();
        var email = dto.Email.Trim();

        if (await _context.Learners.AnyAsync(
            l => l.Email == email && l.LearnerId != id))
        {
            return Conflict(
                "This email address is already used by another learner.");
        }

        learner.LearnerName = learnerName;
        learner.Email = email;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("The learner could not be updated.");
        }

        return Ok(learner);
    }

    // DELETE: api/learners/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Learner ID is required.");

        id = id.Trim();

        var learner = await _context.Learners
            .FirstOrDefaultAsync(l => l.LearnerId == id);

        if (learner is null)
            return NotFound("Learner not found.");

        bool hasEnrollments = await _context.Enrollments
            .AnyAsync(e => e.LearnerId == id);

        if (hasEnrollments)
        {
            return Conflict(
                "This learner has enrollment history and cannot be deleted.");
        }

        _context.Learners.Remove(learner);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
