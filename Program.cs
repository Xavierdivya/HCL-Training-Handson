
using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;
using StudentCourseManagement.Services;
using Microsoft.Data.SqlClient;

DepartmentRepository repository = new();
DepartmentService service = new(repository);
StudentRepository studentRepository = new();
StudentService studentService = new(studentRepository);
CourseRepository courseRepository = new();
CourseService courseService = new(courseRepository);

bool running = true;

while (running)
{
    Console.WriteLine("\n===== Student Course Management =====");
    Console.WriteLine("1. Add Department");
    Console.WriteLine("2. View All Departments");
    Console.WriteLine("3. Update Department");
    Console.WriteLine("4. Delete Department");
    Console.WriteLine("5. Add Student");
    Console.WriteLine("6. View All Students");
    Console.WriteLine("7. Update Student");
    Console.WriteLine("8. Delete Student");
    Console.WriteLine("9.Add Course");
    Console.WriteLine("10.View All Courses");
    Console.WriteLine("11.Update Course");
    Console.WriteLine("12.Delete Course");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");

    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                Console.Write("Enter department name: ");
                string departmentName = Console.ReadLine() ?? "";

                Console.Write("Enter department location (optional): ");
                string? location = Console.ReadLine();

                Department department = new()
                {
                    DepartmentName = departmentName,
                    Location = string.IsNullOrWhiteSpace(location)
                        ? null
                        : location
                };

                int newId = await service.AddAsync(department);

                Console.WriteLine(
                    $"Department added successfully! ID: {newId}");
                break;

            case "2":
                List<Department> departments =
                    await service.GetAllAsync();

                if (departments.Count == 0)
                {
                    Console.WriteLine("No departments found.");
                    break;
                }

                Console.WriteLine("\n--- Department List ---");

                foreach (Department item in departments)
                {
                    Console.WriteLine(
                        $"ID: {item.DepartmentId} | " +
                        $"Name: {item.DepartmentName} | " +
                        $"Location: {item.Location ?? "N/A"} | " +
                        $"Active: {item.IsActive}");
                }
                break;


            case "3":
                Console.Write("Enter department ID to update: ");

                if (!int.TryParse(Console.ReadLine(), out int updateId) ||
                    updateId <= 0)
                {
                    Console.WriteLine("Invalid department ID.");
                    break;
                }

                Console.Write("Enter new department name: ");
                string newName = Console.ReadLine() ?? "";

                Console.Write("Enter new location (optional): ");
                string? newLocation = Console.ReadLine();

                Department updatedDepartment = new()
                {
                    DepartmentId = updateId,
                    DepartmentName = newName,
                    Location = string.IsNullOrWhiteSpace(newLocation)
                        ? null
                        : newLocation
                };

                bool updated = await service.UpdateAsync(updatedDepartment);

                Console.WriteLine(updated
                    ? "Department updated successfully!"
                    : "Department not found.");
                break;

            case "4":
                Console.Write("Enter department ID to delete: ");

                if (!int.TryParse(Console.ReadLine(), out int deleteId) ||
                    deleteId <= 0)
                {
                    Console.WriteLine("Invalid department ID.");
                    break;
                }

                bool deleted = await service.DeleteAsync(deleteId);

                Console.WriteLine(deleted
                    ? "Department deleted successfully!"
                    : "Department not found.");
                break;



            case "5":
                Console.Write("Enter student name: ");
                string studentName = Console.ReadLine() ?? "";

                Console.Write("Enter email: ");
                string email = Console.ReadLine() ?? "";

                Console.Write("Enter age: ");
                if (!int.TryParse(Console.ReadLine(), out int age))
                {
                    Console.WriteLine("Invalid age.");
                    break;
                }

                Console.Write("Enter department ID: ");
                if (!int.TryParse(Console.ReadLine(), out int departmentId))
                {
                    Console.WriteLine("Invalid department ID.");
                    break;
                }

                Student newStudent = new()
                {
                    Name = studentName,
                    Email = email,
                    Age = age,
                    DepartmentId = departmentId
                };

                int studentId = await studentService.AddAsync(newStudent);
                Console.WriteLine($"Student added! ID: {studentId}");
                break;

            case "6":
                List<Student> students = await studentService.GetAllAsync();

                if (students.Count == 0)
                {
                    Console.WriteLine("No students found.");
                    break;
                }

                foreach (Student student in students)
                {
                    Console.WriteLine(
                        $"ID: {student.StudentId} | " +
                        $"Name: {student.Name} | " +
                        $"Email: {student.Email} | " +
                        $"Age: {student.Age} | " +
                        $"Department ID: {student.DepartmentId}");
                }
                break;

            case "7":
                Console.Write("Enter student ID to update: ");
                if (!int.TryParse(Console.ReadLine(), out int updateStudentId))
                {
                    Console.WriteLine("Invalid student ID.");
                    break;
                }

                Console.Write("Enter new name: ");
                string updatedName = Console.ReadLine() ?? "";

                Console.Write("Enter new email: ");
                string updatedEmail = Console.ReadLine() ?? "";

                Console.Write("Enter new age: ");
                if (!int.TryParse(Console.ReadLine(), out int updatedAge))
                {
                    Console.WriteLine("Invalid age.");
                    break;
                }

                Console.Write("Enter new department ID: ");
                if (!int.TryParse(Console.ReadLine(), out int updatedDepartmentId))
                {
                    Console.WriteLine("Invalid department ID.");
                    break;
                }

                Student updatedStudent = new()
                {
                    StudentId = updateStudentId,
                    Name = updatedName,
                    Email = updatedEmail,
                    Age = updatedAge,
                    DepartmentId = updatedDepartmentId
                };

                bool studentUpdated =
                    await studentService.UpdateAsync(updatedStudent);

                Console.WriteLine(studentUpdated
                    ? "Student updated successfully."
                    : "Student not found.");
                break;

            case "8":
                Console.Write("Enter student ID to delete: ");
                if (!int.TryParse(Console.ReadLine(), out int deleteStudentId))
                {
                    Console.WriteLine("Invalid student ID.");
                    break;
                }

                bool studentDeleted =
                    await studentService.DeleteAsync(deleteStudentId);

                Console.WriteLine(studentDeleted
                    ? "Student deleted successfully."
                    : "Student not found.");
                break;


            case "9":
                {
                    Course course = new();

                    Console.Write("Enter course name: ");
                    course.CourseName = Console.ReadLine() ?? "";

                    Console.Write("Enter duration in months: ");
                    if (!int.TryParse(Console.ReadLine(), out int duration))
                    {
                        Console.WriteLine("Invalid duration.");
                        break;
                    }
                    course.DurationMonths = duration;

                    Console.Write("Enter course fee: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal fee))
                    {
                        Console.WriteLine("Invalid fee.");
                        break;
                    }
                    course.Fee = fee;

                    Console.Write("Enter department ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int departmentId))
                    {
                        Console.WriteLine("Invalid department ID.");
                        break;
                    }
                    course.DepartmentId = departmentId;

                    int newId = await courseService.AddAsync(course);
                    Console.WriteLine($"Course added successfully. ID: {newId}");
                    break;
                }

            case "10":
                {
                    List<Course> courses = await courseService.GetAllAsync();

                    if (courses.Count == 0)
                    {
                        Console.WriteLine("No courses found.");
                        break;
                    }

                    foreach (Course course in courses)
                    {
                        Console.WriteLine(
                            $"ID: {course.CourseId} | " +
                            $"Name: {course.CourseName} | " +
                            $"Duration: {course.DurationMonths} months | " +
                            $"Fee: {course.Fee:F2} | " +
                            $"Department ID: {course.DepartmentId} | " +
                            $"Active: {course.IsActive}");
                    }
                    break;
                }

            case "11":
                {
                    Course course = new();

                    Console.Write("Enter course ID to update: ");
                    if (!int.TryParse(Console.ReadLine(), out int courseId))
                    {
                        Console.WriteLine("Invalid course ID.");
                        break;
                    }
                    course.CourseId = courseId;

                    Console.Write("Enter new course name: ");
                    course.CourseName = Console.ReadLine() ?? "";

                    Console.Write("Enter new duration in months: ");
                    if (!int.TryParse(Console.ReadLine(), out int duration))
                    {
                        Console.WriteLine("Invalid duration.");
                        break;
                    }
                    course.DurationMonths = duration;

                    Console.Write("Enter new course fee: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal fee))
                    {
                        Console.WriteLine("Invalid fee.");
                        break;
                    }
                    course.Fee = fee;

                    Console.Write("Enter new department ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int departmentId))
                    {
                        Console.WriteLine("Invalid department ID.");
                        break;
                    }
                    course.DepartmentId = departmentId;

                    bool updated = await courseService.UpdateAsync(course);
                    Console.WriteLine(updated
                        ? "Course updated successfully."
                        : "Course not found.");
                    break;
                }

            case "12":
                {
                    Console.Write("Enter course ID to delete: ");
                    if (!int.TryParse(Console.ReadLine(), out int courseId))
                    {
                        Console.WriteLine("Invalid course ID.");
                        break;
                    }

                    bool deleted = await courseService.DeleteAsync(courseId);
                    Console.WriteLine(deleted
                        ? "Course deleted successfully."
                        : "Course not found.");
                    break;
                }

            case "0":
                running = false;
                Console.WriteLine("Exiting application...");
                break;

            default:
                Console.WriteLine("Invalid choice. Try again.");
                break;
        }
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Validation error: {ex.Message}");
    }
    catch (Microsoft.Data.SqlClient.SqlException ex)
    {
        Console.WriteLine(
            $"Database error: {ex.Message}");
    }
}
