using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class DepartmentService
{
    private readonly DepartmentRepository _repository;

    public DepartmentService(DepartmentRepository repository)
    {
        _repository = repository;
    }

    public Task<int> AddAsync(Department department)
    {
        if (string.IsNullOrWhiteSpace(department.DepartmentName))
        {
            throw new ArgumentException(
                "Department name is required.");
        }

        if (department.DepartmentName.Length > 100)
        {
            throw new ArgumentException(
                "Department name cannot exceed 100 characters.");
        }

        if (department.Location?.Length > 100)
        {
            throw new ArgumentException(
                "Department location cannot exceed 100 characters.");
        }

        department.DepartmentName =
            department.DepartmentName.Trim();

        if (department.Location is not null)
        {
            department.Location = department.Location.Trim();

            if (string.IsNullOrWhiteSpace(department.Location))
            {
                department.Location = null;
            }
        }

        return _repository.AddAsync(department);
    }

    public Task<List<Department>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }
}