# Student Course Management System

A console-based application built with C# and .NET 10 to manage departments, students, courses, enrollments, and audit logs using SQL Server.

## Features

- **Department Management:** Add, view, update, and delete departments.
- **Student Management:** Add, view, update, and delete student records.
- **Course Management:** Add, view, update, and delete courses.
- **Enrollment Management:** Create and view enrollments, update enrollment status, and delete enrollments.
- **Enrollment Report:** View enrollment information by combining student, course, and department data using SQL JOINs.
- **Audit Logging:** Record and view selected create, update, and delete operations.
- **Input Validation:** Validate user input and handle database and application errors.
- **Asynchronous Database Operations:** Use `async` and `await` with ADO.NET.

## Technologies Used

- C#
- .NET 10
- SQL Server Express
- ADO.NET
- `Microsoft.Data.SqlClient`
- Git and GitHub

## Project Structure

```text
StudentCourseManagement/
├── Models/
│   ├── Department.cs
│   ├── Student.cs
│   ├── Course.cs
│   ├── Enrollment.cs
│   └── AuditLog.cs
├── Repositories/
│   ├── DepartmentRepository.cs
│   ├── StudentRepository.cs
│   ├── CourseRepository.cs
│   ├── EnrollmentRepository.cs
│   ├── ReportRepository.cs
│   └── AuditLogRepository.cs
├── Services/
│   ├── DepartmentService.cs
│   ├── StudentService.cs
│   ├── CourseService.cs
│   ├── EnrollmentService.cs
│   └── AuditLogService.cs
├── Helpers/
│   └── DatabaseHelper.cs
├── Program.cs
└── README.md
```

## Prerequisites

- Visual Studio 2026 or a compatible .NET development environment.
- .NET 10 SDK.
- SQL Server Express.
- A SQL Server database named `StudentCourseDB`.
- Git.

## Database Configuration

The application uses SQL Server Express with Windows Authentication.

The default connection string in `DatabaseHelper.cs` is:

```csharp
@"Server=.\SQLEXPRESS;Database=StudentCourseDB;" +
@"Trusted_Connection=True;TrustServerCertificate=True;";
```

Ensure that SQL Server Express is running and that the database and required tables have been created before starting the application.

If your SQL Server instance or database name differs, update the connection string accordingly.

## How to Run

1. Clone the repository or open the project folder.
2. Verify the SQL Server connection string in `Helpers/DatabaseHelper.cs`.
3. Open Developer PowerShell in the project directory.
4. Restore dependencies and build the application:

   ```powershell
   dotnet restore
   dotnet build
   ```

5. Start the application:

   ```powershell
   dotnet run
   ```

6. Follow the console menu to manage departments, students, courses, and enrollments.

## Application Menu

| Option | Operation |
|---|---|
| 1–4 | Department CRUD |
| 5–8 | Student CRUD |
| 9–12 | Course CRUD |
| 13–16 | Enrollment operations |
| 17 | View enrollment report |
| 18 | View audit logs |
| 0 | Exit |

## Git Workflow

The project uses Git for version control and Conventional Commit messages, for example:

```text
feat: add enrollment report menu
feat: add audit log repository
fix: improve application validation and error handling
docs: add project README
```

## Future Improvements

- Add automated unit and integration tests.
- Use database transactions to keep business operations and audit records consistent.
- Improve centralized logging and error reporting.
- Add a graphical user interface or web API.
