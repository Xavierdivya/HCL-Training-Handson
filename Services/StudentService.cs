
using System.Net.Mail;
using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class StudentService
{
    private readonly StudentRepository _repository;

    public StudentService(StudentRepository repository)
    {
        _repository = repository;
    }

    private static void ValidateStudent(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Name))
            throw new ArgumentException("Student name is required.");

        if (student.Name.Trim().Length > 100)
            throw new ArgumentException(
                "Student name cannot exceed 100 characters.");

        if (string.IsNullOrWhiteSpace(student.Email))
            throw new ArgumentException("Email is required.");

        if (student.Email.Trim().Length > 255)
            throw new ArgumentException(
                "Email cannot exceed 255 characters.");

        try
        {
            _ = new MailAddress(student.Email.Trim());
        }
        catch (FormatException)
        {
            throw new ArgumentException("Enter a valid email address.");
        }

        if (student.Age < 17 || student.Age > 60)
            throw new ArgumentException(
                "Student age must be between 17 and 60.");

        if (student.DepartmentId <= 0)
            throw new ArgumentException(
                "Department ID must be greater than zero.");

        student.Name = student.Name.Trim();
        student.Email = student.Email.Trim();
    }

    public Task<int> AddAsync(Student student)
    {
        ValidateStudent(student);
        return _repository.AddAsync(student);
    }

    public Task<List<Student>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<bool> UpdateAsync(Student student)
    {
        if (student.StudentId <= 0)
            throw new ArgumentException(
                "Student ID must be greater than zero.");

        ValidateStudent(student);
        return _repository.UpdateAsync(student);
    }

    public Task<bool> DeleteAsync(int studentId)
    {
        if (studentId <= 0)
            throw new ArgumentException(
                "Student ID must be greater than zero.");

        return _repository.DeleteAsync(studentId);
    }
}
