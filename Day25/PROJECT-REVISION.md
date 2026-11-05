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