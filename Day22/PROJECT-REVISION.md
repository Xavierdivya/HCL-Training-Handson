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

## ShowSeat Entity – Revision Notes

### 1. What is ShowSeat?

ShowSeat represents the availability of a physical seat for a particular movie show.

### 2. Why do we need ShowSeat separately from Seat?

A physical seat can be booked for one show and remain available for another show.

Therefore, availability must be maintained separately for each show.

### 3. What is ShowId?

ShowId is a foreign key connecting the availability record to a particular movie show.

### 4. What is SeatId?

SeatId is a foreign key connecting the availability record to a physical seat.

### 5. What is Status?

Status stores the current availability of the seat.

Our Version 1 supports:

- Available
- Booked

The default status is Available.

### 6. What is a composite unique constraint?

It prevents duplicate combinations of column values.

In our project:

`ShowId + SeatId`

The same physical seat can appear in different shows, but it must appear only once within a particular show.

### Interview questions

**Q1. Why do we need the ShowSeat entity?**

To track seat availability separately for each movie show.

**Q2. Can the same SeatId appear in multiple ShowSeat records?**

Yes. A physical seat can belong to multiple shows.

**Q3. Can the same ShowId and SeatId combination appear twice?**

No. That combination must be unique.

**Q4. What is the default status of a new ShowSeat?**

Available.

**Q5. What is the difference between Seat and ShowSeat?**

Seat stores physical seat information. ShowSeat stores the availability of that seat for a particular show.

## Booking Entity – Revision Notes

**1. What is the purpose of the Booking entity?**

It stores customer details and booking information for a movie show.

**2. Why do we use BookingReference?**

It provides a unique reference that customers can use to identify their bookings.

**3. Why is TotalAmount a decimal?**

Because ticket prices and booking totals can contain decimal values.

**4. What is the purpose of ShowId?**

It identifies which movie show the customer booked.

**5. What are the booking statuses?**

- Confirmed
- Cancelled

**6. Why should cancelled bookings not be deleted?**

Keeping cancelled bookings preserves booking history.

**Interview revision:** Explain how a Booking connects a customer to a particular Show and how its status changes during cancellation.

## BookingSeat Entity – Revision Notes

**1. What is the purpose of BookingSeat?**

It connects a booking to the individual seats selected by the customer.

**2. Why do we need a separate BookingSeat table?**

One booking can contain multiple seats. Each selected seat is stored as a separate record.

**3. What is the relationship between Booking and BookingSeat?**

One Booking can have many BookingSeat records.

**4. Why is SeatId not globally unique in BookingSeat?**

The same physical seat can be booked for different shows at different times.

**5. Why do we store Price in BookingSeat?**

It records the price charged for that particular seat.

**Interview revision:** Explain how Booking and BookingSeat work together when a customer books multiple seats.

## Movie Navigation Property – Revision Notes

**1. What is a navigation property?**

A navigation property allows us to access related entities through an entity object.

**2. What does ICollection<Show> mean?**

It represents a collection of multiple Show objects.

**3. What is the relationship between Movie and Show?**

One Movie can have multiple Shows. Each Show belongs to a Movie.

**4. Why do we initialize the collection using new List<Show>()?**

To ensure the collection is initialized and ready to store related Show objects.

**5. Does a navigation property create a new database table?**

No. It helps EF Core understand the relationship between entities.

**Interview revision:** Explain the difference between a foreign key property and a navigation property.

## Theatre Navigation Property – Revision Notes

**1. What is the relationship between Theatre and Screen?**

One Theatre can have multiple Screens.

**2. What is ICollection<Screen>?**

It represents a collection of related Screen objects.

**3. What is the foreign key in this relationship?**

`TheatreId` in the Screen entity.

**4. What is the navigation property?**

`Screens` in the Theatre entity.

**5. Why initialize Screens with new List<Screen>()?**

To ensure the collection is initialized and ready to store related screens.

**Interview revision:** Explain the difference between TheatreId and Screens in the Theatre–Screen relationship.

## Screen Navigation Properties – Revision Notes

**1. What is the relationship between Theatre and Screen?**

One Theatre can have multiple Screens.

**2. What is the relationship between Screen and Seat?**

One Screen can contain multiple Seats.

**3. What is Theatre in Screen.cs?**

It is a reference navigation property that represents the related Theatre.

**4. What is Seats in Screen.cs?**

It is a collection navigation property representing multiple related Seat objects.

**5. What does null! mean?**

It suppresses the nullable-reference warning. It does not create or load an object.

**6. What is the foreign key connecting Screen and Theatre?**

`TheatreId`.

**Interview revision:** Explain reference navigation properties versus collection navigation properties.

## Seat Navigation Properties – Revision Notes

**1. What is the relationship between Screen and Seat?**

One Screen can contain multiple Seats.

**2. What is ShowSeats?**

It represents the availability records of a physical seat across different shows.

**3. What is BookingSeats?**

It represents the booking records involving a physical seat.

**4. Why can one Seat have multiple ShowSeat records?**

Because the same physical seat can be associated with different shows.

**5. Why can one Seat have multiple BookingSeat records?**

Because the same physical seat can be booked for different shows at different times.

**6. What is ScreenId?**

It is the foreign key connecting Seat to Screen.

**Interview revision:** Explain why Seat is connected to both ShowSeat and BookingSeat instead of storing booking availability directly inside Seat.

## Show Navigation Properties – Revision Notes

**1. What is the relationship between Movie and Show?**

One Movie can have multiple Shows.

**2. What is the relationship between Screen and Show?**

One Screen can host multiple Shows at different times.

**3. What is ShowSeats?**

It represents the seat availability records associated with a particular show.

**4. What is Bookings?**

It represents all bookings made for a particular show.

**5. What are the foreign keys in Show?**

`MovieId` and `ScreenId`.

**6. What is the difference between reference and collection navigation properties?**

- Reference: represents one related entity.
- Collection: represents multiple related entities.

**Interview revision:** Explain how Movie, Screen, ShowSeat and Booking are connected through Show.

## ShowSeat Navigation Properties – Revision Notes

**1. What is ShowSeat?**

ShowSeat connects a particular movie show with a physical seat.

**2. Why do we need ShowSeat?**

The same physical seat can have different availability statuses for different shows.

**3. What are the foreign keys in ShowSeat?**

- `ShowId`
- `SeatId`

**4. What does Status represent?**

It indicates whether the seat is Available or Booked for a particular show.

**5. Why do we need a unique constraint on ShowId and SeatId?**

To prevent duplicate availability records for the same seat in the same show.

**6. What are the navigation properties?**

- `Show`
- `Seat`

**Interview revision:** Explain why seat availability belongs to ShowSeat rather than directly to Seat.

## Booking Navigation Properties – Revision Notes

**1. What is the relationship between Show and Booking?**

One Show can have multiple Bookings.

**2. What is the relationship between Booking and BookingSeat?**

One Booking can contain multiple BookingSeat records.

**3. What is Show in Booking.cs?**

It is a reference navigation property representing the related Show.

**4. What is BookingSeats?**

It is a collection navigation property representing the seats included in a booking.

**5. What is the foreign key in Booking?**

`ShowId`.

**6. Why use a separate BookingSeat entity?**

To store each selected seat separately while allowing one booking to contain multiple seats.

**Interview revision:** Explain how Booking, Show and BookingSeat work together when a customer books multiple seats.

## BookingSeat Navigation Properties – Revision Notes

**1. What is BookingSeat?**

It connects a booking with an individual physical seat.

**2. What is the relationship between Booking and BookingSeat?**

One Booking can contain multiple BookingSeat records.

**3. What is the relationship between Seat and BookingSeat?**

One Seat can appear in multiple BookingSeat records across different bookings.

**4. What are the foreign keys?**

- BookingId
- SeatId

**5. Why should SeatId not be globally unique?**

The same physical seat can be booked for different shows at different times.

**6. What are the navigation properties?**

- Booking
- Seat

**Interview revision:** Explain how BookingSeat connects the Booking and Seat entities.

## ApplicationDbContext – Revision Notes

**1. What is DbContext?**

DbContext is the main class used by Entity Framework Core to communicate with the database.

**2. What is DbSet?**

DbSet represents a collection of entities that EF Core maps to a database table.

**3. Why do we use DbContextOptions?**

It provides configuration information to the DbContext, such as the database provider and connection settings.

**4. What does base(options) mean?**

It passes the received configuration options to the parent DbContext class.

**5. Does creating DbSet properties automatically create database tables?**

No. We need to create and apply EF Core migrations to create the tables in the database.


## SQL Server DbContext Registration – Revision Notes

**1. What is Dependency Injection?**

Dependency Injection is a technique where ASP.NET Core provides required objects to classes instead of requiring them to create those objects manually.

**2. What does AddDbContext() do?**

It registers the ApplicationDbContext in the application's Dependency Injection container.

**3. What does UseSqlServer() do?**

It configures Entity Framework Core to communicate with SQL Server.

**4. What does GetConnectionString() do?**

It retrieves a named connection string from appsettings.json.

**5. Does AddDbContext() create the database?**

No. It configures the connection. We will create the database using EF Core migrations.


## OnModelCreating – Revision Notes

**1. What is Fluent API?**

Fluent API is a way to configure entity properties, relationships, and database constraints using C# code.

**2. What is OnModelCreating()?**

It is an EF Core method used to configure the database model.

**3. What is ModelBuilder?**

ModelBuilder provides methods to configure entities and their database mappings.

**4. Why do we use base.OnModelCreating(modelBuilder)?**

It calls the base DbContext implementation.

**5. What is the difference between Data Annotations and Fluent API?**

Data Annotations use attributes on model properties. Fluent API uses configuration code, usually inside OnModelCreating().


## Operation 20 – EF Core Fluent API Revision

### 1. What is Fluent API?

Fluent API is used to configure entity properties, database tables, relationships, indexes, and constraints inside the `OnModelCreating()` method of `ApplicationDbContext`.

### 2. Important Methods

| Method | Purpose |
|---|---|
| `HasKey()` | Defines the primary key |
| `IsRequired()` | Makes a property mandatory |
| `HasMaxLength()` | Sets maximum string length |
| `HasPrecision(10, 2)` | Sets decimal precision and scale |
| `HasOne()` | Defines the one side of a relationship |
| `WithMany()` | Defines the many side of a relationship |
| `HasForeignKey()` | Defines the foreign key |
| `OnDelete()` | Configures delete behavior |
| `HasIndex()` | Creates a database index |
| `IsUnique()` | Prevents duplicate index values |
| `HasCheckConstraint()` | Adds a database validation condition |
| `ToTable()` | Configures the database table name and table constraints |

### 3. Relationship Example

```csharp
entity.HasOne(s => s.Theatre)
      .WithMany(t => t.Screens)
      .HasForeignKey(s => s.TheatreId)
      .OnDelete(DeleteBehavior.Restrict);
```

Explanation:

- `HasOne()` – Each screen belongs to one theatre.
- `WithMany()` – One theatre can contain multiple screens.
- `HasForeignKey()` – `TheatreId` is the foreign key in the Screen table.
- `Restrict` – Prevents deleting a theatre while dependent screens exist.

### 4. Unique Index Example

```csharp
entity.HasIndex(s => new { s.TheatreId, s.ScreenName })
      .IsUnique();
```

This prevents two screens in the same theatre from having the same screen name.

### 5. Check Constraint Example

```csharp
entity.ToTable("Movies", table =>
    table.HasCheckConstraint(
        "CK_Movies_DurationMinutes",
        "[DurationMinutes] > 0"));
```

This ensures that movie duration must be greater than zero.

### 6. Important Points to Remember

- Primary keys uniquely identify records.
- Foreign keys connect related tables.
- Unique indexes prevent duplicate combinations.
- Check constraints validate data at the database level.
- `DeleteBehavior.Restrict` helps preserve dependent records and history.
- Integer primary keys are generated by convention in EF Core.
- `OnModelCreating()` centralizes database model configuration.

### Build Verification

`dotnet build` – **Succeeded**.

## Operation 21 – EF Core Migration Revision

### 1. What is a Migration?

A migration is a set of instructions generated by EF Core to create or modify the database schema based on changes in entity models and DbContext configuration.

### 2. Command to Create a Migration

```powershell
dotnet ef migrations add InitialCreate
```

- `dotnet ef` – Runs EF Core command-line tools.
- `migrations add` – Creates a new migration.
- `InitialCreate` – Name of the migration.

### 3. Migration Files

| File | Purpose |
|---|---|
| `InitialCreate.cs` | Contains database schema creation instructions |
| `InitialCreate.Designer.cs` | Contains migration metadata |
| `ApplicationDbContextModelSnapshot.cs` | Stores the current model snapshot |

### 4. Migration vs Database Update

**Create migration:**

```powershell
dotnet ef migrations add InitialCreate
```

Generates migration files.

**Apply migration:**

```powershell
dotnet ef database update
```

Applies the migration to SQL Server and creates or updates the database schema.

### 5. Important Points

- Entity classes define the data structure.
- Fluent API configures the database model.
- Migration records schema changes.
- Model Snapshot helps EF Core compare model changes.
- Database Update applies migrations to the database.

### 6. Build Verification

`dotnet build` – **Succeeded.**

`dotnet ef migrations add InitialCreate` – **Succeeded.**

## Operation 22 – Apply Migration Revision

### 1. What is Database Update?

Database Update applies the migration files generated by EF Core to the actual SQL Server database.

### 2. Command

```powershell
dotnet ef database update
```

### 3. What Does It Do?

- Connects to SQL Server.
- Creates the database if it does not exist.
- Applies pending migrations.
- Creates or updates tables and relationships.
- Records migration history.

### 4. What is __EFMigrationsHistory?

It is a system table maintained by EF Core.

It records which migrations have already been applied to the database.

This prevents EF Core from applying the same migration repeatedly.

### 5. Important Difference

**Migration generation:**

```powershell
dotnet ef migrations add InitialCreate
```

Creates migration files.

**Migration application:**

```powershell
dotnet ef database update
```

Applies the migration to SQL Server.

### 6. Database Details

- Database: MovieTicketBookingDB
- SQL Server instance: localhost\SQLEXPRESS
- Migration: InitialCreate
- Status: Successfully applied

### 7. Verification

`dotnet ef database update` – **Succeeded.**

## Operation 23 – Database Verification Revision

### 1. How to View All Tables?

```sql
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';
```

### 2. How to View Primary Keys?

```sql
SELECT *
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE CONSTRAINT_NAME LIKE 'PK_%';
```

### 3. How to View Foreign Keys?

SQL Server system views:

- `sys.foreign_keys`
- `sys.foreign_key_columns`

These help identify relationships between child and parent tables.

### 4. How to View Unique Indexes?

```sql
SELECT
    t.name AS TableName,
    i.name AS IndexName,
    i.is_unique AS IsUnique
FROM sys.indexes i
INNER JOIN sys.tables t
    ON i.object_id = t.object_id
WHERE i.is_unique = 1;
```

### 5. Important Concepts

- Primary Key: Uniquely identifies a database record.
- Foreign Key: Connects related tables.
- Unique Index: Prevents duplicate values or combinations.
- Child Table: Contains the foreign key.
- Parent Table: Contains the referenced primary or unique key.
- Migration History: Tracks migrations applied by EF Core.

### 6. Verification

Database table verification – **Succeeded.**

Primary key verification – **Succeeded.**

Foreign key verification – **Succeeded.**

Unique index verification – **Succeeded.**

## Operation 24 – Repository Layer Revision

### 1. What is the Repository Pattern?

The Repository Pattern separates database access logic from business logic.

It provides methods to perform database operations through repository classes.

### 2. Application Flow

Controller → Service → Repository → DbContext → Database

### 3. Interface vs Implementation

**Interface:**

Defines which operations are available.

Example: `IMovieRepository`

**Implementation:**

Contains the actual EF Core database queries.

Example: `MovieRepository`

### 4. Dependency Injection Through Constructor

```csharp
private readonly ApplicationDbContext _context;

public MovieRepository(ApplicationDbContext context)
{
    _context = context;
}
```

- `ApplicationDbContext` provides access to database tables.
- `_context` stores the injected DbContext instance.
- Constructor injection supplies the dependency.

### 5. Important EF Core Methods

| Method | Explanation |
|---|---|
| `AddAsync()` | Adds a record |
| `AddRangeAsync()` | Adds multiple records |
| `SaveChangesAsync()` | Saves changes to the database |
| `ToListAsync()` | Returns a list |
| `FirstOrDefaultAsync()` | Returns the first match or null |
| `FindAsync()` | Searches by primary key |
| `Update()` | Updates an entity |
| `Remove()` | Deletes an entity |
| `AsNoTracking()` | Disables tracking for read-only queries |
| `Include()` | Loads related data |
| `ThenInclude()` | Loads nested related data |

### 6. Pagination

Example:

```csharp
.Skip((pageNumber - 1) * pageSize)
.Take(pageSize)
.ToListAsync();
```

Explanation:

- `Skip()` ignores records from previous pages.
- `Take()` retrieves the requested number of records.

### 7. Filtering

Example:

```csharp
.Where(m => m.Title.Contains(title))
```

Retrieves movies whose titles contain the supplied search text.

### 8. Include and ThenInclude

Example:

```csharp
.Include(b => b.BookingSeats)
    .ThenInclude(bs => bs.Seat)
```

- `Include()` loads the booking-seat collection.
- `ThenInclude()` loads each related seat.

### 9. Asynchronous Programming

- `async` allows a method to execute asynchronously.
- `await` waits for an asynchronous operation to finish without blocking the current thread.
- `Task<T>` represents an asynchronous operation that returns a value of type `T`.
- `Task` represents an asynchronous operation without a return value.

### 10. Important Concepts

- Repository Interface: Defines available operations.
- Repository Implementation: Contains database query logic.
- DbContext: Manages database access and entity tracking.
- LINQ: Used to query and filter data.
- Pagination: Retrieves data in smaller pages.
- Navigation Property: Represents relationships between entities.
- Transaction: Groups database operations so they succeed or fail together.

### 11. Build Verification

`dotnet build` – **Succeeded.**


## Operation 25 – Dependency Injection Revision

### What is Dependency Injection?

Dependency Injection is a design pattern where an object receives the dependencies it needs instead of creating them itself.

### What is AddScoped?

```csharp
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
```

- `IMovieRepository`: Interface.
- `MovieRepository`: Implementation class.
- `AddScoped`: Creates one instance per HTTP request.

### Three DI service lifetimes

| Lifetime | Meaning |
|---|---|
| Transient | A new instance every time requested. |
| Scoped | One instance per HTTP request. |
| Singleton | One instance for the application's lifetime. |

### Why use Dependency Injection?

- Reduces tight coupling.
- Makes code easier to maintain.
- Makes unit testing easier.
- Separates object creation from business logic.

### Important flow

Controller → Service → Repository → ApplicationDbContext → SQL Server

### Important code

```csharp
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
```

**Remember:** Registering repositories makes them available through DI. It does not automatically create the service layer or API endpoints.

## Operation 26 – Service Layer Revision

### What is the Service Layer?

The service layer contains business logic and validation. It acts as a bridge between controllers and repositories.

### Why do we need services?

- Keeps controllers simple.
- Separates business rules from database operations.
- Improves maintainability.
- Makes unit testing easier.

### Service and Repository difference

| Service Layer | Repository Layer |
|---|---|
| Handles business logic | Handles database operations |
| Validates input | Executes queries |
| Checks related entity existence | Saves and retrieves records |
| Coordinates workflows | Communicates with DbContext |

### Constructor Dependency Injection

```csharp
private readonly IMovieRepository _movieRepository;

public MovieService(IMovieRepository movieRepository)
{
    _movieRepository = movieRepository;
}
```

The repository is supplied through the constructor rather than created manually.

### Asynchronous programming

```csharp
public async Task<Movie> AddAsync(Movie movie)
{
    ValidateMovie(movie);
    return await _movieRepository.AddAsync(movie);
}
```

- `async`: allows asynchronous execution.
- `Task<Movie>`: represents an asynchronous operation returning a Movie.
- `await`: waits for the asynchronous repository operation without blocking the thread.

### Validation example

```csharp
if (movie.DurationMinutes <= 0)
{
    throw new ArgumentException(
        "Movie duration must be greater than zero.");
}
```

### Important exception types

- `ArgumentException`: invalid input.
- `ArgumentNullException`: null object.
- `KeyNotFoundException`: requested record does not exist.

### Architecture to remember

Controller → Service → Repository → DbContext → Database

**Important:** Services handle business rules, while repositories handle data access.


# Controller Layer – Revision Notes

## 1. What is a Controller?

A Controller handles incoming HTTP requests and returns HTTP responses to the client.

It connects the API layer with the Service layer.

## 2. What is `[ApiController]`?

It identifies a class as an API controller and enables API-specific behaviors, including automatic validation error responses.

## 3. What is `[Route]`?

It defines the URL pattern used to access controller endpoints.

Example:

```csharp
[Route("api/[controller]")]
```

For `MoviesController`, the base route is:

`/api/Movies`

## 4. HTTP Methods

| HTTP Method | Purpose |
|---|---|
| GET | Retrieve data |
| POST | Create data |
| PUT | Update data |
| DELETE | Delete data |

## 5. Important HTTP Status Codes

| Status Code | Meaning |
|---|---|
| 200 | OK – Request successful |
| 201 | Created – Resource created |
| 204 | No Content – Successful operation without response body |
| 400 | Bad Request – Invalid request |
| 404 | Not Found – Resource does not exist |
| 500 | Internal Server Error |

## 6. Dependency Injection in Controllers

Example:

```csharp
public MoviesController(IMovieService movieService)
{
    _movieService = movieService;
}
```

The service is injected through the constructor.

This avoids directly creating service objects inside the controller.

## 7. What is IActionResult?

`IActionResult` represents the result returned by a controller action.

Example:

```csharp
return Ok(movie);
```

```csharp
return NotFound();
```

```csharp
return NoContent();
```

## 8. What is CreatedAtAction()?

It returns HTTP 201 Created and provides a route to retrieve the newly created resource.

Example:

```csharp
return CreatedAtAction(
    nameof(GetById),
    new { id = result.MovieId },
    result);
```

## 9. Application Architecture

Remember this execution flow:

**Controller → Service → Repository → DbContext → SQL Server**

- Controller handles HTTP requests.
- Service handles business logic and validation.
- Repository handles database operations.
- DbContext communicates with SQL Server.

## 10. Day22 Progress

Completed:
- Eight entity models.
- Entity Framework Core configuration.
- Initial migration.
- SQL Server database creation.
- Six repository interfaces and implementations.
- Repository Dependency Injection.
- Six service interfaces and implementations.
- Service Dependency Injection.
- Six API controllers.

Next: Improve request DTOs and error handling, then test API endpoints using Scalar.


# Operation 30 – DTO Revision Notes

## 1. DTO Meaning

DTO = Data Transfer Object.

Used to transfer data between API clients and application layers without directly exposing database entities.

## 2. Request DTO vs Response DTO

| Request DTO | Response DTO |
|---|---|
| Receives client data | Returns API data |
| Used for POST and PUT | Used for GET and successful POST |
| Example: CreateMovieDto | Example: MovieResponseDto |

## 3. Data Annotation Attributes

| Attribute | Purpose |
|---|---|
| [Required] | Field must have a value |
| [StringLength] | Limits string length |
| [Range] | Restricts numeric or comparable values |
| [EmailAddress] | Validates email format |
| [Phone] | Validates phone format |
| [MinLength] | Enforces minimum collection or string length |

## 4. Mapping

DTO to Entity:

```csharp
var movie = new Movie
{
    Title = dto.Title,
    DurationMinutes = dto.DurationMinutes
};
```

Entity to DTO:

```csharp
var response = new MovieResponseDto
{
    MovieId = movie.MovieId,
    Title = movie.Title,
    DurationMinutes = movie.DurationMinutes
};
```

## 5. LINQ Select

```csharp
var response = movies.Select(MapToResponse).ToList();
```

Converts each Movie entity into a MovieResponseDto.

## 6. HTTP Responses

| Method | Status | Meaning |
|---|---|---|
| POST | 201 Created | Resource created |
| GET | 200 OK | Resource retrieved |
| PUT | 204 No Content | Update completed |
| DELETE | 204 No Content | Delete completed |
| GET/PUT/DELETE | 404 Not Found | Resource not found |

## 7. Important ASP.NET Core Concepts

- [ApiController]: Enables API-specific behavior, including automatic validation responses for invalid model state.
- [Route]: Defines the controller route.
- CreatedAtAction(): Returns a Created response with a location for the new resource.
- IActionResult: Allows controller actions to return different HTTP responses.
- MapToResponse(): Reusable method for mapping entities to response DTOs.

## 8. Booking Retrieval

```csharp
.Include(b => b.BookingSeats)
    .ThenInclude(bs => bs.Seat)
```

Include loads the booking-seat collection.

ThenInclude loads the related physical Seat entity.

AsNoTracking() is used for read-only queries.

## 9. Architecture

Client → DTO → Controller → Service → Repository → Database

Database → Repository → Service → Controller → DTO → Client

## 10. Verification

```powershell
dotnet build
```

Result: Build succeeded.

## 11. Operation 30 Checklist

- [x] Created Movie DTOs
- [x] Created Theatre DTOs
- [x] Created Screen DTOs
- [x] Created Seat DTOs
- [x] Created Show DTOs
- [x] Created Booking DTOs
- [x] Integrated DTOs into six controllers
- [x] Verified booking retrieval includes BookingSeats
- [x] Build succeeded

Next: Operation 31 – Authentication, Authorization and JWT.

## Operation 31 – Authentication, Identity and JWT Revision

**Authentication:** Verifies who the user is.

**Authorization:** Determines what an authenticated user is allowed to do.

**ASP.NET Core Identity:** Manages users, passwords and roles.

**ApplicationUser:** Custom Identity user model containing FullName.

**IdentityDbContext:** Provides database support for Identity entities.

**JWT (JSON Web Token):** A signed token issued after successful authentication.

**JWT Claims:** Information about the user, such as User ID, email and role.

**UserManager:** Manages user creation, password verification and role assignment.

**RoleManager:** Manages application roles.

**JwtBearer:** Validates JWT tokens sent with API requests.

**User Secrets:** Stores development secrets outside the project configuration files.

**IDesignTimeDbContextFactory:** Provides EF Core with a DbContext during design-time commands such as migrations.

### Important commands

```powershell
dotnet user-secrets set "Jwt:Key" "YOUR_SECRET_KEY"
dotnet ef migrations add AddIdentityAuthentication
dotnet ef database update
dotnet ef migrations list
dotnet build
```

**Remember:** Never commit JWT signing keys or passwords to GitHub.


# Day 22 – Movie Ticket Booking API
# Quick Revision Notes

## 1. What is ASP.NET Core Web API?

ASP.NET Core Web API is used to build HTTP-based services that allow clients such as Postman, web applications and mobile applications to communicate with a backend.

Common HTTP methods:

| Method | Purpose |
|---|---|
| GET | Retrieve data |
| POST | Create data |
| PUT | Update data |
| DELETE | Delete data |

## 2. What is DTO?

DTO means Data Transfer Object.

It transfers data between the client and API without exposing the complete database entity.

Example:

```csharp
public class CreateBookingDto
{
    public int ShowId { get; set; }
    public List<int> SeatIds { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
}
```

Benefits:
- Controls which data the client can submit.
- Prevents unnecessary entity properties from being exposed.
- Supports input validation.
- Separates API contracts from database models.

## 3. Data Annotation Attributes

| Attribute | Purpose |
|---|---|
| `[Required]` | Value is mandatory |
| `[Range]` | Restricts numeric values |
| `[StringLength]` | Restricts string length |
| `[EmailAddress]` | Validates email format |
| `[Phone]` | Validates phone format |
| `[MinLength]` | Specifies minimum collection or string length |

## 4. What is Dependency Injection?

Dependency Injection provides required objects to a class instead of creating those objects manually.

Example:

```csharp
builder.Services.AddScoped<IBookingService, BookingService>();
```

`IBookingService` is the interface.

`BookingService` is its implementation.

`AddScoped` creates one service instance per HTTP request scope.

Other DI lifetimes:

- AddTransient: New instance whenever requested.
- AddScoped: One instance per request scope.
- AddSingleton: One instance for the application lifetime.

## 5. Repository and Service Layers

### Repository

Responsible for database operations.

Examples:
- Add booking.
- Find booking.
- Update booking.
- Retrieve booking details.

### Service

Responsible for business logic.

Examples:
- Validate show.
- Check seat availability.
- Calculate booking amount.
- Generate booking reference.
- Cancel booking.

### Controller

Receives HTTP requests and returns HTTP responses.

Flow:

```text
Postman
   |
   v
Controller
   |
   v
Service
   |
   v
Repository
   |
   v
ApplicationDbContext
   |
   v
SQL Server
```

## 6. Entity Framework Core

EF Core is an Object-Relational Mapper (ORM).

It allows C# applications to interact with relational databases using entity classes and LINQ.

Important concepts:

- DbContext: Manages database interactions.
- DbSet: Represents a database table.
- Migration: Records database schema changes.
- Database Update: Applies migrations to the database.
- Fluent API: Configures entities and relationships.

### Migration Commands

Create migration:

```powershell
dotnet ef migrations add MigrationName
```

Apply migration:

```powershell
dotnet ef database update
```

Remove the latest unapplied migration:

```powershell
dotnet ef migrations remove
```

## 7. Database Relationships

Examples in the project:

- One Theatre has many Screens.
- One Screen has many Seats.
- One Movie has many Shows.
- One Screen has many Shows.
- One Show has many Bookings.
- One Booking has many BookingSeats.

### DeleteBehavior.Restrict

Prevents deleting a parent record while dependent child records exist.

Example:

```csharp
.OnDelete(DeleteBehavior.Restrict);
```

This protects related records from accidental deletion.

## 8. Transactions

A database transaction groups multiple operations into one logical unit.

In booking creation:

1. Create booking.
2. Create booking-seat records.
3. Mark seats as booked.
4. Save changes.

If an operation fails, the transaction can roll back so partial booking data is not committed.

## 9. JWT Authentication

JWT means JSON Web Token.

It is used to authenticate API requests.

Authentication flow:

```text
User Login
    |
    v
Validate Credentials
    |
    v
Generate JWT
    |
    v
Client Stores Token
    |
    v
Send Bearer Token
    |
    v
API Validates Token
    |
    v
Access Granted / Denied
```

Postman Authorization header:

```text
Authorization: Bearer <access_token>
```

Never commit JWT signing keys, passwords or live access tokens to GitHub.

## 10. HTTP Status Codes

| Status | Meaning |
|---|---|
| 200 | OK |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 500 | Internal Server Error |

## 11. Booking API Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/Bookings` | Create booking |
| GET | `/api/Bookings/{id}` | Get booking by ID |
| GET | `/api/Bookings/reference/{bookingReference}` | Get booking by reference |
| PUT | `/api/Bookings/{id}/cancel` | Cancel booking |

## 12. Booking Business Logic

### Create Booking

- Validate show ID.
- Validate seat IDs.
- Check seat availability.
- Prevent duplicate seat selection.
- Calculate total amount.
- Generate unique booking reference.
- Save booking and booking seats.
- Mark seats as booked.

### Cancel Booking

- Find booking.
- Change status to Cancelled.
- Release associated seats.
- Save changes.

## 13. Common Errors and Solutions

### Error 1: Unable to resolve service

Cause: Service implementation was not registered in Program.cs.

Solution:

```csharp
builder.Services.AddScoped<IBookingService, BookingService>();
```

### Error 2: String or binary data would be truncated

Cause: Application generated a longer booking reference than the database column allowed.

Solution:

```csharp
entity.Property(b => b.BookingReference)
      .HasMaxLength(50);
```

Then create and apply a migration.

### Error 3: AppHost.exe Access Denied

Workaround:

```powershell
dotnet build -p:UseAppHost=false
dotnet run -p:UseAppHost=false
```

### Error 4: ECONNREFUSED in Postman

Cause: API server is not running or the request uses the wrong URL or port.

Solution:
- Start the API.
- Verify the listening URL.
- Check the Postman baseUrl variable.

## 14. Important Interview Questions

**Q1. What is DTO?**

A DTO transfers data between the client and application without exposing the database entity directly.

**Q2. What is Dependency Injection?**

Dependency Injection provides the required dependencies to a class rather than making the class create them.

**Q3. What is the Repository Pattern?**

It separates database access logic from business logic.

**Q4. What is a migration?**

A migration records changes to the database schema.

**Q5. What is JWT?**

JWT is a token format used to securely transmit claims and authenticate API requests.

**Q6. What is the difference between Authentication and Authorization?**

Authentication verifies who the user is. Authorization determines what the user is allowed to access.

**Q7. What is a transaction?**

A transaction ensures that a group of database operations succeeds together or rolls back on failure.

**Q8. Why use DeleteBehavior.Restrict?**

It prevents deleting a parent record when dependent child records exist.

**Q9. Why use AddScoped?**

It creates one service instance per request scope, which is suitable for many services that depend on EF Core DbContext.

**Q10. Why use a BookingReference?**

It provides a unique reference that customers can use to identify and retrieve their bookings.

## 15. Revision Checklist

- [ ] Understand ASP.NET Core Web API.
- [ ] Understand DTOs and validation.
- [ ] Understand Controllers, Services and Repositories.
- [ ] Understand Dependency Injection.
- [ ] Understand EF Core migrations.
- [ ] Understand parent-child relationships.
- [ ] Understand transactions.
- [ ] Understand JWT authentication.
- [ ] Test booking creation.
- [ ] Test booking retrieval by ID.
- [ ] Test booking retrieval by reference.
- [ ] Test booking cancellation.
- [ ] Verify seat availability after cancellation.
- [ ] Understand Git feature branches and Pull Requests.