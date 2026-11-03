
using Microsoft.EntityFrameworkCore;
using StudentCourseEFCore.Data;
using StudentCourseEFCore.Models;

namespace StudentCourseEFCore.Repositories;

public class CourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Course course)
    {
        _context.Courses.Add(course);
        _context.SaveChanges();
    }

    public List<Course> GetAll()
    {
        return _context.Courses
            .AsNoTracking()
            .ToList();
    }

    public Course? GetById(int id)
    {
        return _context.Courses.Find(id);
    }

    public void Update(Course course)
    {
        _context.Courses.Update(course);
        _context.SaveChanges();
    }

    public bool Delete(int id)
    {
        var course = _context.Courses.Find(id);

        if (course is null)
            return false;

        _context.Courses.Remove(course);
        _context.SaveChanges();

        return true;
    }
}
