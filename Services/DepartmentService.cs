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


    public async Task<bool> UpdateAsync(Department department)
    {
        if (department.DepartmentId <= 0)
        {
            throw new ArgumentException(
                "Department ID must be greater than zero.");
        }

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

        return await _repository.UpdateAsync(department);
    }

    public async Task<bool> DeleteAsync(int departmentId)
    {
        if (departmentId <= 0)
        {
            throw new ArgumentException(
                "Department ID must be greater than zero.");
        }

        return await _repository.DeleteAsync(departmentId);
    }

}