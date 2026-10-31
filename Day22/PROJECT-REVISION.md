# Movie Ticket Booking System — Future Revision Handbook

This is the personal revision document for revisiting the project after a break, preparing for interviews, or refreshing C#, ASP.NET Core, EF Core, SQL Server and REST API concepts.

**How to use:** Add notes as each topic is taught and implemented. Keep examples aligned with the actual code in the repository. If implementation decisions change, record the new decision and date rather than silently rewriting history.

---

## 1. Project in one minute

The Movie Ticket Booking System is a .NET 10 ASP.NET Core Web API backed by SQL Server and EF Core 10. It manages movies, theatres, screens, physical seats, shows, per-show seat availability, bookings and cancellations.

Request flow:

```text
HTTP Request
    ↓
Controller
    ↓
Service (business rules)
    ↓
Repository (data access)
    ↓
EF Core DbContext
    ↓
SQL Server
```

The response returns through the same layers to the client.

## 2. Core modules

| Module | Main responsibility |
|---|---|
| Movies | Maintain movie catalogue |
| Theatres | Maintain theatre locations |
| Screens | Maintain screens inside theatres |
| Seats | Maintain physical seats inside screens |
| Shows | Schedule a movie on a screen |
| ShowSeats | Track each seat's status for a particular show |
| Bookings | Store customer and booking-level details |
| BookingSeats | Store seats and captured prices for a booking |

## 3. Database revision map

### Eight tables

1. `Movies`
2. `Theatres`
3. `Screens`
4. `Seats`
5. `Shows`
6. `ShowSeats`
7. `Bookings`
8. `BookingSeats`

### Relationships to remember

- Theatre 1 → many Screens.
- Screen 1 → many Seats.
- Movie 1 → many Shows.
- Screen 1 → many Shows over time.
- Show 1 → many ShowSeats.
- Seat 1 → many ShowSeats over different shows.
- Show 1 → many Bookings.
- Booking 1 → many BookingSeats.
- Seat 1 → many BookingSeats over different shows/bookings.

### Three seat concepts — do not confuse them

- **Seat:** the physical seat in a screen, such as A1.
- **ShowSeat:** the availability of that physical seat for one specific show.
- **BookingSeat:** the record that the seat was included in a particular booking, including its captured price.

A physical seat can be available for one show and booked for another.

### Constraints worth revising

- Unique `(ScreenId, RowName, SeatNumber)` for physical seat coordinates.
- Unique `(ShowId, SeatId)` for one availability row per seat per show.
- Unique `BookingReference`.
- `EndTime` must be later than `StartTime`.
- Ticket prices and booking amounts must not be negative.
- Restrictive delete behavior protects dependent show/booking history.

## 4. Architecture concepts

| Concept | Meaning | Project responsibility |
|---|---|---|
| Controller | HTTP boundary | Routes, request handling, HTTP results |
| Service | Business logic | Validation, workflow coordination, calculations |
| Repository | Data access abstraction | Queries and persistence operations |
| DbContext | EF Core unit of work | Entity sets, tracking, mappings and saving |
| Entity | Persisted domain object | Database data and relationships |
| DTO | Data transfer object | Request/response contract |

**Remember:** Do not put booking rules in controllers. Do not return EF entities directly as public API responses by default. Do not put HTTP status-code decisions in repositories.

## 5. EF Core and database topics

Fill in the examples from the completed project.

### DbContext
- What is `DbContext`?
- What does each `DbSet<TEntity>` represent?
- What does `SaveChangesAsync()` do?
- What is change tracking?

**Project example:** Add the actual `ApplicationDbContext` code and explain it here.

### Fluent API
Revise:
- `HasKey`
- `Property`
- `IsRequired`
- `HasMaxLength`
- `HasPrecision`
- `HasOne`
- `WithMany`
- `HasForeignKey`
- `OnDelete`
- `HasIndex`
- `IsUnique`

**Project example:** Paste a real relationship configuration from the project and explain each method.

### Migrations
Revise:
- What is a migration?
- Why use `dotnet ef migrations add ...`?
- What does `dotnet ef database update` do?
- What is the model snapshot?
- Why inspect generated migration code?
- How do you add a later schema change safely?

Commands:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet ef migrations list
```

### Delete behavior
Explain the selected delete behavior for each relationship. In particular, why should movies/screens/seats with dependent booking history not be deleted casually?

### Transactions
A transaction groups related database changes so they commit together or roll back together. Booking creation and cancellation must not leave partially updated booking and seat-status data.

## 6. Booking logic revision

### Create booking

```text
Receive customer details + ShowId + SeatIds
        ↓
Validate show exists
        ↓
Validate SeatIds are distinct
        ↓
Find matching ShowSeat rows for this show
        ↓
Check every requested seat is Available
        ↓
Calculate price on the server
        ↓
Create Booking and BookingSeats
        ↓
Mark ShowSeats as Booked
        ↓
Commit transaction
        ↓
Return booking confirmation
```

### Cancel booking

```text
Find booking
    ↓
Check booking exists and is Confirmed
    ↓
Begin transaction
    ↓
Mark booking Cancelled
    ↓
Set associated ShowSeats to Available
    ↓
Commit transaction
```

Preserve booking history; do not delete the booking records during cancellation.

**Version 1 limitation:** A basic availability check and transaction do not guarantee protection from two simultaneous requests trying to book the same seat. Concurrency protection and temporary seat holds are later-version topics.

## 7. REST API and HTTP revision

| Status | Meaning | Example in this project |
|---|---|---|
| 200 | OK | Successful GET or update with response |
| 201 | Created | Movie or booking created |
| 204 | No Content | Successful delete |
| 400 | Bad Request | Invalid input |
| 404 | Not Found | Movie/show/booking ID does not exist |
| 409 | Conflict | Seat unavailable or deletion blocked by dependencies |
| 500 | Internal Server Error | Unexpected server failure |

### API request/response notes

Add real tested examples here for:
- Create movie.
- Create theatre/screen/seats.
- Create show.
- Retrieve show seat layout.
- Create booking.
- Cancel booking.

Record the HTTP method, URL, JSON body, status code and response.

## 8. Validation checklist

- Movie duration greater than zero.
- Seat number greater than zero.
- Show end time later than start time.
- Ticket price and booking seat price non-negative.
- Booking seat IDs distinct.
- Requested seats belong to the selected show/screen.
- Every requested ShowSeat exists and is Available.
- Customer name, email and phone are required.
- Cancellation only for a Confirmed booking.
- Screen has no overlapping show schedule.

## 9. Common commands

```bash
# Build
dotnet build

# Run
dotnet run

# Add migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# List migrations
dotnet ef migrations list

# Git status and history
git status
git log --oneline
git diff

# Stage, commit and push
git add .
git commit -m "feat: describe the completed feature"
git push
```

Use the exact project folder and configured API URL when running commands or testing endpoints.

## 10. Interview practice questions

Add your own answer below each question after the topic is taught.

1. What is ASP.NET Core Web API?
2. What is the difference between a controller and a service?
3. Why use the repository pattern?
4. What is dependency injection?
5. What is `DbContext` in EF Core?
6. What is the difference between an entity and a DTO?
7. What is a migration and what is the model snapshot?
8. What is a foreign key?
9. Explain one-to-many relationships in this project.
10. What does `OnDelete(DeleteBehavior.Restrict)` achieve?
11. Why use `decimal` for ticket prices?
12. Why calculate the booking total on the server?
13. Why use a database transaction for booking?
14. What is the difference between `Seat`, `ShowSeat` and `BookingSeat`?
15. Why should a cancelled booking be preserved?
16. What is the difference between HTTP 400, 404 and 409?
17. Why use async methods for database calls?
18. Why should controllers remain thin?
19. What problem can occur if two customers book the same seat simultaneously?
20. What improvements belong in later versions rather than Version 1?

## 11. Errors and fixes log

Record real errors encountered during implementation.

| Date | Phase | Error/message | Root cause | Fix | What I learned |
|---|---|---|---|---|---|
| | | | | | |
| | | | | | |
| | | | | | |

## 12. Phase-by-phase revision tracker

| Phase | Topics to revise | Completed? | Last revised |
|---|---|---|---|
| 1. Setup | .NET project, packages, configuration, connection string | No | — |
| 2. Entities | Classes, navigation properties, relationships, Fluent API | No | — |
| 3. Database | Migrations, snapshot, SQL tables, constraints | No | — |
| 4. Movies | DTOs, CRUD, LINQ, DI, controller/service/repository | No | — |
| 5. Theatre/Screen/Seat | Foreign keys, validation, bulk insert, delete rules | No | — |
| 6. Shows | Date/time validation, overlap query, ShowSeat generation | No | — |
| 7. Booking | Transactions, price calculation, status changes, cancellation | No | — |
| 8. Errors | Custom exceptions, IExceptionHandler, ProblemDetails, logging | No | — |
| 9. Testing | End-to-end API testing, edge cases, Git release | No | — |

## 13. Future-version reminders

- V2: concurrency protection, temporary seat holds and expiry.
- V3: ASP.NET Core Identity, JWT, Customer/Admin roles and authorization.
- V4: payment records, payment status, idempotency and gateway integration.
- V5: Angular frontend and seat map.
- V6: deployment, CI/CD, monitoring, rate limiting and production hardening.

## 14. Personal revision notes

Use this space for short explanations in your own words, diagrams, questions to revisit and topics you find difficult.

### Topics I understand well
- 

### Topics I need to practise again
- 

### Questions to ask during the next session
- 

### Last revision date
- 
