using Microsoft.Extensions.DependencyInjection;
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

var connectionString =
    "Server=localhost\\SQLEXPRESS;Database=LeaveManagementDb;Trusted_Connection=True;TrustServerCertificate=True;";

// Ask for the username when the application starts
Console.Write("Enter your username: ");
string? currentUser = Console.ReadLine();

while (string.IsNullOrWhiteSpace(currentUser))
{
    Console.Write("Username cannot be empty. Enter your username: ");
    currentUser = Console.ReadLine();
}

currentUser = currentUser.Trim();

Console.WriteLine($"Welcome, {currentUser}!");

// Create the dependency injection container
var services = new ServiceCollection();
// Register database context
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register application services
services.AddScoped<EmployeeService>();
services.AddScoped<LeaveTypeService>();
services.AddScoped<LeaveRequestService>();
services.AddScoped<LeaveApprovalService>();
services.AddScoped<AuditLogService>();


// Build the dependency injection container
using var serviceProvider = services.BuildServiceProvider();

// Create a scope
using var scope = serviceProvider.CreateScope();

// Get services from the DI container
var employeeService = scope.ServiceProvider
    .GetRequiredService<EmployeeService>();

var leaveTypeService = scope.ServiceProvider
    .GetRequiredService<LeaveTypeService>();

var leaveRequestService = scope.ServiceProvider
    .GetRequiredService<LeaveRequestService>();

var leaveApprovalService = scope.ServiceProvider
    .GetRequiredService<LeaveApprovalService>();

var auditLogService = scope.ServiceProvider
    .GetRequiredService<AuditLogService>();

bool running = true;

while (running)
{
    Console.WriteLine("\n=== Leave Management System ===");
    Console.WriteLine("1. Add employee");
    Console.WriteLine("2. View all employees");
    Console.WriteLine("3. Find employee by ID");
    Console.WriteLine("4. Update employee");
    Console.WriteLine("5. Delete employee");
    Console.WriteLine("6. Add leave type");
    Console.WriteLine("7. View all leave types");
    Console.WriteLine("8. Find leave type by ID");
    Console.WriteLine("9. Update leave type");
    Console.WriteLine("10. Delete leave type");
    Console.WriteLine("11. Submit leave request");
    Console.WriteLine("12. View all leave requests");
    Console.WriteLine("13. Find leave request by ID");
    Console.WriteLine("14. View pending leave requests");
    Console.WriteLine("15. Approve or reject a leave request");
    Console.WriteLine("16. View Audit Logs");
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
                string? department =
                    ReadOptional("Department (optional): ");

                var employee = new Employee
                {
                    FullName = name,
                    Email = email,
                    Department = department,
                    IsActive = true
                };

                    await employeeService.AddEmployeeAsync(employee);

                    // Save audit log
                    await auditLogService.LogAsync(
                        "CREATE",
                        "Employee",
                        employee.EmployeeId,
                        $"Employee {employee.FullName} was added.",
                        currentUser);

                    Console.WriteLine(
                        $"Employee added. ID: {employee.EmployeeId}");
                    break;
            }

            case "2":
            {
                var employees =
                    await employeeService.GetAllEmployeesAsync();

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
                if (!TryReadId("Employee ID: ", out int id))
                    break;

                var employee =
                    await employeeService.GetEmployeeByIdAsync(id);

                if (employee == null)
                {
                    Console.WriteLine("Employee not found.");
                    break;
                }

                Console.WriteLine($"Name: {employee.FullName}");
                Console.WriteLine($"Email: {employee.Email}");
                Console.WriteLine(
                    $"Department: {employee.Department ?? "N/A"}");
                Console.WriteLine($"Active: {employee.IsActive}");
                break;
            }

            case "4":
            {
                if (!TryReadId("Employee ID: ", out int id))
                    break;

                string name = ReadRequired("New full name: ");
                string email = ReadRequired("New email: ");
                string? department =
                    ReadOptional("New department (optional): ");

                    bool updated =
        await employeeService.UpdateEmployeeAsync(
            id, name, email, department);

                    if (updated)
                    {
                        await auditLogService.LogAsync(
                            "UPDATE",
                            "Employee",
                            id,
                            $"Employee {name} was updated.",
                            currentUser);

                        Console.WriteLine("Employee updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Employee not found.");
                    }
                    break;
            }

            case "5":
            {
                if (!TryReadId("Employee ID: ", out int id))
                    break;

                    bool deleted =
         await employeeService.DeleteEmployeeAsync(id);

                    if (deleted)
                    {
                        await auditLogService.LogAsync(
                            "DELETE",
                            "Employee",
                            id,
                            $"Employee with ID {id} was deleted.",
                            currentUser);

                        Console.WriteLine("Employee deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Employee not found or has existing leave requests.");
                    }
                    break;
            }

            case "6":
            {
                string name = ReadRequired("Leave type name: ");

                if (!TryReadPositiveInt(
                    "Maximum days: ", out int maximumDays))
                    break;

                var leaveType = new LeaveType
                {
                    LeaveTypeName = name,
                    MaximumDays = maximumDays,
                    IsActive = true
                };

                    await leaveTypeService.AddLeaveTypeAsync(leaveType);

                    await auditLogService.LogAsync(
                        "CREATE",
                        "LeaveType",
                        leaveType.LeaveTypeId,
                        $"Leave type {leaveType.LeaveTypeName} was added.",
                        currentUser);

                    Console.WriteLine(
                        $"Leave type added. ID: {leaveType.LeaveTypeId}");
                    break;
            }

            case "7":
            {
                var leaveTypes =
                    await leaveTypeService.GetAllLeaveTypesAsync();

                if (leaveTypes.Count == 0)
                {
                    Console.WriteLine("No leave types found.");
                    break;
                }

                foreach (var leaveType in leaveTypes)
                {
                    Console.WriteLine(
                        $"ID: {leaveType.LeaveTypeId} | " +
                        $"Name: {leaveType.LeaveTypeName} | " +
                        $"Maximum days: {leaveType.MaximumDays} | " +
                        $"Active: {leaveType.IsActive}");
                }

                break;
            }

            case "8":
            {
                if (!TryReadId("Leave type ID: ", out int id))
                    break;

                var leaveType =
                    await leaveTypeService.GetLeaveTypeByIdAsync(id);

                if (leaveType == null)
                {
                    Console.WriteLine("Leave type not found.");
                    break;
                }

                Console.WriteLine($"Name: {leaveType.LeaveTypeName}");
                Console.WriteLine(
                    $"Maximum days: {leaveType.MaximumDays}");
                Console.WriteLine($"Active: {leaveType.IsActive}");
                break;
            }

            case "9":
            {
                if (!TryReadId("Leave type ID: ", out int id))
                    break;

                string name =
                    ReadRequired("New leave type name: ");

                if (!TryReadPositiveInt(
                    "New maximum days: ", out int maximumDays))
                    break;

                    bool updated =
        await leaveTypeService.UpdateLeaveTypeAsync(
            id, name, maximumDays);

                    if (updated)
                    {
                        await auditLogService.LogAsync(
                            "UPDATE",
                            "LeaveType",
                            id,
                            $"Leave type {name} was updated.",
                            currentUser);

                        Console.WriteLine("Leave type updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Leave type not found.");
                    }

                    break;
            }

            case "10":
            {
                if (!TryReadId("Leave type ID: ", out int id))
                    break;

                    bool deleted =
        await leaveTypeService.DeleteLeaveTypeAsync(id);

                    if (deleted)
                    {
                        await auditLogService.LogAsync(
                            "DELETE",
                            "LeaveType",
                            id,
                            $"Leave type with ID {id} was deleted.",
                            currentUser);

                        Console.WriteLine("Leave type deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Leave type not found or referenced by leave requests.");
                    }
                    break;
            }

            case "11":
                {
                    if (!TryReadId("Employee ID: ", out int employeeId))
                        break;

                    if (!TryReadId("Leave Type ID: ", out int leaveTypeId))
                        break;

                    if (!TryReadDate("Start Date (yyyy-MM-dd): ", out DateOnly startDate))
                        break;

                    if (!TryReadDate("End Date (yyyy-MM-dd): ", out DateOnly endDate))
                        break;

                    Console.Write("Reason: ");
                    string reason = Console.ReadLine() ?? "";

                    var result =
                        await leaveRequestService.AddLeaveRequestAsync(
                            employeeId,
                            leaveTypeId,
                            startDate,
                            endDate,
                            reason);

                    if (result.Success)
                    {
                        await auditLogService.LogAsync(
                            "CREATE",
                            "LeaveRequest",
                            result.RequestId,
                            $"Leave request submitted by employee ID {employeeId}.",
                            currentUser);
                    }

                    Console.WriteLine(result.Message);
                    break;
                }

            case "12":
            {
                var requests =
                    await leaveRequestService.GetAllLeaveRequestsAsync();

                if (requests.Count == 0)
                {
                    Console.WriteLine("No leave requests found.");
                    break;
                }

                foreach (var request in requests)
                {
                    DisplayLeaveRequest(request);
                    Console.WriteLine("------------------------------");
                }

                break;
            }

            case "13":
            {
                if (!TryReadId("Leave Request ID: ", out int requestId))
                    break;

                var request =
                    await leaveRequestService.GetLeaveRequestByIdAsync(
                        requestId);

                if (request == null)
                {
                    Console.WriteLine("Leave request not found.");
                    break;
                }

                DisplayLeaveRequest(request);
                break;
            }

            case "14":
            {
                var pendingRequests =
                    await leaveApprovalService.GetPendingRequestsAsync();

                if (pendingRequests.Count == 0)
                {
                    Console.WriteLine("No pending leave requests found.");
                    break;
                }

                foreach (var request in pendingRequests)
                {
                    DisplayLeaveRequest(request);
                    Console.WriteLine("------------------------------");
                }

                break;
                }

case "15":
                {
                    if (!TryReadId(
                        "Leave Request ID: ", out int requestId))
                        break;

                    Console.WriteLine("1. Approve");
                    Console.WriteLine("2. Reject");
                    Console.Write("Choose an action: ");

                    string? approvalChoice = Console.ReadLine();

                    LeaveStatus newStatus;
                    string auditAction;

                    if (approvalChoice == "1")
                    {
                        newStatus = LeaveStatus.Approved;
                        auditAction = "APPROVE";
                    }
                    else if (approvalChoice == "2")
                    {
                        newStatus = LeaveStatus.Rejected;
                        auditAction = "REJECT";
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                        break;
                    }

                    bool success =
                        await leaveApprovalService.UpdateRequestStatusAsync(
                            requestId, newStatus);

                    if (success)
                    {
                        await auditLogService.LogAsync(
                            auditAction,
                            "LeaveRequest",
                            requestId,
                            $"Leave request {requestId} was {newStatus}.",
                            currentUser);

                        Console.WriteLine(
                            $"Request {requestId} updated to {newStatus}.");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Request not found or it is no longer pending.");
                    }

                    break;
                }

            case "16":
                {
                    var auditLogs =
                        await auditLogService.GetAllAuditLogsAsync();

                    if (auditLogs.Count == 0)
                    {
                        Console.WriteLine("No audit logs found.");
                        break;
                    }

                    Console.WriteLine("\n========== AUDIT HISTORY ==========");

                    foreach (var log in auditLogs)
                    {
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine($"Audit ID    : {log.AuditLogId}");
                        Console.WriteLine($"Action      : {log.Action}");
                        Console.WriteLine($"Entity      : {log.EntityName}");
                        Console.WriteLine($"Entity ID   : {log.EntityId}");
                        Console.WriteLine($"Description : {log.Description}");
                        Console.WriteLine($"Performed By: {log.PerformedBy}");
                        Console.WriteLine($"Date & Time : {log.PerformedAt:dd-MM-yyyy HH:mm:ss}");
                    }

                    Console.WriteLine("===================================");
                    break;
                }

            case "0":
            {
                running = false;
                Console.WriteLine("Exiting application.");
                break;
            }

            default:
            {
                Console.WriteLine(
                    "Invalid option. Choose a number from 0 to 15.");
                break;
            }
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

    return string.IsNullOrWhiteSpace(value)
        ? null
        : value.Trim();
}

static bool TryReadId(string prompt, out int id)
{
    Console.Write(prompt);

    bool valid =
        int.TryParse(Console.ReadLine(), out id) && id > 0;

    if (!valid)
        Console.WriteLine("Enter a valid positive ID.");

    return valid;
}

static bool TryReadPositiveInt(string prompt, out int value)
{
    Console.Write(prompt);

    bool valid =
        int.TryParse(Console.ReadLine(), out value) && value > 0;

    if (!valid)
        Console.WriteLine("Enter a positive whole number.");

    return valid;
}

static bool TryReadDate(string prompt, out DateOnly date)
{
    Console.Write(prompt);

    bool valid = DateOnly.TryParseExact(
        Console.ReadLine(),
        "yyyy-MM-dd",
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None,
        out date);

    if (!valid)
        Console.WriteLine("Enter a valid date in yyyy-MM-dd format.");

    return valid;
}

static void DisplayLeaveRequest(LeaveRequest request)
{
    Console.WriteLine($"Request ID: {request.LeaveRequestId}");
    Console.WriteLine(
        $"Employee: {request.Employee?.FullName ?? "Unknown"}");
    Console.WriteLine(
        $"Leave type: {request.LeaveType?.LeaveTypeName ?? "Unknown"}");
    Console.WriteLine($"Start date: {request.StartDate:yyyy-MM-dd}");
    Console.WriteLine($"End date: {request.EndDate:yyyy-MM-dd}");
    Console.WriteLine($"Reason: {request.Reason}");
    Console.WriteLine($"Status: {request.Status}");
    Console.WriteLine($"Requested on: {request.RequestedOn:yyyy-MM-dd HH:mm} UTC");
}
