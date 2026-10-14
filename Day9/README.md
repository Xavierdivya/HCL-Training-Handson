# Day 9 - Training Management System

## 📌 Project Overview

The **Training Management System** is a console-based application developed using C# and .NET. It helps manage training-related information through a structured design that separates data models, business logic, and application execution.

The project follows a modular architecture to improve code organization, readability, and maintainability.

## 🎯 Objectives

- Understand object-oriented programming concepts in C#.
- Implement a layered project structure.
- Separate data models and business logic.
- Practice collections, methods, and exception handling where applicable.
- Develop a menu-driven console application.

## 🛠️ Technologies Used

- **Programming Language:** C#
- **Framework:** .NET
- **IDE:** Visual Studio 2026
- **Application Type:** Console Application

## 📂 Project Structure

```text
TrainingManagementSystem/
│
├── Models/
│   └── Classes for representing application data
│
├── Services/
│   └── EnrollmentService.cs
│
├── Utilities/
│   └── Helper classes and utility methods
│
├── Program.cs
└── TrainingManagementSystem.csproj
```

## 🏗️ Architecture

The application uses a layered structure:

- **Models:** Represents the data used by the application.
- **Services:** Contains business logic and enrollment-related operations.
- **Utilities:** Provides reusable helper methods and supporting functionality.
- **Program.cs:** Entry point of the application that starts and manages the program flow.

## ⚙️ Features

- Organized training management functionality.
- Enrollment-related operations through the service layer.
- Separation of application data and business logic.
- Modular and maintainable code structure.
- Console-based interaction.

*Note: Update this feature list to match the exact operations implemented in your source code.*

## ▶️ How to Run the Project

### Prerequisites

- Install the .NET SDK.
- Install Visual Studio with the .NET development workload, or use the .NET CLI.

### Steps

1. Clone or download this repository.
2. Open the `TrainingManagementSystem` project folder.
3. Open the `.csproj` file in Visual Studio, or open a terminal in the project directory.
4. Restore the dependencies:

   ```bash
   dotnet restore
   ```

5. Build the project:

   ```bash
   dotnet build
   ```

6. Run the application:

   ```bash
   dotnet run
   ```

## 📚 Learning Outcomes

- Gained practical experience in C# and .NET development.
- Learned to organize code using namespaces and classes.
- Practiced separating models, services, and application entry-point logic.
- Improved understanding of modular programming and maintainable project design.

## 🚀 Future Enhancements

- Add database integration using SQL Server.
- Implement user authentication and role-based access.
- Add training and enrollment search functionality.
- Develop a graphical user interface or web-based interface.
- Generate enrollment reports.

## 👩‍💻 Training Details

- **Repository:** HCL Training Hands-on
- **Day:** 9
- **Project:** Training Management System
- **Language:** C#

---

*Developed as part of HCL training hands-on practice.*
