
using Microsoft.EntityFrameworkCore;
using StudentCourseEFCore.Helpers;
using StudentCourseEFCore.Models;
using StudentCourseEFCore.Services;

namespace StudentCourseEFCore.Menus;

public class CourseMenu
{
    private readonly CourseService _service;

    public CourseMenu(CourseService service)
    {
        _service = service;
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine("\n===== COURSE MANAGEMENT =====");
            Console.WriteLine("1. Add Course");
            Console.WriteLine("2. View All Courses");
            Console.WriteLine("3. View Course By ID");
            Console.WriteLine("4. Update Course");
            Console.WriteLine("5. Delete Course");
            Console.WriteLine("0. Back to Main Menu");

            Console.Write("\nEnter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Add();
                    break;
                case 2:
                    GetAll();
                    break;
                case 3:
                    GetById();
                    break;
                case 4:
                    Update();
                    break;
                case 5:
                    Delete();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine(
                        "Invalid choice. Select an option from 0 to 5.");
                    break;
            }
        }
    }

    private void Add()
    {
        string name = ConsoleHelper.ReadRequiredInput(
            "Enter course name: ");

        Console.Write("Enter course credits (1-6): ");

        if (!int.TryParse(Console.ReadLine(), out int credits)
            || credits < 1 || credits > 6)
        {
            Console.WriteLine("Credits must be between 1 and 6.");
            return;
        }

        var course = new Course
        {
            CourseName = name,
            Credits = credits
        };

        try
        {
            _service.Add(course);
            Console.WriteLine("Course added successfully.");
        }
        catch (DbUpdateException)
        {
            Console.WriteLine(
                "Could not add course. The course name may already exist.");
        }
    }

    private void GetAll()
    {
        var courses = _service.GetAll();

        if (courses.Count == 0)
        {
            Console.WriteLine("No courses found.");
            return;
        }

        Console.WriteLine("\nID | Course Name | Credits | Active");

        foreach (var course in courses)
        {
            Console.WriteLine(
                $"{course.CourseId} | " +
                $"{course.CourseName} | " +
                $"{course.Credits} | " +
                $"{course.IsActive}");
        }
    }

    private void GetById()
    {
        if (!ConsoleHelper.TryReadId(
                "Enter course ID: ", out int id))
            return;

        var course = _service.GetById(id);

        if (course is null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        Console.WriteLine("\nCourse Details");
        Console.WriteLine($"ID: {course.CourseId}");
        Console.WriteLine($"Name: {course.CourseName}");
        Console.WriteLine($"Credits: {course.Credits}");
        Console.WriteLine($"Active: {course.IsActive}");
    }

    private void Update()
    {
        if (!ConsoleHelper.TryReadId(
                "Enter course ID to update: ", out int id))
            return;

        var course = _service.GetById(id);

        if (course is null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        string name = ConsoleHelper.ReadRequiredInput(
            "Enter new course name: ");

        Console.Write("Enter new course credits (1-6): ");

        if (!int.TryParse(Console.ReadLine(), out int credits)
            || credits < 1 || credits > 6)
        {
            Console.WriteLine("Credits must be between 1 and 6.");
            return;
        }

        course.CourseName = name;
        course.Credits = credits;

        try
        {
            _service.Update(course);
            Console.WriteLine("Course updated successfully.");
        }
        catch (DbUpdateException)
        {
            Console.WriteLine(
                "Could not update course. The course name may already exist.");
        }
    }

    private void Delete()
    {
        if (!ConsoleHelper.TryReadId(
                "Enter course ID to delete: ", out int id))
            return;

        var course = _service.GetById(id);

        if (course is null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        Console.Write(
            $"Delete course '{course.CourseName}'? (y/n): ");

        if (!string.Equals(
                Console.ReadLine(),
                "y",
                StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Deletion cancelled.");
            return;
        }

        try
        {
            bool deleted = _service.Delete(id);

            Console.WriteLine(
                deleted
                    ? "Course deleted successfully."
                    : "Course not found.");
        }
        catch (DbUpdateException)
        {
            Console.WriteLine(
                "Could not delete the course because it may be in use.");
        }
    }
}
