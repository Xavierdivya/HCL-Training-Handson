
using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;
using StudentCourseManagement.Services;

DepartmentRepository repository = new();
DepartmentService service = new(repository);

bool running = true;

while (running)
{
    Console.WriteLine("\n===== Student Course Management =====");
    Console.WriteLine("1. Add Department");
    Console.WriteLine("2. View All Departments");
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
