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

## Phase 2 – Movie Entity Revision

### 1. What is an entity?

An entity is a C# class that EF Core maps to a database table.

Example: The `Movie` class represents the `Movies` table.

### 2. What is a property?

A property represents data belonging to an object.

Example:
`public string Title { get; set; }`

Here, `Title` stores the movie title.

### 3. What does get; set; mean?

- `get` retrieves the property value.
- `set` assigns or changes the property value.

### 4. What does string? mean?

It indicates that a string property can contain null.

Example:
`public string? Description { get; set; }`

A movie description is optional.

### 5. What does string.Empty mean?

It initializes a string property with an empty string.

Example:
`public string Title { get; set; } = string.Empty;`

### 6. How does EF Core identify a primary key?

EF Core identifies properties named `Id` or `<EntityName>Id` by convention.

For the Movie entity:

`public int MovieId { get; set; }`

EF Core recognizes `MovieId` as the primary key.

### 7. Why do we use DateTime?

`DateTime` represents a date and time value.

In our Movie entity, `ReleaseDate` stores the movie's release date.

### Interview questions

**Q1. What is an entity in EF Core?**

An entity is a C# class that EF Core maps to a database table.

**Q2. What is the difference between a class and an object?**

A class is a blueprint. An object is an instance created from that class.

**Q3. What is the difference between string and string?**

`string` is a non-nullable reference type under nullable reference type checking, while `string?` explicitly allows null.

**Q4. Why do we use primary keys?**

A primary key uniquely identifies each record in a database table.

**Q5. Does EF Core require us to explicitly configure MovieId as the primary key?**

No. EF Core recognizes `MovieId` by convention.

### Practical revision

Try answering these without looking at the notes:

1. What is an entity?
2. What is a property?
3. What is the purpose of `get; set;`?
4. Why is Description nullable?
5. Why does EF Core recognize MovieId as the primary key?
6. What is the purpose of `string.Empty`?

## Theatre Entity – Revision Notes

### 1. What is the Theatre entity?

The Theatre entity is a C# class representing a theatre in our movie ticket booking system.

### 2. What is TheatreId?

`TheatreId` uniquely identifies each theatre.

EF Core recognizes `TheatreId` as the primary key by convention.

### 3. What is the purpose of Name?

It stores the theatre's name.

Example: PVR Cinemas.

### 4. What is the purpose of Location?

It stores the theatre's location or address.

Example: Thiruvananthapuram.

### 5. Why will Theatre be related to Screen?

A theatre can contain multiple screens, but each screen belongs to one theatre.

This is called a one-to-many relationship.

### Interview questions

**Q1. What is a primary key?**

A primary key uniquely identifies each record in a database table.

**Q2. Can one theatre have multiple screens?**

Yes. One theatre can have multiple screens.

**Q3. What is a one-to-many relationship?**

It means one record in a parent table can be associated with multiple records in a child table.

**Q4. Which property will connect Screen to Theatre?**

`TheatreId` will act as the foreign key in the Screen entity.

## Screen Entity – Revision Notes

### 1. What is the Screen entity?

The Screen entity represents an individual cinema screen inside a theatre.

### 2. What is ScreenId?

`ScreenId` uniquely identifies each screen.

It is recognized as the primary key by EF Core convention.

### 3. What is TheatreId in Screen?

`TheatreId` is a foreign key that connects a screen to its theatre.

### 4. What is a foreign key?

A foreign key is a column that references a primary key in another table and establishes a relationship between the tables.

### 5. What is a one-to-many relationship?

One theatre can have multiple screens, but each screen belongs to one theatre.

Example:

- Theatre 1 → Screen 1
- Theatre 1 → Screen 2
- Theatre 1 → Screen 3

### Interview questions

**Q1. What is a foreign key?**

A foreign key connects one table to another by referencing the primary key of the related table.

**Q2. What is the difference between a primary key and a foreign key?**

A primary key uniquely identifies a record in its own table. A foreign key establishes a relationship with another table.

**Q3. Can one theatre have multiple screens?**

Yes. This is a one-to-many relationship.

**Q4. Why do we need TheatreId in the Screen entity?**

It identifies which theatre owns the screen.

## Seat Entity – Revision Notes

### 1. What is the Seat entity?

The Seat entity represents a physical seat inside a cinema screen.

### 2. What is ScreenId?

`ScreenId` is a foreign key that connects a seat to its screen.

### 3. What is RowName?

`RowName` identifies the row in which a seat is located.

Example: A, B, C.

### 4. What is SeatNumber?

`SeatNumber` identifies the seat's number within a row.

Example: 1, 2, 3.

### 5. What is SeatType?

`SeatType` identifies the category of a seat.

Examples: Regular, Premium.

### 6. What is a composite unique constraint?

A composite unique constraint ensures that a combination of multiple column values cannot be duplicated.

In our project, the combination is:

`ScreenId + RowName + SeatNumber`

Example:

ScreenId = 1, RowName = A, SeatNumber = 1

This combination cannot be inserted twice.

### Interview questions

**Q1. What is the purpose of ScreenId in the Seat entity?**

It identifies the screen to which the physical seat belongs.

**Q2. Can two screens have a seat named A1?**

Yes. Each screen has its own seating arrangement.

**Q3. Why do we need a composite unique constraint?**

To prevent duplicate seat positions within the same screen.

**Q4. What is the difference between SeatId and SeatNumber?**

SeatId uniquely identifies a database record. SeatNumber identifies the seat's position within its row.

## Show Entity – Revision Notes

### 1. What is the Show entity?

The Show entity represents a movie screening scheduled at a particular screen on a specific date and time.

### 2. What is MovieId?

`MovieId` is a foreign key connecting a show to a movie.

### 3. What is ScreenId?

`ScreenId` is a foreign key connecting a show to a cinema screen.

### 4. What is DateTime?

`DateTime` is a C# data type that stores date and time information.

Example:

`new DateTime(2026, 11, 5, 18, 0, 0)`

This represents November 5, 2026, at 6:00 PM.

### 5. Why do we use decimal for TicketPrice?

`decimal` is suitable for monetary calculations because it provides decimal-based precision.

### 6. What business rules apply to a show?

- EndTime must be later than StartTime.
- TicketPrice must not be negative.
- Shows scheduled on the same screen must not overlap.

### Interview questions

**Q1. What is a foreign key in the Show entity?**

MovieId and ScreenId are foreign keys.

**Q2. Why do we use DateTime?**

To store the date and time of a movie screening.

**Q3. Why is decimal used for TicketPrice?**

Because ticket prices represent monetary values.

**Q4. Can two shows run at the same time on the same screen?**

No. The application must validate screen scheduling to prevent overlapping shows.

**Q5. Where should show validation logic be implemented?**

Business validation should be implemented in the Service layer.