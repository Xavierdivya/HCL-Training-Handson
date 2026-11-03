using StudentCourseEFCore.Models;
using StudentCourseEFCore.Repositories;

namespace StudentCourseEFCore.Services;

public class CourseService
{
    private readonly CourseRepository _repository;

    public CourseService(CourseRepository repository)
    {
        _repository = repository;
    }

    public void Add(Course course)
        => _repository.Add(course);

    public List<Course> GetAll()
        => _repository.GetAll();

    public Course? GetById(int id)
        => _repository.GetById(id);

    public void Update(Course course)
        => _repository.Update(course);

    public bool Delete(int id)
        => _repository.Delete(id);
}