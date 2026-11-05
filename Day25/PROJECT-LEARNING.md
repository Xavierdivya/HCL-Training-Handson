# Day25 – Campus Placement Management System

## 1. Project Overview

Campus Placement Management is a backend application developed using ASP.NET Core Web API and .NET 10.

The purpose of this application is to manage college placement activities by connecting colleges, placement officers, companies, company HR representatives, and students through a centralized platform.

The system will support college registration and approval, placement officer management, student enrollment, company registration, job postings, job applications, recruitment drives, interview scheduling, and placement reports.

### Project Objectives

- Provide a centralized college placement management platform.
- Implement secure authentication and role-based authorization.
- Manage college and company registration and approval workflows.
- Allow placement officers to manage students belonging to their colleges.
- Allow company HR representatives to manage recruitment activities.
- Allow students to view eligible jobs and track their applications.
- Maintain accurate recruitment and placement records.

---

## 2. Technology Stack

| Technology | Purpose |
|---|---|
| C# | Backend programming language |
| .NET 10 | Application framework |
| ASP.NET Core Web API | REST API development |
| Entity Framework Core 10 | ORM and database operations |
| Microsoft SQL Server | Relational database |
| ASP.NET Core Identity | User and role management |
| JWT Bearer Authentication | API authentication |
| Postman | API testing |
| Visual Studio | Development environment |
| Git and GitHub | Version control |

---

## 3. System Architecture

The application follows a layered architecture.

### Presentation Layer

Contains API controllers that receive HTTP requests and return HTTP responses.

Example:
- CollegesController

### Business Logic Layer

Contains services that implement application rules and validations.

Example:
- ICollegeService
- CollegeService

### Data Access Layer

Contains repositories that communicate with the database using Entity Framework Core.

Example:
- ICollegeRepository
- CollegeRepository

### Database Layer

SQL Server stores application users, colleges, companies, students, jobs, applications, and other placement-related information.

### Request Flow

Client → Controller → Service → Repository → ApplicationDbContext → SQL Server

The response travels back through the same layers.

---

## 4. Initial Project Setup

### Step 1: Create the ASP.NET Core Web API

Created the project:

CampusPlacementManagement

Project location:

`D:\HCL\Day25\CampusPlacementManagement`

Target framework:

.NET 10

The project uses controllers and OpenAPI support.

### Step 2: Install NuGet Packages

The following packages were installed:

- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.Design
- Microsoft.AspNetCore.OpenApi

These packages provide Identity integration, SQL Server connectivity, EF Core migrations, and OpenAPI documentation.

---

## 5. Authentication Foundation

### 5.1 ApplicationUser Model

File:

`Models/Identity/ApplicationUser.cs`

ApplicationUser extends the ASP.NET Core Identity IdentityUser class.

Additional properties:

| Property | Description |
|---|---|
| FullName | Full name of the user |
| AccountStatus | Current account approval status |
| CreatedAt | Account creation timestamp |
| UpdatedAt | Last update timestamp |

The AccountStatus property determines whether an account is pending, approved, rejected, suspended, or inactive.

### 5.2 AccountStatus Enum

File:

`Models/Enums/AccountStatus.cs`

Values:

1. Pending
2. Approved
3. Rejected
4. Suspended
5. Inactive

The default account status is Pending.

### 5.3 ApplicationDbContext

File:

`Data/ApplicationDbContext.cs`

ApplicationDbContext inherits from:

`IdentityDbContext<ApplicationUser>`

This allows the application to use ASP.NET Core Identity tables together with application-specific entities.

The DbContext contains:

- Identity configuration.
- ApplicationUser property configuration.
- College entity configuration.
- College database constraints and indexes.

The AccountStatus and College Status enums are stored as strings using EF Core HasConversion<string>().

The CollegeCode column has a unique index to prevent duplicate college codes.

---

## 6. Database Configuration and Migrations

### 6.1 SQL Server Connection

The application uses SQL Server Express.

Database name:

`CampusPlacementDB`

Connection string is configured through the DefaultConnection setting in appsettings.json.

The application connects through:

`UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))`

### 6.2 Initial Identity Migration

Created and applied the initial Identity migration:

`InitialIdentityCreate`

This migration created the ASP.NET Core Identity database tables.

### 6.3 College Migration

Created and applied the College entity migration:

`AddCollegeEntity`

This migration created the Colleges table with its configured columns and unique CollegeCode index.

### 6.4 Database Verification

Verified the database and Colleges table using SQL Server Management Studio.

Verification query:

```sql
USE CampusPlacementDB;
GO

SELECT *
FROM Colleges;
```

The registered college record was successfully stored in the database.

---

## 7. Identity Roles and SuperAdmin Seeding

### 7.1 AppRoles

File:

`Authorization/AppRoles.cs`

The application defines four roles:

- SuperAdmin
- PlacementOfficer
- CompanyHR
- Student

These roles are used for role-based authorization.

### 7.2 IdentitySeeder

File:

`Data/Seed/IdentitySeeder.cs`

IdentitySeeder performs the following operations:

1. Creates the required Identity roles if they do not exist.
2. Checks whether the initial SuperAdmin account exists.
3. Creates the SuperAdmin account if necessary.
4. Assigns the SuperAdmin role.
5. Marks the seeded SuperAdmin email as confirmed.
6. Sets the SuperAdmin account status to Approved.

SuperAdmin credentials are retrieved from User Secrets rather than being hard-coded in the source code.

Configuration keys:

- SuperAdmin:Email
- SuperAdmin:Password
- SuperAdmin:FullName

This keeps sensitive credentials outside the application source code.

---

## 8. College Module – Implementation

The College module is the first business module implemented in the application.

Its first feature is public college registration.

### 8.1 CollegeStatus Enum

File:

`Models/Enums/CollegeStatus.cs`

Values:

1. Pending
2. Approved
3. Rejected
4. Suspended

A newly registered college receives Pending status.

### 8.2 College Model

File:

`Models/College.cs`

The College model represents a registered educational institution.

Properties:

| Property | Description |
|---|---|
| CollegeId | Primary key |
| CollegeName | Name of the college |
| CollegeCode | Unique college identification code |
| UniversityName | Associated university |
| OfficialEmail | Official college email |
| PhoneNumber | Contact number |
| Address | College address |
| Website | College website |
| Status | College approval status |
| CreatedAt | Registration timestamp |

CollegeCode is unique to prevent duplicate registrations.

### 8.3 College Registration DTO

File:

`DTOs/Colleges/CollegeRegistrationDto.cs`

The DTO receives registration details from the client.

Validation attributes used:

- Required
- StringLength
- EmailAddress
- Phone
- Url

DTOs prevent clients from directly submitting or modifying database entity properties that should remain controlled by the application.

### 8.4 College Response DTO

File:

`DTOs/Colleges/CollegeResponseDto.cs`

This DTO contains the information returned to the API client.

It includes CollegeId, CollegeName, CollegeCode, UniversityName, OfficialEmail, PhoneNumber, Address, Website, Status, and CreatedAt.

### 8.5 College Status Update DTO

File:

`DTOs/Colleges/CollegeStatusUpdateDto.cs`

This DTO contains a Remarks property with a maximum length of 500 characters.

It is intended for the college approval and status management workflow. The approval workflow has not yet been implemented.

---

## 9. Repository Layer

### 9.1 ICollegeRepository

File:

`Repositories/Interfaces/ICollegeRepository.cs`

The repository interface defines database operations required by the College module.

Methods:

- GetAllAsync()
- GetByIdAsync(int collegeId)
- GetByCodeAsync(string collegeCode)
- AddAsync(College college)
- SaveChangesAsync()

These methods separate database access from business logic.

### 9.2 CollegeRepository

File:

`Repositories/Implementations/CollegeRepository.cs`

CollegeRepository implements ICollegeRepository.

It uses ApplicationDbContext to communicate with SQL Server.

Important EF Core methods:

**ToListAsync()**

Retrieves multiple records asynchronously.

**FirstOrDefaultAsync()**

Retrieves the first matching record or null.

**AsNoTracking()**

Retrieves records without tracking them in the EF Core change tracker. It is useful for read-only queries.

**AddAsync()**

Adds a new college entity to the DbContext.

**SaveChangesAsync()**

Persists changes to the database.

---

## 10. Service Layer

### 10.1 ICollegeService

File:

`Services/Interfaces/ICollegeService.cs`

Defines the business operations exposed by the College service.

Methods:

- RegisterAsync()
- GetAllAsync()
- GetByIdAsync()

### 10.2 CollegeService

File:

`Services/Implementations/CollegeService.cs`

CollegeService contains the business logic for college registration and retrieval.

#### Registration workflow

1. Receives CollegeRegistrationDto.
2. Trims and normalizes CollegeCode to uppercase.
3. Checks whether the CollegeCode already exists.
4. Throws InvalidOperationException if the code is already registered.
5. Creates a new College entity.
6. Sets its status to Pending.
7. Adds the entity through the repository.
8. Saves changes to SQL Server.
9. Maps the College entity to CollegeResponseDto.
10. Returns the response DTO.

#### Why normalize CollegeCode?

For example:

- abc001
- Abc001
- ABC001

All are normalized to:

ABC001

This prevents duplicate codes caused by differences in letter casing.

#### DTO Mapping

The service maps the entity to CollegeResponseDto instead of returning the database entity directly.

This keeps the database model separate from the API response contract.

---

## 11. Controller Layer

### CollegesController

File:

`Controllers/CollegesController.cs`

The controller exposes the College Registration API.

Base route:

`/api/colleges`

### Implemented Endpoints

| HTTP Method | Endpoint | Access |
|---|---|---|
| POST | /api/colleges/register | Public |
| GET | /api/colleges | SuperAdmin |
| GET | /api/colleges/{id} | SuperAdmin |

### POST /api/colleges/register

Purpose:

Allows a college to submit its registration details.

Authorization:

`[AllowAnonymous]`

The endpoint accepts CollegeRegistrationDto.

On successful registration, it returns:

`201 Created`

If the CollegeCode already exists, it returns:

`409 Conflict`

### GET /api/colleges

Purpose:

Retrieves all registered colleges.

Authorization:

`[Authorize(Roles = AppRoles.SuperAdmin)]`

This endpoint requires a SuperAdmin role.

### GET /api/colleges/{id}

Purpose:

Retrieves a college by its ID.

Authorization:

`[Authorize(Roles = AppRoles.SuperAdmin)]`

Returns 404 when the requested college does not exist.

Note: JWT Bearer authentication and login endpoints are still pending implementation. Therefore, protected endpoint testing is deferred until authentication is configured.

---

## 12. Dependency Injection

File:

`Program.cs`

The College repository and service are registered using scoped dependency injection.

```csharp
builder.Services.AddScoped<
    ICollegeRepository,
    CollegeRepository>();

builder.Services.AddScoped<
    ICollegeService,
    CollegeService>();
```

Scoped lifetime creates one service instance per HTTP request scope.

The application also registers:

- ApplicationDbContext
- ASP.NET Core Identity
- Identity role and user stores
- OpenAPI services
- Controllers

Authentication middleware must execute before authorization middleware.

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

The application maps controllers using:

```csharp
app.MapControllers();
```

---

## 13. College Registration API Testing

Testing was performed using Postman.

### Test 1 – Successful Registration

HTTP Method:

POST

Endpoint:

`/api/colleges/register`

Sample request:

```json
{
  "collegeName": "ABC Engineering College",
  "collegeCode": "ABC001",
  "universityName": "ABC University",
  "officialEmail": "placement@abc.edu",
  "phoneNumber": "9876543210",
  "address": "Thiruvananthapuram, Kerala",
  "website": "https://www.abc.edu"
}
```

Expected result:

`201 Created`

Actual result:

Passed.

The API returned the registered