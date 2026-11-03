# Student Course Management System

A menu-driven **C# console application** for managing departments, students, courses, and student course enrollments. The project uses **Entity Framework Core (EF Core) Code First** with **SQL Server Express** for database operations.

## Features

### Department Management
- Add a department with a required name and optional location.
- View all departments and retrieve a department by ID.
- Update department details.
- Delete a department.
- Prevent duplicate department names through a unique database index.

### Student Management
- Add, view, update, and delete students.
- Associate each student with an existing department.
- Display department information with student records.
- Enforce unique email addresses.
- Validate student age (17–60) and required fields.

### Course Management
- Add, view, update, and delete courses.
- Enforce unique course names.
- Validate course credits (1–6).
- Track whether a course is active.

### Enrollment Management
- Enroll a student in a course.
- View all enrollments or retrieve an enrollment by ID.
- View enrollments for a particular student.
- Cancel an enrollment.
- Store the enrollment date and time.
- Validate that the referenced student and course exist.

> The current data model allows more than one enrollment record for the same student-course pair.

## Technology Stack

| Technology | Usage |
|---|---|
| C# | Application language |
| .NET 10 | Runtime and target framework |
| Entity Framework Core 10 | ORM and database access |
| SQL Server Express | Relational database |
| EF Core Migrations | Database schema versioning |
| Microsoft.Extensions.Hosting | Generic Host and dependency-injection container |

## Project Structure

```text
StudentCourseEFCore/
├── Data/
│   └── AppDbContext.cs
├── Helper/
│   └── ConsoleHelper.cs
├── Menus/
│   ├── CourseMenu.cs
│   ├── DepartmentMenu.cs
│   ├── EnrollmentMenu.cs
│   └── StudentMenu.cs
├── Migrations/
│   ├── ..._InitialCreate.cs
│   ├── ..._AddStudentEntity.cs
│   ├── ..._AddStudentValidationConstraints.cs
│   ├── ..._AddCourseEntity.cs
│   ├── ..._AddEnrollmentEntity.cs
│   └── AppDbContextModelSnapshot.cs
├── Models/
│   ├── Course.cs
│   ├── Department.cs
│   ├── Enrollment.cs
│   └── Student.cs
├── Repositories/
│   ├── CourseRepository.cs
│   ├── DepartmentRepository.cs
│   ├── EnrollmentRepository.cs
│   └── StudentRepository.cs
├── Services/
│   ├── CourseService.cs
│   ├── DepartmentService.cs
│   ├── EnrollmentService.cs
│   └── StudentService.cs
├── docs/
│   ├── 02-dbcontext.md
│   ├── 03-department-entity-and-migration.md
│   ├── 04-department-crud-and-validation.md
│   ├── 05-student-entity-relationship-and-validation.md
│   ├── 06-course-entity-and-migration.md
│   ├── 07-enrollment-management.md
│   └── 08-dependency-injection.md
├── Program.cs
├── StudentCourseEFCore.csproj
├── StudentCourseEFCore.slnx
└── README.md
```

## Database Design

The application contains four main entities:

- **Department** — stores department name, location, and active status.
- **Student** — stores student details and the department foreign key.
- **Course** — stores course name, credit value, and active status.
- **Enrollment** — links a student to a course and records when the enrollment was created.

Relationship overview:

```text
Department 1 ───────── * Student

Student    1 ───────── * Enrollment * ───────── 1 Course
```

The Student–Course many-to-many relationship is represented by the `Enrollment` entity.

### Validation and Database Rules

| Entity | Rule |
|---|---|
| Department | Department name is required, maximum 100 characters, and unique |
| Department | Location is optional, maximum 100 characters |
| Student | Name is required, maximum 100 characters |
| Student | Email is required, maximum 450 characters, and unique |
| Student | Age must be between 17 and 60 |
| Student | A valid department is required; department deletion is restricted while students reference it |
| Course | Course name is required, maximum 100 characters, and unique |
| Course | Credits must be between 1 and 6 |
| Enrollment | Student and Course foreign keys are required |
| Enrollment | Enrollment timestamp defaults to SQL Server `SYSDATETIME()` |

These rules are configured in `Data/AppDbContext.cs` using EF Core Fluent API and database constraints.

## Prerequisites

Install or configure the following before running the application:

1. **.NET 10 SDK**
2. **SQL Server Express** (or another SQL Server instance)
3. **SQL Server Management Studio (SSMS)** — optional, useful for viewing the database
4. **EF Core command-line tool**

Check the .NET SDK:

```bash
dotnet --version
```

Install the EF Core CLI tool if it is not already installed:

```bash
dotnet tool install --global dotnet-ef
```

If it is already installed, update it:

```bash
dotnet tool update --global dotnet-ef
```

## Configuration

The current connection string is configured in both `Program.cs` and `Data/AppDbContext.cs`:

```text
Server=.\SQLEXPRESS;Database=StudentCourseEFCoreDB;Trusted_Connection=True;TrustServerCertificate=True;
```

Before running the project:

1. Make sure SQL Server Express is installed and its service is running.
2. Confirm that your SQL Server instance name is `SQLEXPRESS`.
3. If your instance name is different, update the connection string in both locations.
4. Windows integrated authentication is used; ensure your Windows account has permission to create and access the database.

The database name is `StudentCourseEFCoreDB`. EF Core migrations create/update its schema; you do not need to manually create the tables.

> For a shared or production environment, move the connection string into configuration (for example, user secrets or environment-specific settings) instead of keeping it hardcoded in source code.

## Run the Application

Open a terminal in the folder containing `StudentCourseEFCore.csproj`, then run:

```bash
dotnet restore
dotnet build
dotnet ef database update
dotnet run
```

The migration command applies all pending migrations to the configured SQL Server database. The application then displays the main menu:

```text
===== STUDENT COURSE MANAGEMENT =====
1. Department Management
2. Student Management
3. Course Management
4. Enrollment Management
0. Exit
```

Choose a menu option and follow the prompts. Use `0` to exit a submenu or the main application, as applicable.

## Useful EF Core Commands

Run these commands from the project directory:

```bash
# List available migrations
dotnet ef migrations list

# Apply pending migrations
dotnet ef database update

# Create a new migration after changing the EF Core model
dotnet ef migrations add MigrationName

# Roll the database back to a previous migration
dotnet ef database update PreviousMigrationName
```

Do not create a new migration just to run the existing project. The migrations are already included in the repository.

## Application Architecture

The code is organized into layers:

| Layer | Responsibility |
|---|---|
| Models | Defines Department, Student, Course, and Enrollment entities |
| Data | Defines `AppDbContext`, DbSets, relationships, and Fluent API constraints |
| Repositories | Contains database query and persistence operations |
| Services | Provides an application-facing layer over repositories |
| Menus | Handles console menus, prompts, and user interaction |
| Helper | Contains reusable console input and display methods |
| Migrations | Tracks database schema changes |

The application uses constructor injection and registers the DbContext, repositories, services, and menu classes through the .NET Generic Host in `Program.cs`.

**Implementation note:** The current code is a learning-stage implementation with mixed context-management patterns. Some repository classes create `AppDbContext` directly, while the Course repository receives a context through constructor injection. The DI registrations are present, but the repository layer is not yet consistently using the injected context. This can be standardized as a future refactoring.

## Learning Documentation

The `docs/` directory contains phase-wise notes covering DbContext, entity configuration, migrations, CRUD operations, validation, relationships, enrollment management, and dependency injection.

## Current Scope and Possible Improvements

- Standardize all repositories to use constructor-injected `AppDbContext`.
- Move the SQL Server connection string to configuration or user secrets.
- Add automated unit and integration tests.
- Add duplicate-enrollment prevention if the business rules require it.
- Improve centralized exception handling and logging.
- Add search, filtering, and pagination for larger datasets.

## Author

**Divya Xavier**

---

This project was developed as part of hands-on .NET and Entity Framework Core practice.
