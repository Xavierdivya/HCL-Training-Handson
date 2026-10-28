# Student Course Management System

A menu-driven console application built with **C# and .NET 10** that
manages departments, students, courses, enrollments, and audit logs
using **SQL Server**. The project is a practical exercise in
object-oriented programming, ADO.NET, asynchronous programming,
validation, reporting, transactions, and Git workflows.

## Contents

-   [Features](#features)
-   [Technology Stack](#technology-stack)
-   [Project Structure](#project-structure)
-   [Prerequisites](#prerequisites)
-   [Database Configuration](#database-configuration)
-   [Build and Run](#build-and-run)
-   [Testing and Verification](#testing-and-verification)
-   [Git Workflow](#git-workflow)
-   [Documentation and Change
    Tracking](#documentation-and-change-tracking)
-   [Known Follow-up Work](#known-follow-up-work)

## Features

### Department Management

-   Create, view, update, and delete department records, where
    implemented.
-   Search departments by name.
-   Report student counts by department.

### Student Management

-   Create, view, update, and delete student records, where implemented.
-   Validate student input.
-   View a student's enrollments.

### Course Management

-   Create, view, update, and delete course records, where implemented.
-   Check course activity before allowing an enrollment.

### Enrollment Management

-   Create and view enrollments.
-   View enrollments by student or by course.
-   Validate that the selected student and course are active.
-   Prevent duplicate student-course enrollments with a database unique
    constraint.
-   Enforce a maximum of five active enrollments per student in
    application logic.
-   Create the enrollment and its audit record within a single SQL
    transaction.

### Reports and Audit Logging

-   Count students by department.
-   Count enrollments by course.
-   Generate a student enrollment report.
-   Record enrollment creation in the audit log.
-   View audit records where the corresponding menu operation is
    implemented.

### Programming Concepts

-   C# classes and objects
-   A model / repository / service separation of responsibilities
-   ADO.NET and `Microsoft.Data.SqlClient`
-   `async` / `await`
-   Parameterized SQL commands
-   Input validation and exception handling
-   SQL transactions and rollback
-   Git version control and Conventional Commit messages

> **Implementation note:** This README describes the features discussed
> and tested during development. Check the source code and current
> console menu to confirm the exact operations available in your
> checkout.

## Technology Stack

-   **Language:** C#
-   **Framework:** .NET 10
-   **Database:** SQL Server Express
-   **Data access:** ADO.NET
-   **SQL client:** `Microsoft.Data.SqlClient`
-   **Version control:** Git
-   **Hosting:** GitHub

## Project Structure

The names below reflect the intended organization. If your actual
project uses a different folder name, follow the structure in your
checkout.

``` text
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
├── docs/
│   ├── Models/
│   └── CHANGELOG.md
├── Program.cs
└── README.md
```

## Prerequisites

-   Windows or another environment supported by .NET 10
-   .NET 10 SDK
-   Visual Studio 2026 or another compatible C# editor
-   SQL Server Express (or another SQL Server instance)
-   SQL Server Management Studio (SSMS), recommended
-   Git

## Database Configuration

The development environment for this project has used the database
`StudentCourseDB` and the SQL Server Express instance `.\SQLEXPRESS`.

A Windows Authentication connection string may look like this:

``` csharp
@"Server=.\SQLEXPRESS;Database=StudentCourseDB;" +
@"Trusted_Connection=True;TrustServerCertificate=True;";
```

Check `DatabaseHelper.cs` (or the equivalent helper path in your
project) and make sure the connection string matches your SQL Server
installation.

Before running the application:

1.  Start the SQL Server service.
2.  Create or restore `StudentCourseDB`.
3.  Run the database schema/setup script used by your project.
4.  Confirm that the required tables exist, including `Departments`,
    `Students`, `Courses`, `Enrollments`, and `AuditLogs`.
5.  Confirm that the application account has the required database
    permissions.

**Security:** Do not commit passwords, access tokens, or other secrets
to GitHub. Keep local credentials out of source control.

## Build and Run

Open PowerShell in the project directory:

``` powershell
cd D:\HCL\Day18\StudentCourseManagement
dotnet restore
dotnet build
dotnet run
```

Follow the menu displayed by the application and enter the option number
for the operation you want to perform.

## Testing and Verification

The following checks have been completed during development:

-   `dotnet build` succeeded on .NET 10.
-   Enrollment creation returned a new enrollment ID.
-   A SQL query confirmed that the tested enrollment was saved.
-   A second SQL query confirmed that the corresponding audit log was
    saved.
-   A duplicate student-course enrollment was rejected by the database
    unique constraint.

The maximum-five-active-enrollments rule is implemented in application
logic, but a dedicated runtime test of that limit remains a follow-up
task.

### Verify an Enrollment and Its Audit Record

Replace `8` with an enrollment ID created during your own test:

``` sql
USE StudentCourseDB;
GO

SELECT *
FROM Enrollments
WHERE EnrollmentId = 8;

SELECT *
FROM AuditLogs
WHERE EntityName = 'Enrollment'
  AND EntityId = 8
  AND Action = 'Created';
```

## Git Workflow

Check your current branch and working tree:

``` bash
git status
```

Stage and commit a documentation change:

``` bash
git add README.md docs
git commit -m "docs: improve project and model documentation"
```

Push the current branch (the project has used `master` during
development):

``` bash
git push origin master
```

If your repository uses `main`, push `main` instead. Check
`git branch --show-current` to confirm the branch name.

### Conventional Commit Examples

  -----------------------------------------------------------------------------------------------
  Type                    Example                                         Use
  ----------------------- ----------------------------------------------- -----------------------
  `feat`                  `feat: add student enrollment report`           Add functionality

  `fix`                   `fix: handle invalid student input`             Fix a defect

  `refactor`              `refactor: simplify enrollment validation`      Restructure without
                                                                          changing intended
                                                                          behavior

  `test`                  `test: cover duplicate enrollment validation`   Add or update tests

  `docs`                  `docs: document enrollment model`               Documentation only

  `chore`                 `chore: update project configuration`           Maintenance
  -----------------------------------------------------------------------------------------------

## Documentation and Change Tracking

Each model has a separate document under [`docs/Models`](docs/Models/).
Use it to record: - The model's purpose and responsibilities -
Properties and their meanings - Related database table - Validation
rules and relationships - The commits that changed the model - Tests or
checks performed after a change

Use [`docs/CHANGELOG.md`](docs/CHANGELOG.md) as a project-level
timeline. Update the relevant model document whenever a model changes,
and add a project-level entry when a feature or milestone is completed.

## Known Follow-up Work

-   Add automated unit and integration tests.
-   Run a dedicated runtime test for the five-active-enrollment limit.
-   Review error messages so common validation failures are easy for
    users to understand.
-   Keep the database setup/schema script in the repository and document
    how to run it.
-   Continue documenting model and feature changes alongside their
    commits.
