
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

var connectionString =
    "Server=localhost\\SQLEXPRESS;Database=LeaveManagementDb;Trusted_Connection=True;TrustServerCertificate=True;";

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var dbContext = new ApplicationDbContext(options);
var employeeService = new EmployeeService(dbContext);

bool running = true;

while (running)
{
    Console.WriteLine("\n=== Leave Management System ===");
    Console.WriteLine("1. Add employee");
    Console.WriteLine("2. View all employees");
    Console.WriteLine("3. Find employee by ID");
    Console.WriteLine("4. Update employee");
    Console.WriteLine("5. Delete employee");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");

    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
            {
                string name = ReadRequired("Full name: ");
                string email = ReadRequired("Email: ");
                string? department = ReadOptional("Department (optional): ");

                var employee = new Employee
                {
                    FullName = name,
                    Email = email,
                    Department = department,
                    IsActive = true
                };

                await employeeService.AddEmployeeAsync(employee);
                Console.WriteLine(
                    $"Employee added successfully. ID: {employee.EmployeeId}");
                break;
            }

            case "2":
            {
                var employees = await employeeService.GetAllEmployeesAsync();

                if (employees.Count == 0)
                {
                    Console.WriteLine("No employees found.");
                    break;
                }

                foreach (var employee in employees)
                {
                    Console.WriteLine(
                        $"ID: {employee.EmployeeId} | " +
                        $"Name: {employee.FullName} | " +
                        $"Email: {employee.Email} | " +
                        $"Department: {employee.Department ?? "N/A"} | " +
                        $"Active: {employee.IsActive}");
                }

                break;
            }

            case "3":
            {
                if (!TryReadId(out int id))
                    break;

                var employee = await employeeService.GetEmployeeByIdAsync(id);

                if (employee == null)
                {
                    Console.WriteLine("Employee not found.");
                    break;
                }

                Console.WriteLine($"ID: {employee.EmployeeId}");
                Console.WriteLine($"Name: {employee.FullName}");
                Console.WriteLine($"Email: {employee.Email}");
                Console.WriteLine($"Department: {employee.Department ?? "N/A"}");
                Console.WriteLine($"Active: {employee.IsActive}");
                break;
            }

            case "4":
            {
                if (!TryReadId(out int id))
                    break;

                string name = ReadRequired("New full name: ");
                string email = ReadRequired("New email: ");
                string? department = ReadOptional("New department (optional): ");

                bool updated = await employeeService.UpdateEmployeeAsync(
                    id, name, email, department);

                Console.WriteLine(updated
                    ? "Employee updated successfully."
                    : "Employee not found.");

                break;
            }

            case "5":
            {
                if (!TryReadId(out int id))
                    break;

                bool deleted = await employeeService.DeleteEmployeeAsync(id);

                Console.WriteLine(deleted
                    ? "Employee deleted successfully."
                    : "Employee not found or has existing leave requests.");

                break;
            }

            case "0":
                running = false;
                Console.WriteLine("Exiting application.");
                break;

            default:
                Console.WriteLine("Invalid option. Choose 0 to 5.");
                break;
        }
    }
    catch (DbUpdateException ex)
    {
        Console.WriteLine(
            $"Database update failed: {ex.GetBaseException().Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Operation failed: {ex.Message}");
    }
}

static string ReadRequired(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? value = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(value))
            return value.Trim();

        Console.WriteLine("This field is required.");
    }
}

static string? ReadOptional(string prompt)
{
    Console.Write(prompt);
    string? value = Console.ReadLine();

    return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

static bool TryReadId(out int id)
{
    Console.Write("Employee ID: ");
    bool valid = int.TryParse(Console.ReadLine(), out id) && id > 0;

    if (!valid)
        Console.WriteLine("Enter a valid positive employee ID.");

    return valid;
}
