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



## Unit 4 – Student Management

- Created Student entity and StudentStatus enum.
- Configured Student relationships with ApplicationUser, College, and PlacementOfficer.
- Added Student registration and response DTOs.
- Implemented Student repository and service.
- Added StudentsController with role-based access.
- Created and applied AddStudent migration.
- Verified successful build and database table creation.

## Unit 5 – Job Management

- Created Job entity and JobStatus enum.
- Configured Job relationship with Company in ApplicationDbContext.
- Added JobCreateDto, JobUpdateDto, and JobResponseDto.
- Implemented Job repository and service.
- Registered Job repository and service in Program.cs.
- Created JobsController with role-based access and company ownership checks.
- Created and applied AddJob migration.
- Verified successful build and database table creation.

# Unit 6 – Job Application Management

## 1. Overview

This unit introduces the job application workflow. Students can apply for eligible job opportunities, track their application status, and withdraw applications when permitted.

Company HR representatives review applications belonging to their company. Placement Officers can view applications associated with their college. SuperAdmin can access application information across the platform.

## 2. JobApplication Entity

Created:

`Models/JobApplication.cs`

The entity connects a Student with a Job.

Important data includes:

- JobApplicationId – Primary key.
- JobId – Related job foreign key.
- StudentId – Applicant student foreign key.
- ApplicationDate – Application submission date.
- Status – Current application status.
- CoverLetter – Student's application message, where supplied.
- Remarks – Review remarks, where supplied.
- CreatedAt and UpdatedAt – Audit timestamps.

## 3. Application Status

Created:

`Models/Enums/ApplicationStatus.cs`

The application workflow supports:

1. Submitted
2. UnderReview
3. Shortlisted
4. Rejected
5. Selected
6. Withdrawn

The status represents the current stage of an application.

## 4. Database Configuration

Updated:

`Data/ApplicationDbContext.cs`

Configured:

- JobApplication DbSet.
- Relationship with Job.
- Relationship with Student.
- Restrict delete behavior for dependent application records.
- Unique composite index on JobId and StudentId.
- Status enum conversion to string.
- Application-related property constraints.

The unique composite index prevents the same student from submitting duplicate applications for the same job.

Created and applied the `AddJobApplication` migration.

Verified that the `JobApplications` table was created.

## 5. DTOs

Created DTOs under:

`DTOs/JobApplications/`

The DTO layer separates incoming application requests and outgoing application responses from database entities.

## 6. Repository Layer

Created:

- `Repositories/Interfaces/IJobApplicationRepository.cs`
- `Repositories/Implementations/JobApplicationRepository.cs`

The repository handles application data retrieval, filtering, insertion, updates, and persistence.

## 7. Service Layer

Created:

- `Services/Interfaces/IJobApplicationService.cs`
- `Services/Implementations/JobApplicationService.cs`

The service contains application business rules, including:

- Checking that the student account is approved and active.
- Checking that the job is open and its deadline has not passed.
- Checking that the company is approved.
- Checking student eligibility requirements such as CGPA, active backlogs, department, and degree.
- Preventing duplicate applications.
- Controlling permitted application status transitions.
- Enforcing ownership and organization-level access.

## 8. Controller and Authorization

Created:

`Controllers/JobApplicationsController.cs`

The controller exposes application operations according to the authenticated user's role.

- Students manage their own applications.
- Company HR manages applications for jobs belonging to their company.
- Placement Officers view applications belonging to their college.
- SuperAdmin can access application information across the platform.

Company ownership and college ownership are checked in the application workflow.

## 9. Dependency Injection

Registered the JobApplication repository and service in `Program.cs`.

## 10. Build and Database Verification

Completed:

- JobApplication entity.
- ApplicationStatus enum.
- EF Core configuration.
- Migration creation and database update.
- DTOs.
- Repository interface and implementation.
- Service interface and implementation.
- Controller.
- Dependency injection.

**Build result:** Succeeded.

**Migration result:** `AddJobApplication` applied successfully.

**API/Postman testing:** Deferred until the complete project implementation is finished.

## 11. Key Learning Points

- How to model a many-to-one relationship between JobApplication and Job.
- How to associate an application with a Student.
- How to prevent duplicate records using a composite unique index.
- How to validate business eligibility before accepting an application.
- How to manage workflow statuses using enums.
- How to enforce organization ownership.
- Why business rules belong in the service layer.

---

# Unit 7 – Placement Drive Management

## 1. Overview

This unit introduces placement drive management.

A Placement Officer can organize recruitment drives for their college by selecting an approved company, specifying drive details, defining eligibility criteria, and managing the drive status.

SuperAdmin can view placement drives across the platform. Students can view drives that are open for registration and satisfy the configured availability conditions.

## 2. PlacementDrive Entity

Created:

`Models/PlacementDrive.cs`

Important properties include:

- PlacementDriveId – Primary key.
- CollegeId – Associated college.
- CompanyId – Recruiting company.
- PlacementOfficerId – Officer responsible for the drive.
- DriveName – Name of the placement drive.
- Description – Drive description.
- Venue – Drive location.
- StartDate and EndDate – Drive schedule.
- RegistrationDeadline – Last date for registration.
- MinimumCGPA – Minimum academic requirement.
- MaximumActiveBacklogs – Maximum allowed active backlogs.
- EligibleDepartments – Eligible departments.
- EligibleDegrees – Eligible degree programmes.
- GraduationYearFrom and GraduationYearTo – Eligible graduation-year range.
- Status – Current drive status.
- CreatedAt and UpdatedAt – Audit timestamps.

## 3. Placement Drive Status

Created:

`Models/Enums/PlacementDriveStatus.cs`

Supported values:

1. Draft
2. Upcoming
3. RegistrationOpen
4. InProgress
5. Completed
6. Cancelled

The status describes the current stage of the placement drive.

## 4. DTOs

Created placement drive DTOs under:

`DTOs/PlacementOfficers/`

Files:

- `PlacementDriveCreateDto.cs`
- `PlacementDriveUpdateDto.cs`
- `PlacementDriveResponseDto.cs`

The DTOs define the request and response contracts for placement drive operations.

The DTO namespace used by these files is:

`CampusPlacementManagement.DTOs.PlacementDrives`

## 5. Database Configuration

Updated:

`Data/ApplicationDbContext.cs`

Configured:

- PlacementDrives DbSet.
- Primary key.
- Relationships with College, Company, and PlacementOfficer.
- Restrict delete behavior.
- Foreign-key indexes.
- Required fields and maximum lengths.
- CGPA precision.
- String conversion for PlacementDriveStatus.

The placement drive database migration was applied successfully.

## 6. Repository Layer

Created:

- `Repositories/Interfaces/IPlacementDriveRepository.cs`
- `Repositories/Implementations/PlacementDriveRepository.cs`

The repository retrieves drives by ID, college, and Placement Officer and handles persistence operations.

## 7. Service Layer

Created:

- `Services/Interfaces/IPlacementDriveService.cs`
- `Services/Implementations/PlacementDriveService.cs`

The service validates:

- Placement Officer profile and account approval.
- College approval.
- Company existence and approval.
- Drive date consistency.
- Registration deadline.
- Minimum CGPA range.
- Maximum active backlogs.
- Graduation-year range.
- Ownership of the drive during updates.

The service also provides open-drive retrieval with relevant availability and organization approval checks.

## 8. Controller and Authorization

Created:

`Controllers/PlacementDrivesController.cs`

The controller provides operations for:

- Placement Officer drive creation and management.
- SuperAdmin drive listing.
- Placement Officer college-specific drive retrieval.
- Public retrieval of open placement drives.
- Authorized retrieval of individual drives.

Placement Officer operations are restricted to the officer's own college and drives.

## 9. Dependency Injection

Registered the PlacementDrive repository and service in `Program.cs`.

## 10. Build and Database Verification

Completed:

- PlacementDrive entity.
- PlacementDriveStatus enum.
- DTOs.
- EF Core configuration.
- Repository interface and implementation.
- Service interface and implementation.
- Controller.
- Dependency injection.
- Database migration and update.

**Build result:** Succeeded.

**Migration result:** Applied successfully.

**API/Postman testing:** Deferred until the complete project implementation is finished.

## 11. Key Learning Points

- How to associate a recruitment drive with a college, company, and Placement Officer.
- How to validate date ranges and registration deadlines.
- How to represent a business workflow with enum statuses.
- How to implement college ownership checks.
- How to expose public read-only information while protecting management operations.
- How to apply EF Core relationship configuration.

---

# Unit 8 – Interview Scheduling

## 1. Overview

This unit introduces interview scheduling for job applications.

Company HR representatives can schedule interviews for applicants associated with their company's recruitment process.

Students can view their own interview schedules. SuperAdmin can view interview records across the platform.

## 2. InterviewSchedule Entity

Created:

`Models/InterviewSchedule.cs`

The interview schedule connects an interview with the relevant job application, company, and Company HR representative.

Important properties include:

- InterviewScheduleId – Primary key.
- JobApplicationId – Related job application.
- CompanyId – Recruiting company.
- CompanyHRId – HR representative responsible for the interview.
- ScheduledAt – Interview date and time.
- DurationMinutes – Planned interview duration.
- Mode – Interview method.
- MeetingDetails – Meeting link or location details.
- Notes – Additional interview notes.
- Status – Interview status.
- Feedback – Interview feedback.
- CreatedAt and UpdatedAt – Audit timestamps.

## 3. Interview Enums

The interview module uses:

- `InterviewMode` – Represents the interview method.
- `InterviewStatus` – Represents the interview workflow status.

The status supports scheduling, rescheduling, completion, and other configured interview outcomes.

## 4. DTOs

Created interview request and response DTOs for:

- Interview creation.
- Interview updates.
- Interview response data.

DTOs prevent direct exposure of database entities through the API.

## 5. Database Configuration

Updated:

`Data/ApplicationDbContext.cs`

Configured:

- InterviewSchedules DbSet.
- Relationship with JobApplication.
- Relationship with Company.
- Relationship with CompanyHR.
- Restrict delete behavior.
- Foreign-key indexes.
- ScheduledAt index.
- Maximum lengths and required constraints.
- String conversion for interview enums.

Created and applied the `AddInterviewSchedule` migration.

## 6. Repository Layer

Created:

- `Repositories/Interfaces/IInterviewRepository.cs`
- `Repositories/Implementations/InterviewRepository.cs`

The repository retrieves interview schedules by company HR, student, and interview ID, and supports persistence operations.

## 7. Service Layer

Created:

- `Services/Interfaces/IInterviewService.cs`
- `Services/Implementations/InterviewService.cs`

The service coordinates interview scheduling and retrieval operations.

It uses the related job application, company, and Company HR information to associate interviews with the correct recruitment process.

Ownership checks ensure that Company HR representatives manage interviews belonging to their company and students access their own interview information.

## 8. Controller and Authorization

Created:

`Controllers/InterviewsController.cs`

The controller provides these operations:

- Company HR creates interviews.
- SuperAdmin views all interviews.
- Company HR views interviews for their company.
- Students view their own interviews.
- Authorized users retrieve individual interview details.
- Company HR updates interview information.

## 9. Dependency Injection

Registered the Interview repository and service in `Program.cs`.

## 10. Build and Database Verification

Completed:

- InterviewSchedule entity.
- Interview enums.
- Interview DTOs.
- EF Core configuration.
- Repository interface and implementation.
- Service interface and implementation.
- Controller.
- Dependency injection.
- `AddInterviewSchedule` migration and database update.

**Build result:** Succeeded.

**Migration result:** Applied successfully.

**API/Postman testing:** Deferred until the complete project implementation is finished.

## 11. Key Learning Points

- How to model interview scheduling as a separate entity.
- How to connect interviews to job applications.
- How to track interview modes and statuses.
- How to retrieve data based on the authenticated user's organization or profile.
- How to apply role-based authorization to interview operations.

---

# Unit 9 – Placement Reports

## 1. Overview

This unit introduces reporting functionality for the Campus Placement Management System.

Reports are generated from existing database records. No separate report entity or database migration is required.

The reporting module provides information about:

- Overall placement statistics.
- College-wise placement performance.
- Company-wise recruitment activity.
- Job-wise application statistics.

## 2. Report DTOs

Created DTOs under:

`DTOs/Reports/`

Files:

- `PlacementSummaryDto.cs`
- `CollegePlacementReportDto.cs`
- `CompanyRecruitmentReportDto.cs`
- `JobApplicationReportDto.cs`

These DTOs define the report response structure.

### PlacementSummaryDto

Contains overall counts and statistics, including:

- Total colleges.
- Total companies.
- Total students.
- Active students.
- Total jobs.
- Open jobs.
- Total applications.
- Application counts by status.
- Total placement drives.
- Total interviews.
- Scheduled interviews.
- Completed interviews.
- Placed students.
- Placement percentage.

### CollegePlacementReportDto

Contains:

- College ID and name.
- Total students.
- Active students.
- Students who applied.
- Total applications.
- Shortlisted applications.
- Selected applications.
- Rejected applications.
- Total placement drives.
- Placed students.
- Placement percentage.

### CompanyRecruitmentReportDto

Contains:

- Company ID and name.
- Total jobs.
- Open jobs.
- Total applications.
- Submitted applications.
- Under-review applications.
- Shortlisted applications.
- Rejected applications.
- Selected applications.
- Total interviews.
- Completed interviews.

### JobApplicationReportDto

Contains:

- Job ID and title.
- Company ID and name.
- Total applications.
- Submitted applications.
- Under-review applications.
- Shortlisted applications.
- Rejected applications.
- Selected applications.
- Withdrawn applications.

## 3. Repository Layer

Created:

- `Repositories/Interfaces/IPlacementReportRepository.cs`
- `Repositories/Implementations/PlacementReportRepository.cs`

The repository uses Entity Framework Core queries and aggregate calculations to generate report data.

The reports are calculated from existing entities such as:

- Colleges.
- Companies.
- Students.
- Jobs.
- JobApplications.
- PlacementDrives.
- InterviewSchedules.

The repository does not create separate report records.

## 4. Service Layer

Created:

- `Services/Interfaces/IPlacementReportService.cs`
- `Services/Implementations/PlacementReportService.cs`

The service provides report operations and validates the associated Placement Officer or Company HR account and organization status before returning organization-specific reports.

## 5. Controller and Authorization

Created:

`Controllers/PlacementReportsController.cs`

Implemented endpoints:

| HTTP method | Endpoint | Authorization |
|---|---|---|
| GET | `/api/placementreports/summary` | SuperAdmin |
| GET | `/api/placementreports/colleges` | SuperAdmin |
| GET | `/api/placementreports/colleges/{collegeId}` | SuperAdmin |
| GET | `/api/placementreports/my-college` | PlacementOfficer |
| GET | `/api/placementreports/companies` | SuperAdmin |
| GET | `/api/placementreports/companies/{companyId}` | SuperAdmin |
| GET | `/api/placementreports/my-company` | CompanyHR |
| GET | `/api/placementreports/jobs` | SuperAdmin |

## 6. Report Calculation

The placement reporting logic uses the `Selected` application status to identify placed students.

Distinct Student IDs are counted so that a student selected for more than one job is not counted multiple times in the placed-student count.

Placement percentage is calculated as:

`Placement Percentage = (Placed Students / Active Students) × 100`

If there are no active students, the percentage is returned as zero.

## 7. Dependency Injection

Registered the PlacementReport repository and service in `Program.cs`.

## 8. Database and Build Verification

Completed:

- Report DTOs.
- Repository interface and implementation.
- Service interface and implementation.
- Controller.
- Dependency injection.
- Report aggregation queries.

No separate report table or migration was required.

**Build result:** Succeeded.

**API/Postman testing:** Deferred until the complete project implementation is finished.

## 9. Key Learning Points

- How to create aggregate reports using LINQ and EF Core.
- How to calculate counts using `CountAsync()`.
- How to count distinct students using `Distinct()`.
- How to calculate percentages using decimal arithmetic.
- How to separate report response models from database entities.
- How to enforce role-based access to sensitive reports.
- How to generate reports from existing data without creating duplicate report tables.

---

# Overall Implementation Progress

| Unit | Module | Implementation | Build |
|---|---|---|---|
| 1 | Authentication and College Approval | Completed | Succeeded |
| 2 | College and Placement Officer Management | Completed | Succeeded |
| 3 | Company and Company HR Management | Completed | Succeeded |
| 4 | Student Management | Completed | Succeeded |
| 5 | Job Management | Completed | Succeeded |
| 6 | Job Application Management | Completed | Succeeded |
| 7 | Placement Drive Management | Completed | Succeeded |
| 8 | Interview Scheduling | Completed | Succeeded |
| 9 | Placement Reports | Completed | Succeeded |

## Overall Project Status

**Implementation:** All nine planned units have been implemented.

**Build verification:** Successful after completing Unit 9.

**Database migrations:** Applied for the units that required schema changes.

**Postman testing:** Pending.

**Documentation:** Updated through Unit 9.

**Next phase:** Comprehensive API testing, authorization testing, validation testing, and final project review.

