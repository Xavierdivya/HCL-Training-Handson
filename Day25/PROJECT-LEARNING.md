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




# Unit 1 – College Representative Authentication and Approval

## 1. Objective

Implemented authentication and the college representative registration and approval workflow using ASP.NET Core Identity, JWT authentication, Entity Framework Core, and SQL Server.

The workflow ensures that a college representative cannot access protected resources until the SuperAdmin approves the associated college and representative account.

## 2. Authentication Configuration

Implemented JWT Bearer authentication.

Configuration is stored securely using .NET User Secrets rather than committing signing keys to `appsettings.json`.

Configuration keys:

- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:ExpiryMinutes`
- `Jwt:Key`

The JWT contains the following claims:

- User ID
- Full name
- Email
- Role
- College ID (for college representatives)

JWT validation checks the signing key, issuer, audience, and token lifetime.

## 3. College Representative Registration

Implemented:

`POST /api/auth/register/college-representative`

The registration process:

1. Receives representative and college details.
2. Checks whether the college code already exists.
3. Checks whether the email is already registered.
4. Creates the college with Pending status.
5. Creates the representative Identity account with Pending status.
6. Associates the representative with the newly created college.
7. Assigns the `PlacementOfficer` role.
8. Saves the registration within a database transaction.

Successful registration returns **201 Created**.

### Registration verification

Test account:

- Email: `representative@testcollege.com`
- College: Test Engineering College
- College code: `TEST001`
- College ID: `3`

The database initially showed:

- College status: Pending
- Representative account status: Pending

## 4. SuperAdmin Approval Workflow

Only users with the `SuperAdmin` role can access college management endpoints.

Implemented operations:

- Approve college
- Reject college
- Suspend college
- View all colleges
- View college details
- View pending colleges

When a college is approved, its representative accounts are also updated to Approved.

When a college is rejected, its representative accounts are updated to Rejected.

When a college is suspended, its representative accounts are updated to Suspended.

These changes are persisted together through the shared Entity Framework Core DbContext.

## 5. Login and Current User

Implemented:

- `POST /api/auth/login`
- `GET /api/auth/me`

Login verifies the user's credentials and account status.

For college representatives, login also verifies that the associated college is Approved.

A JWT is generated only when the account and associated college satisfy the required status checks.

The current-user endpoint returns the authenticated user's profile, role, account status, and college information where applicable.

## 6. Authorization

Configured JWT authentication and role-based authorization.

Important middleware order:

1. `UseAuthentication()`
2. `UseAuthorization()`

College management endpoints are protected using:

`[Authorize(Roles = AppRoles.SuperAdmin)]`

A representative attempting to access a SuperAdmin-only endpoint receives **403 Forbidden** when authenticated but lacking the required role.

An unauthenticated or invalid login request receives **401 Unauthorized**.

## 7. Testing Results

| Test | Expected result | Actual result |
|---|---|---|
| Representative registration | 201 Created | Passed |
| SuperAdmin login | 200 OK | Passed |
| College approval | Successful approval | Passed |
| Representative login after approval | 200 OK | Passed |
| Get current user | 200 OK | Passed |
| Representative accesses SuperAdmin college endpoint | 403 Forbidden | Passed |
| College rejection | Successful rejection | Passed |
| Rejected representative login | 401 Unauthorized | Passed |
| College suspension | Successful suspension | Passed |
| Suspended representative login | 401 Unauthorized | Passed |

## 8. Important Learning Outcomes

- Integrated ASP.NET Core Identity with a custom ApplicationUser.
- Implemented JWT token generation and validation.
- Used User Secrets for sensitive JWT configuration.
- Implemented role-based authorization.
- Connected a representative account to a college through a foreign key.
- Used database transactions during representative registration.
- Synchronized college and representative account statuses.
- Understood the difference between 401 Unauthorized and 403 Forbidden.
- Tested authentication and authorization scenarios using Postman.

## 9. Notes and Future Improvements

- Email confirmation is not implemented yet. The current representative registration flow sets `EmailConfirmed` to true as an interim implementation.
- Student and CompanyHR registration workflows will be implemented in their respective modules.
- Additional authorization policies and resource-based access checks will be introduced as the application modules are developed.
- College ID 3 was suspended during testing. It should not be reused for active-college workflow testing.


# Unit 2 – College and Placement Officer Management

## 1. Overview

This unit extends the College module and introduces Placement Officer profile management.

A Placement Officer is associated with one College and an Identity user account. The Placement Officer role is reused for college representatives; a separate CollegeRepresentative role is not required.

## 2. PlacementOfficer Entity

Created:

`Models/PlacementOfficer.cs`

The entity contains:

- `PlacementOfficerId` – Primary key.
- `UserId` – Identity user foreign key.
- `CollegeId` – Associated college foreign key.
- `Designation` – Placement Officer's designation.
- `CreatedAt` – Profile creation timestamp.
- `UpdatedAt` – Optional last-update timestamp.
- `User` – Navigation property to ApplicationUser.
- `College` – Navigation property to College.

## 3. Database Configuration

Updated:

`Data/ApplicationDbContext.cs`

Added the `PlacementOfficers` DbSet and configured:

- `PlacementOfficerId` as the primary key.
- `UserId` as required and uniquely indexed.
- `Designation` as required with a maximum length of 100.
- One-to-one relationship between PlacementOfficer and ApplicationUser.
- Many-to-one relationship between PlacementOfficer and College.
- Restrict delete behavior for both relationships.

Created and applied the `AddPlacementOfficer` EF Core migration.

The `PlacementOfficers` table is now available in `CampusPlacementDB`.

## 4. Placement Officer DTO

Created:

`DTOs/PlacementOfficers/PlacementOfficerResponseDto.cs`

The response DTO exposes:

- Placement Officer ID.
- Identity User ID.
- Full name.
- Email.
- Phone number.
- College ID and college name.
- Designation.
- Profile creation date.

DTOs prevent controllers from directly exposing database entities.

## 5. Repository Layer

Created:

- `Repositories/Interfaces/IPlacementOfficerRepository.cs`
- `Repositories/Implementations/PlacementOfficerRepository.cs`

Implemented operations:

- Retrieve all Placement Officer profiles.
- Retrieve a profile by Placement Officer ID.
- Retrieve a profile by Identity User ID.
- Add a Placement Officer profile.
- Save database changes.

Related User and College navigation properties are loaded when retrieving profiles.

Read queries use `AsNoTracking()`.

## 6. Service Layer

Created:

- `Services/Interfaces/IPlacementOfficerService.cs`
- `Services/Implementations/PlacementOfficerService.cs`

The service retrieves Placement Officer profiles and maps entity data to `PlacementOfficerResponseDto`.

It keeps business logic and DTO mapping outside the controller.

## 7. Controller Layer

Created:

`Controllers/PlacementOfficersController.cs`

Implemented endpoints:

| HTTP method | Endpoint | Authorization |
|---|---|---|
| GET | `/api/placementofficers` | SuperAdmin |
| GET | `/api/placementofficers/{id}` | SuperAdmin |
| GET | `/api/placementofficers/me` | PlacementOfficer |

The `/me` endpoint obtains the authenticated user's ID from the JWT NameIdentifier claim and retrieves that user's Placement Officer profile.

## 8. Registration Integration

Updated:

- `DTOs/Auth/CollegeRepresentativeRegistrationDto.cs`
- `Services/Implementations/AuthService.cs`

Added the `Designation` property to the college representative registration DTO.

During registration, the authentication service now:

1. Creates a Pending College.
2. Creates the associated Pending Identity user.
3. Assigns the `PlacementOfficer` role.
4. Creates the PlacementOfficer profile using the new Identity user ID and College ID.
5. Saves the profile within the existing database transaction.
6. Commits the transaction after all registration operations succeed.

If registration fails, the transaction is rolled back.

This ensures that every newly registered college representative has a corresponding Placement Officer profile.

## 9. Dependency Injection

Updated `Program.cs` to register:

- `IPlacementOfficerRepository` with `PlacementOfficerRepository`.
- `IPlacementOfficerService` with `PlacementOfficerService`.

## 10. Build Verification

The following implementation changes were completed:

- PlacementOfficer model.
- EF Core database configuration and migration.
- Placement Officer DTO.
- Repository interface and implementation.
- Service interface and implementation.
- Placement Officers controller.
- Dependency injection registrations.
- College representative registration integration.

**Build result:** Succeeded.

API/Postman testing is intentionally deferred until the complete project implementation is finished.

## 11. Key Learning Points

- How to model a one-to-one relationship with ASP.NET Core Identity.
- How to associate a business profile with both an Identity user and an organization.
- How to configure EF Core relationships and unique indexes.
- How to use repository and service layers for profile retrieval.
- How to map database entities to response DTOs.
- How to obtain the current authenticated user's ID from JWT claims.
- How to create related records inside a database transaction.
- How to register repository and service dependencies in ASP.NET Core.

## 12. Unit 2 Completion

**Unit:** College and Placement Officer Management

**Status:** Implementation completed; build succeeded.

**Testing:** Deferred until all project units are implemented.

**Next unit:** Company and Company HR Management.
