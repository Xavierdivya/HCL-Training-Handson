
using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class EnrollmentService
{
    private readonly EnrollmentRepository _repository;

    public EnrollmentService(EnrollmentRepository repository)
    {
        _repository = repository;
    }

    public Task<int> AddAsync(Enrollment enrollment)
    {
        ValidateIds(enrollment.StudentId, enrollment.CourseId);

        enrollment.Status = "Enrolled";

        return _repository.AddAsync(enrollment);
    }

    public Task<List<Enrollment>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<bool> UpdateStatusAsync(
        int enrollmentId, string status)
    {
        if (enrollmentId <= 0)
        {
            throw new ArgumentException(
                "Enrollment ID must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ArgumentException(
                "Enrollment status is required.");
        }

        status = status.Trim();

        string[] validStatuses =
        {
            "Enrolled",
            "Completed",
            "Cancelled"
        };

        string? validStatus = validStatuses.FirstOrDefault(
            s => s.Equals(
                status, StringComparison.OrdinalIgnoreCase));

        if (validStatus is null)
        {
            throw new ArgumentException(
                "Status must be Enrolled, Completed, or Cancelled.");
        }

        return _repository.UpdateStatusAsync(
            enrollmentId, validStatus);
    }

    public Task<bool> DeleteAsync(int enrollmentId)
    {
        if (enrollmentId <= 0)
        {
            throw new ArgumentException(
                "Enrollment ID must be greater than zero.");
        }

        return _repository.DeleteAsync(enrollmentId);
    }

    private static void ValidateIds(int studentId, int courseId)
    {
        if (studentId <= 0)
        {
            throw new ArgumentException(
                "Student ID must be greater than zero.");
        }

        if (courseId <= 0)
        {
            throw new ArgumentException(
                "Course ID must be greater than zero.");
        }
    }
}
