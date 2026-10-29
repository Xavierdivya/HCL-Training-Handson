# Leave Management System — Project Learning Guide

## 1. Project Overview

**Project Name:** LeaveManagementSystem

**Location:** `D:\HCL\Day20\LeaveManagementSystem`

**Purpose:** Build a leave management application using C#, Entity Framework Core, and SQL Server.

Employees will eventually be able to submit leave requests, view their requests, and track approval status. Managers will be able to approve or reject requests.

## 2. Technology Stack

- C#
- .NET Console Application
- Entity Framework Core
- SQL Server
- Git and GitHub
- Conventional Commits

## 3. Project Structure

- `Models/` — entity classes representing application data
- `Data/` — database context and database configuration
- `Services/` — application business logic
- `docs/` — learning notes and project documentation
- `Program.cs` — application entry point

## 4. Development Workflow

For each development step:

1. Write the code.
2. Understand the logic.
3. Build and test the application.
4. Update this documentation.
5. Stage only the intended files.
6. Create a Conventional Commit.
7. Push the changes to GitHub when appropriate.

## 5. Development Log

### Step 1 — Project Initialization

**Status:** In progress

**Tasks:**
- Create the Day20 folder.
- Create the LeaveManagementSystem console application.
- Create Models, Data, Services, and docs folders.
- Add this learning guide.

**Concepts learned:**
- `dotnet new console` creates a C# console project.
- `mkdir` creates a directory.
- `Program.cs` contains the application's entry-point code.
- Git repository root: `D:\HCL`

**Build result:** Successful.

**Commit:** Pending verification.

## 6. Database Design

Planned entities:

- Employee
- LeaveType
- LeaveRequest

Relationships, keys, validation rules, and migrations will be documented as they are implemented.

## 7. Git Commit Convention

Use the format:

`type: short description`

Examples:

- `chore: initialize leave management project`
- `docs: add project learning guide`
- `feat: add employee model`
- `fix: validate leave request dates`
- `test: add leave request validation tests`

## 8. Issues and Solutions

Record errors, their causes, and the solutions applied during development.

## 9. Progress Tracker

- [x] Plan project structure
- [ ] Verify project builds
- [ ] Commit project initialization
- [ ] Create entity models
- [ ] Configure EF Core
- [ ] Create database migration
- [ ] Implement CRUD operations
- [ ] Implement leave approval workflow
- [ ] Test and document the application
