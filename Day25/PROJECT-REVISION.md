# Day25 – Campus Placement Management
# Project Revision Notes

## 1. Project Overview

Campus Placement Management is a .NET 10 ASP.NET Core Web API application designed to manage college placement activities.

### Technology Stack

- C#
- ASP.NET Core Web API
- .NET 10
- Entity Framework Core 10
- SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication
- Postman
- Git and GitHub

---

## 2. Main User Roles

The application has four authentication roles:

| Role | Responsibility |
|---|---|
| SuperAdmin | Manages and approves colleges, officers and companies |
| PlacementOfficer | Manages students belonging to an associated college |
| CompanyHR | Manages company recruitment and job postings |
| Student | Views eligible jobs and manages own applications |

College and Company are organizational entities, not authentication roles.

---

## 3. Account Status

Defined in:

`Models/Enums/AccountStatus.cs`

| Status | Meaning |
|---|---|
| Pending | Waiting for approval |
| Approved | Approved account |
| Rejected | Registration rejected |
| Suspended | Temporarily blocked |
| Inactive | Account inactive |

Default status:

`Pending`

---

## 4. College Status

Defined in:

`Models/Enums/CollegeStatus.cs`

| Status | Meaning |
|---|---|
| Pending | Awaiting SuperAdmin approval |
| Approved | College approved |
| Rejected | College registration rejected |
| Suspended | College temporarily suspended |

A newly registered college must have Pending status.

---

## 5. Important Project Files

| File | Responsibility |
|---|---|
| ApplicationUser.cs | Extends IdentityUser |
| AccountStatus.cs | Defines user account statuses |
| College.cs | College entity |
| CollegeStatus.cs | College approval statuses |
| ApplicationDbContext.cs | EF Core database context |
| AppRoles.cs | Centralized role names |
| IdentitySeeder.cs | Seeds roles and SuperAdmin |
| CollegeRegistrationDto.cs | College registration request |
| CollegeResponseDto.cs | College response |
| CollegeStatusUpdateDto.cs | Status update request |
| ICollegeRepository.cs | Repository contract |
| CollegeRepository.cs | Database operations |
| ICollegeService.cs | Service contract |
| CollegeService.cs | College business logic |
| CollegesController.cs | College API endpoints |

---

## 6. Layered Architecture

Request flow:

Client → Controller → Service → Repository → DbContext → SQL Server

### Controller

Handles HTTP requests and responses.

### Service

Contains business rules and validations.

### Repository

Contains database access operations.

### DbContext

Provides communication between EF Core and SQL Server.

---

## 7. Important EF Core Concepts

### DbSet

Represents a collection of entities in the database.

Example:

```csharp
public DbSet<College> Colleges { get; set; }
```

### AsNoTracking()

Used for read-only queries.

It avoids unnecessary EF Core change tracking.

### FirstOrDefaultAsync()

Returns the first matching record or null.

### ToListAsync()

Retrieves multiple records asynchronously.

### AddAsync()

Adds an entity to the DbContext.

### SaveChangesAsync()

Persists changes to the database.

### HasIndex().IsUnique()

Creates a unique database index.

Example:

```csharp
entity.HasIndex(c => c.CollegeCode)
    .IsUnique();
```

This prevents duplicate CollegeCode values at the database level.

### HasConversion<string>()

Stores enum values as strings instead of their underlying integer values.

Example:

```csharp
entity.Property(c => c.Status)
    .HasConversion<string>();
```

---

## 8. Important ASP.NET Core Concepts

### [ApiController]

Provides automatic request model validation and HTTP 400 responses for invalid models.

### [AllowAnonymous]

Allows an endpoint to be accessed without authentication.

### [Authorize]

Restricts endpoint access to authenticated users.

### [Authorize(Roles = ...)]

Restricts access to users belonging to the specified role.

### [HttpPost]

Maps an action to an HTTP POST request.

### [HttpGet]

Maps an action to an HTTP GET request.

### [Route("api/[controller]")]

Defines the controller's route.

For CollegesController:

`api/colleges`

### CreatedAtAction()

Returns HTTP 201 Created and includes a location pointing to the created resource.

---

## 9. DTO Concepts

DTO means Data Transfer Object.

DTOs define what information a client can send or receive.

### Why use DTOs?

- Prevent direct exposure of database entities.
- Control which properties clients can submit.
- Add validation rules.
- Separate API contracts from database models.

### Validation Attributes

| Attribute | Purpose |
|---|---|
| [Required] | Field must contain a value |
| [StringLength] | Restricts string length |
| [EmailAddress] | Validates email format |
| [Phone] | Validates phone format |
| [Url] | Validates URL format |

---

## 10. Dependency Injection

Dependency injection provides required objects to classes instead of creating dependencies manually.

Example:

```csharp
builder.Services.AddScoped<
    ICollegeRepository,
    CollegeRepository>();

builder.Services.AddScoped<
    ICollegeService,
    CollegeService>();
```

### Service Lifetimes

| Lifetime | Meaning |
|---|---|
| Transient | New instance whenever requested |
| Scoped | One instance per request scope |
| Singleton | One instance for the application lifetime |

Scoped is commonly used for EF Core repositories and application services.

---

## 11. ASP.NET Core Identity

ApplicationUser extends:

`IdentityUser`

ApplicationDbContext extends:

`IdentityDbContext<ApplicationUser>`

Important Identity classes:

- UserManager<ApplicationUser>
- RoleManager<IdentityRole>
- SignInManager<ApplicationUser>

### IdentitySeeder

Responsible for creating roles and the initial SuperAdmin account.

SuperAdmin credentials are stored in User Secrets.

Configuration keys:

- SuperAdmin:Email
- SuperAdmin:Password
- SuperAdmin:FullName

Never hard-code production administrator passwords in source code.

---

## 12. College Registration API

Base route:

`/api/colleges`

### Implemented Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| POST | /api/colleges/register | Register a college |
| GET | /api/colleges | Retrieve all colleges |
| GET | /api/colleges/{id} | Retrieve college by ID |

Registration is public.

The two GET endpoints require the SuperAdmin role.

Protected endpoint testing will follow JWT authentication implementation.

### Registration Business Logic

1. Receive CollegeRegistrationDto.
2. Normalize CollegeCode to uppercase.
3. Check whether CollegeCode exists.
4. Reject duplicates.
5. Create College entity.
6. Set status to Pending.
7. Save using repository.
8. Return CollegeResponseDto.

---

## 13. Important HTTP Status Codes

| Status | Meaning | Usage in this project |
|---|---|---|
| 200 OK | Request successful | Successful GET |
| 201 Created | Resource created | College registration |
| 400 Bad Request | Invalid request | DTO validation failure |
| 401 Unauthorized | Authentication missing or invalid | Protected API without valid authentication |
| 403 Forbidden | Authenticated but insufficient permission | Wrong role or authorization failure |
| 404 Not Found | Resource not found | College ID does not exist |
| 409 Conflict | Resource conflicts with existing data | Duplicate CollegeCode |
| 500 Internal Server Error | Unexpected server failure | Unhandled application error |

### Important Difference: 401 vs 403

401 means the request does not have valid authentication credentials.

403 means the user is authenticated but does not have permission to access the resource.

---

## 14. Postman Testing Results

| Test case | Expected status | Result |
|---|---|---|
| Register valid college | 201 | Passed |
| Verify SQL Server record | Record exists | Passed |
| Register duplicate CollegeCode | 409 | Passed |
| Submit invalid registration data | 400 | Passed |

### Sample Registration Request

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

### SQL Server Verification

```sql
USE CampusPlacementDB;
GO

SELECT *
FROM Colleges;
```

---

## 15. Important Interview Questions

### Q1. What is Entity Framework Core?

Entity Framework Core is an Object Relational Mapper (ORM) that allows developers to interact with relational databases using C# objects instead of writing SQL for every operation.

### Q2. What is DbContext?

DbContext represents a session with the database and manages entity querying, tracking, inserting, updating and deleting.

### Q3. What is the Repository Pattern?

The Repository Pattern separates database access logic from business logic by providing an abstraction over data operations.

### Q4. Why do we use DTOs?

DTOs control the data transferred between the client and server and prevent direct exposure of database entities.

### Q5. What is dependency injection?

Dependency injection is a design pattern in which a class receives its dependencies from an external container rather than creating them itself.

### Q6. Why use asynchronous database operations?

Async operations help avoid blocking request-processing threads while waiting for database operations to complete.

### Q7. What is AsNoTracking()?

It disables change tracking for a query and is useful when retrieved entities will only be read.

### Q8. What is a unique index?

A unique index ensures that a database column or combination of columns cannot contain duplicate values.

### Q9. What is the purpose of [ApiController]?

It enables API-specific behavior such as automatic model validation and automatic HTTP 400 responses for invalid request models.

### Q10. What is the difference between authentication and authorization?

Authentication verifies who the user is.

Authorization determines what the authenticated user is allowed to do.

### Q11. Why use ASP.NET Core Identity?

Identity provides user management, password hashing, role management, authentication-related services and token support.

### Q12. Why should administrator passwords not be hard-coded?

Hard-coded credentials can be exposed through source code or repository history. User Secrets and secure production configuration help protect sensitive information.

### Q13. What is a database migration?

A migration records schema changes made to the application's EF Core model and allows those changes to be applied to the database.

### Q14. Why return 409 Conflict for duplicate CollegeCode?

Because the submitted registration conflicts with an existing college record using the same unique code.

### Q15. What is the difference between 401 and 403?

401 indicates missing or invalid authentication.

403 indicates that the authenticated user does not have sufficient permission.

---

## 16. Useful Development Commands

### Build

```powershell
dotnet build
```

### Run

```powershell
dotnet run
```

### List migrations

```powershell
dotnet ef migrations list
```

### Create migration

```powershell
dotnet ef migrations add MigrationName
```

### Apply migration

```powershell
dotnet ef database update
```

### Check Git status

```powershell
git status
```

### Stage changes

```powershell
git add .
```

### Commit changes

```powershell
git commit -m "feat: implement college registration api"
```

### Push changes

```powershell
git push
```

---

## 17. Current Progress and Next Steps

### Completed

- Project foundation.
- Identity configuration.
- ApplicationUser and account statuses.
- SuperAdmin and role seeding.
- SQL Server database setup.
- College entity and migration.
- College DTOs.
- College repository.
- College service.
- College registration controller.
- Dependency injection.
- College registration API tests.

### Next

College Approval Workflow:

1. Get pending colleges.
2. Approve college.
3. Reject college.
4. Suspend college.
5. Test authorization using SuperAdmin JWT.

After completing the College module, proceed to the Placement Officer module.

---

**Remember the implementation sequence:**

Model → DTO → Repository Interface → Repository Implementation → Service Interface → Service Implementation → Controller → Program.cs → Migration → Postman Testing → Documentation → Git Commit and Push.




# Unit 1 – Authentication and College Approval Revision Notes

## 1. Important Endpoints

| Method | Endpoint | Purpose | Access |
|---|---|---|---|
| POST | `/api/auth/register/college-representative` | Register a college and representative | Anonymous |
| POST | `/api/auth/login` | Authenticate and receive JWT | Anonymous |
| GET | `/api/auth/me` | Get current user details | Authenticated |
| GET | `/api/colleges` | Get all colleges | SuperAdmin |
| GET | `/api/colleges/{id}` | Get college details | SuperAdmin |
| GET | `/api/colleges/pending` | Get pending colleges | SuperAdmin |
| PUT | `/api/colleges/{id}/approve` | Approve a college | SuperAdmin |
| PUT | `/api/colleges/{id}/reject` | Reject a college | SuperAdmin |
| PUT | `/api/colleges/{id}/suspend` | Suspend a college | SuperAdmin |

## 2. Application Roles

The application currently defines four roles:

- `SuperAdmin`
- `PlacementOfficer`
- `CompanyHR`
- `Student`

A college representative uses the `PlacementOfficer` role. College is an organizational entity, not a separate application role.

## 3. Account and College Statuses

### AccountStatus

- `Pending`
- `Approved`
- `Rejected`
- `Suspended`
- `Inactive`

### CollegeStatus

- `Pending`
- `Approved`
- `Rejected`
- `Suspended`

A representative's account status is synchronized when the SuperAdmin changes the associated college's status.

## 4. Authentication Flow

1. A college representative submits registration details.
2. The application creates a Pending college.
3. The application creates a Pending representative account.
4. The representative receives the `PlacementOfficer` role.
5. The SuperAdmin logs in and approves the college.
6. The college and its representative account become Approved.
7. The representative can log in and receive a JWT.
8. The representative sends the JWT as a Bearer token to access authenticated endpoints.

## 5. JWT Configuration

JWT settings are stored in .NET User Secrets:

- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:ExpiryMinutes`
- `Jwt:Key`

Do not commit the JWT signing key or other credentials to source control.

JWT validation should include:

- Signing key validation
- Issuer validation
- Audience validation
- Token lifetime validation

## 6. HTTP Status Codes

| Status code | Meaning | Example |
|---|---|---|
| 201 | Created | College representative registration |
| 200 | OK | Successful login, profile retrieval, or approval |
| 401 | Unauthorized | Invalid credentials or unapproved/rejected/suspended account |
| 403 | Forbidden | Authenticated representative tries to access a SuperAdmin-only endpoint |
| 409 | Conflict | Duplicate college code or email registration |

## 7. Authorization Reminder

Authentication identifies the user. Authorization determines whether the authenticated user has permission to perform an operation.

- **401:** The request does not have valid authentication.
- **403:** The user is authenticated but does not have the required permission.

Register authentication middleware before authorization middleware:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

## 8. Testing Checklist

- [x] Register a college representative.
- [x] Verify the college and representative initially have Pending status.
- [x] Log in as SuperAdmin.
- [x] Approve the college.
- [x] Verify the representative can log in after approval.
- [x] Retrieve the current user's profile.
- [x] Verify the representative cannot access SuperAdmin-only college endpoints.
- [x] Reject a separate test college.
- [x] Verify the rejected representative cannot log in.
- [x] Suspend the approved test college.
- [x] Verify the suspended representative cannot log in.

## 9. Important Testing Data

The approved test college used for the suspension test was College ID `3` (`TEST001`). It is now Suspended.

Do not use this college for later tests that require an approved college. Create a separate test college for those workflows.

## 10. Next Development Focus

Proceed with the next planned module only after committing and pushing the completed authentication unit.

Future work includes the remaining organizational, student, job, application, recruitment, interview, and reporting modules.



# Unit 2 – College and Placement Officer Management

## 1. PlacementOfficer Entity

**Location:** `Models/PlacementOfficer.cs`

Important properties:

- `PlacementOfficerId` – Primary key.
- `UserId` – Identity user foreign key.
- `CollegeId` – College foreign key.
- `Designation` – Officer designation.
- `CreatedAt`, `UpdatedAt` – Audit timestamps.

## 2. Database Relationships

**Location:** `Data/ApplicationDbContext.cs`

- One-to-one: PlacementOfficer → ApplicationUser.
- Many-to-one: PlacementOfficer → College.
- Unique index on `UserId`.
- Restrict delete behavior on both relationships.

**Migration:** `AddPlacementOfficer`

Commands:

```powershell
Add-Migration AddPlacementOfficer
Update-Database
```

## 3. Architecture

| Layer | Files |
|---|---|
| DTO | `DTOs/PlacementOfficers/PlacementOfficerResponseDto.cs` |
| Repository interface | `IPlacementOfficerRepository.cs` |
| Repository | `PlacementOfficerRepository.cs` |
| Service interface | `IPlacementOfficerService.cs` |
| Service | `PlacementOfficerService.cs` |
| Controller | `PlacementOfficersController.cs` |

## 4. API Endpoints

| Method | Endpoint | Access |
|---|---|---|
| GET | `/api/placementofficers` | SuperAdmin |
| GET | `/api/placementofficers/{id}` | SuperAdmin |
| GET | `/api/placementofficers/me` | PlacementOfficer |

The `/me` endpoint uses the JWT `NameIdentifier` claim to identify the current user.

## 5. Registration Workflow

College representative registration now creates:

1. College record with Pending status.
2. Identity user with Pending status.
3. PlacementOfficer role assignment.
4. PlacementOfficer profile linked to the user and college.

All registration operations are performed within a database transaction.

The designation is supplied through the registration DTO, with `Placement Officer` as the default.

## 6. Important Concepts

- **Identity relationship:** Connect business profiles to authenticated users.
- **Repository pattern:** Separate database access from business logic.
- **Service layer:** Map entities to DTOs and coordinate operations.
- **JWT NameIdentifier:** Retrieve the authenticated user's ID.
- **Database transaction:** Keep related registration records consistent.
- **DTO mapping:** Return only required profile information.

## 7. Verification Status

- EF Core migration: Applied.
- Database update: Succeeded.
- Project build: Succeeded.
- API/Postman testing: Deferred until project completion.

## 8. Next Unit

**Unit 3 – Company and Company HR Management**

Planned implementation:

- Company entity and status management.
- Company HR profile and Identity association.
- Company and Company HR repositories.
- Company and Company HR services.
- Controllers and authorization.
- Company HR registration and approval workflow.



## Unit 4 – Student Management

- A Student belongs to one College and one PlacementOfficer.
- Student identity accounts are created by Placement Officers.
- Register numbers must be unique within a College.
- Students can view their own profile.
- Placement Officers can manage students belonging to their College.

## Unit 5 – Job Management

- Every Job belongs to a Company.
- Only approved Company HR accounts can create jobs.
- Company HR users can access only their own company's jobs.
- SuperAdmin can view and manage all jobs.
- Students and visitors can view open jobs.
- Job eligibility includes CGPA, backlogs, departments, degrees, and graduation years.
- Job statuses: Draft, Open, Closed, Cancelled.

# Day25 – Campus Placement Management System
# Units 6–9 Revision Notes

## 1. Job Application Management

### Purpose

Job Application Management allows students to apply for jobs and enables Company HR representatives to review applications.

### Main Entity

`JobApplication`

Important properties:

- JobApplicationId
- JobId
- StudentId
- ApplicationDate
- Status
- CoverLetter
- Remarks
- CreatedAt
- UpdatedAt

### Application Status

| Status | Meaning |
|---|---|
| Submitted | Application submitted by student |
| UnderReview | Application being reviewed |
| Shortlisted | Student shortlisted |
| Rejected | Application rejected |
| Selected | Student selected |
| Withdrawn | Student withdrew application |

### Important Business Rules

1. Only approved and active students can apply.
2. The job must be open.
3. The application deadline must not have passed.
4. The company must be approved.
5. The student must satisfy job eligibility requirements.
6. A student cannot apply twice for the same job.
7. Withdrawn applications cannot be updated.
8. Company HR can manage applications belonging to its own company.
9. Placement Officers can view applications belonging to their college.

### Important EF Core Concept: Composite Unique Index

A composite unique index prevents duplicate applications for the same student and job.

```csharp
entity.HasIndex(a => new
{
    a.JobId,
    a.StudentId
})
.IsUnique();
```

This means the same combination of JobId and StudentId cannot be inserted twice.

### Important LINQ Concepts

```csharp
// Filter applications
var applications = await _context.JobApplications
    .Where(a => a.JobId == jobId)
    .ToListAsync();

// Count applications
var total = await _context.JobApplications
    .CountAsync();

// Count shortlisted applications
var shortlisted = await _context.JobApplications
    .CountAsync(a =>
        a.Status == ApplicationStatus.Shortlisted);
```

### Revision Questions

1. What is a JobApplication entity?
2. Why does JobApplication have both JobId and StudentId?
3. What is a composite unique index?
4. Why should a student not be allowed to apply twice for the same job?
5. What is the difference between Submitted and UnderReview?
6. Why should a withdrawn application not be updated?
7. Why should job eligibility be checked in the service layer?
8. How can Company HR access only its company's applications?

---

## 2. Placement Drive Management

### Purpose

Placement Drive Management allows Placement Officers to organize recruitment activities for their colleges.

A placement drive connects:

- College
- Company
- Placement Officer

### Main Entity

`PlacementDrive`

Important properties:

| Property | Purpose |
|---|---|
| PlacementDriveId | Primary key |
| CollegeId | Associated college |
| CompanyId | Recruiting company |
| PlacementOfficerId | Responsible officer |
| DriveName | Drive name |
| Description | Drive details |
| Venue | Drive location |
| StartDate | Drive starting date |
| EndDate | Drive ending date |
| RegistrationDeadline | Registration closing date |
| MinimumCGPA | Minimum academic eligibility |
| MaximumActiveBacklogs | Maximum allowed backlogs |
| EligibleDepartments | Eligible departments |
| EligibleDegrees | Eligible degree programmes |
| GraduationYearFrom | Starting graduation year |
| GraduationYearTo | Ending graduation year |
| Status | Drive status |

### Placement Drive Status

- Draft
- Upcoming
- RegistrationOpen
- InProgress
- Completed
- Cancelled

### Important Business Rules

1. Only an approved Placement Officer can create a drive.
2. The associated college must be approved.
3. The selected company must be approved.
4. Start date must be before end date.
5. Registration deadline must be consistent with the drive schedule.
6. CGPA must be within the supported range.
7. Maximum active backlogs cannot be negative.
8. Graduation-year range must be valid.
9. A Placement Officer can update only their own drives.

### Important Concepts

**Foreign key:** Connects a placement drive to another entity.

**Navigation property:** Allows EF Core to access related entity information.

**Restrict delete:** Prevents deletion of a related record when dependent records exist.

**Enum:** Represents a fixed set of allowed status values.

### Revision Questions

1. What is a placement drive?
2. Why does a placement drive require CollegeId and CompanyId?
3. What is the difference between a job and a placement drive?
4. Why is RegistrationDeadline required?
5. Why validate StartDate and EndDate?
6. What is a navigation property?
7. Why use an enum for PlacementDriveStatus?
8. How does the service ensure a Placement Officer updates only their own drive?

---

## 3. Interview Scheduling

### Purpose

Interview Scheduling manages interviews for students who have applied for jobs.

Company HR representatives schedule interviews and students can retrieve their own interview details.

### Main Entity

`InterviewSchedule`

Important properties:

- InterviewScheduleId
- JobApplicationId
- CompanyId
- CompanyHRId
- ScheduledAt
- DurationMinutes
- Mode
- MeetingDetails
- Notes
- Status
- Feedback
- CreatedAt
- UpdatedAt

### Interview Mode

The InterviewMode enum represents the configured interview methods.

Examples of interview modes include:

- Online
- Offline

Check the actual enum in the project for the complete list of supported values.

### Interview Status

The InterviewStatus enum represents the interview workflow.

The reporting implementation uses these values:

- Scheduled
- Rescheduled
- Completed

Other supported values should be checked in the project's enum definition.

### Important Relationships

```text
JobApplication
      |
      | 1-to-many
      |
InterviewSchedule

Company
   |
   | 1-to-many
   |
InterviewSchedule

CompanyHR
   |
   | 1-to-many
   |
InterviewSchedule
```

The exact cardinality depends on the configured foreign keys and navigation properties.

### Important Business Rules

1. Company HR manages interviews associated with its company.
2. Students can view their own interviews.
3. SuperAdmin can view interview records across the platform.
4. Interview records are associated with the relevant job application.
5. Interview updates must respect ownership and authorization rules.

### Revision Questions

1. What is an InterviewSchedule?
2. Why does an interview reference JobApplicationId?
3. Why store CompanyId and CompanyHRId?
4. What is the difference between InterviewMode and InterviewStatus?
5. Why should a student only access their own interview records?
6. How does role-based authorization protect interview endpoints?
7. Why is feedback stored separately from the original application?

---

## 4. Placement Reports

### Purpose

Placement Reports provide statistics about recruitment and placement activities.

Reports are calculated using existing database records instead of storing duplicate report data.

### Report DTOs

| DTO | Purpose |
|---|---|
| PlacementSummaryDto | Overall placement statistics |
| CollegePlacementReportDto | College-wise placement statistics |
| CompanyRecruitmentReportDto | Company-wise recruitment statistics |
| JobApplicationReportDto | Job-wise application statistics |

### Important Reporting Concepts

#### CountAsync()

Counts matching database records asynchronously.

```csharp
var totalStudents =
    await _context.Students.CountAsync();
```

#### Distinct()

Removes duplicate values from a query.

```csharp
var placedStudents = await _context.JobApplications
    .Where(a => a.Status == ApplicationStatus.Selected)
    .Select(a => a.StudentId)
    .Distinct()
    .CountAsync();
```

Why use `Distinct()`?

A student may be selected for more than one job. Without `Distinct()`, the same student could be counted multiple times.

#### Placement Percentage

Formula:

```text
Placement Percentage =
    (Placed Students / Active Students) * 100
```

C# implementation:

```csharp
var percentage = activeStudents == 0
    ? 0
    : Math.Round(
        (decimal)placedStudents / activeStudents * 100,
        2);
```

The zero check prevents division by zero.

### Report Access

| Report | Role |
|---|---|
| Overall summary | SuperAdmin |
| All college reports | SuperAdmin |
| Individual college report | SuperAdmin |
| Own college report | PlacementOfficer |
| All company reports | SuperAdmin |
| Individual company report | SuperAdmin |
| Own company report | CompanyHR |
| Job-wise application reports | SuperAdmin |

### Reporting Endpoints

| Method | Endpoint | Role |
|---|---|---|
| GET | `/api/placementreports/summary` | SuperAdmin |
| GET | `/api/placementreports/colleges` | SuperAdmin |
| GET | `/api/placementreports/colleges/{collegeId}` | SuperAdmin |
| GET | `/api/placementreports/my-college` | PlacementOfficer |
| GET | `/api/placementreports/companies` | SuperAdmin |
| GET | `/api/placementreports/companies/{companyId}` | SuperAdmin |
| GET | `/api/placementreports/my-company` | CompanyHR |
| GET | `/api/placementreports/jobs` | SuperAdmin |

### Revision Questions

1. What is an aggregate report?
2. Why use DTOs for reports?
3. What does CountAsync() do?
4. What does Distinct() do?
5. Why calculate placement percentage using decimal?
6. Why check whether ActiveStudents is zero?
7. Why can reports be generated without creating separate report tables?
8. How can Company HR be restricted to its own company's report?

---

# 5. Core Architecture Revision

The project uses layered architecture.

```text
                 CLIENT
                   |
                   v
              CONTROLLER
                   |
                   v
                SERVICE
                   |
                   v
              REPOSITORY
                   |
                   v
             APPLICATION DB
                CONTEXT
                   |
                   v
               SQL SERVER
```

### Controller Layer

Responsibilities:

- Accept HTTP requests.
- Read route and request parameters.
- Apply authorization attributes.
- Call the service layer.
- Return HTTP responses.

### Service Layer

Responsibilities:

- Business rules.
- Validation.
- Workflow coordination.
- Ownership checks.
- Calculations.
- Calling repository operations.

### Repository Layer

Responsibilities:

- Database queries.
- Adding entities.
- Updating entities.
- Retrieving related data.
- Saving database changes.

### DTO Layer

Responsibilities:

- Define request models.
- Define response models.
- Prevent direct exposure of database entities.
- Control which information is returned by an endpoint.

### DbContext

Responsibilities:

- Manage entity sets.
- Configure relationships.
- Track changes.
- Execute database operations.
- Persist changes using SaveChangesAsync().

---

# 6. Authentication and Authorization Revision

## Application Roles

| Role | Responsibility |
|---|---|
| SuperAdmin | Platform-wide management |
| PlacementOfficer | College placement management |
| CompanyHR | Company recruitment management |
| Student | Student job and application activities |

College and Company are organizational entities, not authentication roles.

## Authentication vs Authorization

**Authentication:** Identifies who the user is.

**Authorization:** Determines what the authenticated user is allowed to do.

### HTTP Status Codes

| Code | Meaning |
|---|---|
| 200 | OK |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 409 | Conflict |
| 500 | Internal Server Error |

Remember:

- 401 means valid authentication is missing.
- 403 means the user is authenticated but lacks permission.

### Middleware Order

```csharp
app.UseAuthentication();

app.UseAuthorization();
```

Authentication must execute before authorization.

---

# 7. Entity Framework Core Revision

## DbSet

Represents an entity collection managed by the DbContext.

```csharp
public DbSet<Student> Students { get; set; }
```

## AsNoTracking()

Used for read-only queries to avoid unnecessary change tracking.

```csharp
var students = await _context.Students
    .AsNoTracking()
    .ToListAsync();
```

## FirstOrDefaultAsync()

Returns the first matching record or null.

## ToListAsync()

Retrieves matching records asynchronously.

## CountAsync()

Counts matching records.

## Include()

Loads related entities.

```csharp
var application = await _context.JobApplications
    .Include(a => a.Student)
    .Include(a => a.Job)
    .FirstOrDefaultAsync(a =>
        a.JobApplicationId == id);
```

## HasIndex().IsUnique()

Creates a unique index to prevent duplicate values.

## HasConversion<string>()

Stores enum values as strings.

## OnDelete(DeleteBehavior.Restrict)

Prevents deleting a principal record when dependent records exist.

## Migrations

Common commands:

```powershell
# Create migration
Add-Migration MigrationName

# Apply migration
Update-Database

# List migrations
Get-Migration
```

CLI equivalents:

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
dotnet ef migrations list
```

A migration records the database schema changes associated with modifications to the EF Core model.

---

# 8. Security and Ownership Revision

Important ownership rules:

| User | Data access |
|---|---|
| SuperAdmin | Platform-wide data |
| PlacementOfficer | Own college data |
| CompanyHR | Own company data |
| Student | Own student-related data |

Important concepts:

- Role-based authorization.
- JWT Bearer authentication.
- Claims.
- User ID retrieval.
- Organization ownership.
- Account approval status.
- Company approval status.
- College approval status.

Example:

```csharp
[Authorize(Roles = AppRoles.SuperAdmin)]
```

This restricts an endpoint to authenticated users with the SuperAdmin role.

---

# 9. Build and Database Commands

Run from the project directory:

```powershell
cd D:\HCL\Day25\CampusPlacementManagement

# Build
dotnet build

# Run
dotnet run

# List packages
dotnet list package

# Check migrations
dotnet ef migrations list

# Apply pending migrations
dotnet ef database update
```

Do not create a new migration unless the EF Core model has a corresponding schema change.

---

# 10. Git Revision

Useful commands:

```powershell
git status

git branch

git log --oneline

git diff

git add Day25/

git commit -m "feat: complete placement management reports"

git push
```

Before committing:

1. Verify the correct branch.
2. Check the staged files.
3. Avoid committing secrets.
4. Avoid committing `bin/`, `obj/`, or `.vs/`.
5. Confirm the build succeeds.
6. Use a conventional commit message.

---

# 11. Interview Practice Questions

## Architecture and ASP.NET Core

1. What is ASP.NET Core Web API?
2. Why use layered architecture?
3. What is dependency injection?
4. What is the repository pattern?
5. What is the difference between a controller and a service?
6. What is a DTO?
7. What is the purpose of ApplicationDbContext?
8. What is the difference between an entity and a DTO?

## Authentication and Authorization

9. What is ASP.NET Core Identity?
10. What is JWT authentication?
11. What is the difference between authentication and authorization?
12. What is the difference between 401 and 403?
13. What are claims in JWT?
14. How does role-based authorization work?
15. Why should account approval be checked during login?

## EF Core and SQL Server

16. What is a DbSet?
17. What is a migration?
18. What is a foreign key?
19. What is a composite unique index?
20. What is the purpose of AsNoTracking()?
21. What is the difference between FirstOrDefaultAsync() and SingleOrDefaultAsync()?
22. What does Include() do?
23. What is DeleteBehavior.Restrict?
24. Why use asynchronous database methods?

## Job Applications

25. How is a JobApplication related to Student and Job?
26. How do you prevent duplicate applications?
27. How do you validate a student's job eligibility?
28. Why use an enum for application status?
29. Why should business rules be implemented in the service layer?

## Placement Drives and Interviews

30. What is the purpose of PlacementDrive?
31. How do you enforce Placement Officer ownership?
32. What is the difference between a Job and a Placement Drive?
33. How is an InterviewSchedule associated with a JobApplication?
34. How do you restrict Company HR access to its own company data?

## Reporting

35. What is an aggregate query?
36. What is the purpose of Distinct()?
37. How do you calculate placement percentage?
38. Why should placed students be counted distinctly?
39. Why can reports be generated without separate report tables?
40. How do you protect organization-specific reports?

---

# 12. Project Completion Tracker

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

## Remaining Work

- [ ] Complete Postman testing for all modules.
- [ ] Test successful and unsuccessful scenarios.
- [ ] Test role-based authorization.
- [ ] Test college and company ownership restrictions.
- [ ] Test validation and duplicate prevention.
- [ ] Verify report calculations against database records.
- [ ] Review security and configuration.
- [ ] Perform final build.
- [ ] Review Git status.
- [ ] Commit and push the completed Day25 project.

## Personal Revision Notes

### Topics I understand well

- 

### Topics I need to practise again

- 

### Questions to revisit

- 

### Last revision date

- 
