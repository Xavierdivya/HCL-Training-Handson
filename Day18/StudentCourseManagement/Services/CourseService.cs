
using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class CourseService
{
    private readonly CourseRepository _repository;

    public CourseService(CourseRepository repository)
    {
        _repository = repository;
    }

    private static void Validate(Course course)
    {
        if (string.IsNullOrWhiteSpace(course.CourseName))
            throw new ArgumentException("Course name is required.");

        if (course.CourseName.Trim().Length > 100)
            throw new ArgumentException(
                "Course name cannot exceed 100 characters.");

        if (course.DurationMonths <= 0)
            throw new ArgumentException(
                "Duration must be greater than zero.");

        if (course.Fee < 0 || course.Fee > 99999999.99m)
            throw new ArgumentException(
                "Fee must be between 0 and 99,999,999.99.");

        if (course.DepartmentId <= 0)
            throw new ArgumentException(
                "Department ID must be greater than zero.");

        course.CourseName = course.CourseName.Trim();
    }

    public Task<int> AddAsync(Course course)
    {
        Validate(course);
        return _repository.AddAsync(course);
    }

    public Task<List<Course>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<bool> UpdateAsync(Course course)
    {
        if (course.CourseId <= 0)
            throw new ArgumentException(
                "Course ID must be greater than zero.");

        Validate(course);
        return _repository.UpdateAsync(course);
    }

    public Task<bool> DeleteAsync(int courseId)
    {
        if (courseId <= 0)
            throw new ArgumentException(
                "Course ID must be greater than zero.");

        return _repository.DeleteAsync(courseId);
    }
}
