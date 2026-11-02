# Movie Ticket Booking System — Project Learning & Working Plan

**Technology:** C# · .NET 10 · ASP.NET Core Web API · EF Core 10 · SQL Server  
**Architecture:** Controller → Service → Repository → DbContext  
**Version:** 1.0 learning backend  
**Frontend:** Angular (later version)

---

## 1. Project goal

Build a backend REST API that manages movies, theatres, screens, physical seats, show schedules, per-show seat availability, bookings and cancellations.

The project is a learning implementation. Version 1 does not include authentication, authorization, real payment processing, temporary seat holds or advanced concurrency protection. Do not deploy the unauthenticated Version 1 API as a public production service.

## 2. How we will work

For every phase, follow this cycle:

1. Understand the requirement and the business reason.
2. Create only the folders and files needed for that phase.
3. Implement one small feature at a time.
4. Build the project and fix compilation errors.
5. Run the API and test the endpoint using Scalar/Swagger or Postman.
6. Test a successful case and at least one invalid case.
7. Explain the code and important concepts.
8. Update this document and `PROJECT-REVISION.md`.
9. Review `git status`, create a conventional commit and push the branch.
10. Move to the next phase only after the current phase works.

**Git rule:** Commit working milestones. Do not commit broken or untested code as a completed feature.

## 3. Suggested repository structure

Start with one ASP.NET Core Web API project and use folder boundaries. Extract separate projects only if there is a clear learning or maintenance reason.

```text
MovieTicketBooking/
├── Controllers/
├── Models/
├── DTOs/
│   ├── Movies/
│   ├── Theatres/
│   ├── Screens/
│   ├── Seats/
│   ├── Shows/
│   └── Bookings/
├── Data/
│   └── ApplicationDbContext.cs
├── Repositories/
│   ├── Interfaces/
│   └── implementations
├── Services/
│   ├── Interfaces/
│   └── implementations
├── Migrations/
├── Middleware/                 # introduced in the exception-handling phase
├── docs/
│   ├── PROJECT-LEARNING.md
│   └── PROJECT-REVISION.md
├── Program.cs
├── appsettings.json
└── MovieTicketBooking.csproj
```

The agreed Version 1 design uses a single API project. Show-seat endpoints are exposed through `ShowsController`; a separate `ShowSeatsController` is not required. Do not add a generic repository or Unit of Work abstraction for Version 1.

## 4. Implementation roadmap

### Phase 1 — Solution setup and database planning

**Tasks**
- Create the .NET 10 ASP.NET Core Web API project.
- Confirm the project runs locally.
- Install/configure EF Core SQL Server packages.
- Configure the SQL Server connection string safely.
- Create the planned folders.
- Review the eight-table database design and relationships.

**Deliverable**
- Runnable empty API and agreed folder/database plan.

**Verification**
- `dotnet build` succeeds.
- `dotnet run` starts the API.
- Open the actual OpenAPI/Scalar route configured by the project.

**Suggested commit**
```bash
git add .
git commit -m "chore: initialize movie ticket booking api"
```

### Phase 2 — Entities and EF Core configuration

Create these eight entities:
1. `Movie`
2. `Theatre`
3. `Screen`
4. `Seat`
5. `Show`
6. `ShowSeat`
7. `Booking`
8. `BookingSeat`

**Tasks**
- Add entity properties and navigation properties.
- Add all eight `DbSet` properties to `ApplicationDbContext`.
- Configure primary keys, foreign keys, required fields, maximum lengths and decimal precision.
- Configure unique constraints/indexes.
- Configure restrictive delete behavior where dependent booking/show history must be preserved.
- Review one-to-many relationships.

**Important rules**
- A physical `Seat` belongs to a `Screen`.
- `ShowSeat` represents one physical seat's availability for one particular show.
- `BookingSeat` records a seat included in a booking.
- Unique `(ShowId, SeatId)` is required for `ShowSeat`.
- Unique `(ScreenId, RowName, SeatNumber)` is required for physical seat coordinates.
- Do not make `BookingSeat.SeatId` globally unique.

**Deliverable**
- All entity and relationship mappings compile.

**Verification**
- `dotnet build` succeeds.
- Review the model configuration before creating the migration.

**Suggested commit**
```bash
git add .
git commit -m "feat: add movie booking entities and relationships"
```

### Phase 3 — Migration and database creation

**Tasks**
- Create the initial EF Core migration.
- Inspect the generated migration and model snapshot.
- Apply the migration to SQL Server.
- Verify the database and all eight tables.
- Verify foreign keys, unique indexes and column types.

**Commands**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet ef migrations list
```

**Deliverable**
- `MovieTicketBookingDB` exists with the intended schema.

**Suggested commit**
```bash
git add .
git commit -m "feat: create initial movie booking database migration"
```

### Phase 4 — Movie APIs

**Tasks**
- Create movie request/response DTOs.
- Create `IMovieRepository` and `MovieRepository`.
- Create `IMovieService` and `MovieService`.
- Register dependencies in `Program.cs`.
- Create `MoviesController`.
- Implement create, list, get-by-ID, update, delete and title search.
- Add input and business validation.

**Endpoints**
- `POST /api/movies`
- `GET /api/movies`
- `GET /api/movies/{id}`
- `PUT /api/movies/{id}`
- `DELETE /api/movies/{id}`
- `GET /api/movies/search?title=leo`

**Deliverable**
- Movie CRUD and search work and are tested.

**Suggested commit**
```bash
git add .
git commit -m "feat: implement movie management api"
```

### Phase 5 — Theatre, Screen and Seat APIs

**Tasks**
- Implement theatre CRUD.
- Implement screen creation/listing/update/deletion and theatre filtering.
- Implement seat creation, bulk seat creation, listing by screen, update and deletion.
- Validate parent IDs and duplicate screen/seat coordinates.
- Prevent deletion when dependent history exists.

**Key endpoints**
- `/api/theatres`
- `/api/screens`
- `/api/screens/theatre/{theatreId}`
- `/api/seats`
- `/api/seats/bulk`
- `/api/seats/screen/{screenId}`

**Deliverable**
- Theatre → Screen → Seat setup works.

**Suggested commit**
```bash
git add .
git commit -m "feat: implement theatre screen and seat management"
```

### Phase 6 — Show APIs and automatic seat generation

**Tasks**
- Create show request/response DTOs.
- Implement show repository and service.
- Validate movie and screen existence.
- Validate `EndTime > StartTime` and non-negative ticket price.
- Check that the screen has no overlapping show.
- Create the show.
- In the same workflow, create one `ShowSeat` record for every physical seat on the selected screen, initially `Available`.
- Implement show filters and show-seat layout/availability endpoints.

**Endpoints**
- `POST /api/shows`
- `GET /api/shows`
- `GET /api/shows/{id}`
- `GET /api/shows/movie/{movieId}`
- `GET /api/shows/theatre/{theatreId}`
- `GET /api/shows/search?date=YYYY-MM-DD`
- `GET /api/shows/{showId}/seats`
- `GET /api/shows/{showId}/seats/available`
- `GET /api/shows/{showId}/seats/booked`

**Deliverable**
- Creating a show automatically creates its per-show seat availability records.

**Verification**
- Compare the number of `ShowSeat` records with the number of physical seats on the screen.
- Confirm all newly generated records have `Available` status.

**Suggested commit**
```bash
git add .
git commit -m "feat: implement show scheduling and seat availability"
```

### Phase 7 — Booking and cancellation

**Booking workflow**
1. Receive customer details, `ShowId` and a list of `SeatIds`.
2. Confirm the show exists.
3. Confirm the requested seat IDs are distinct.
4. Confirm every requested seat belongs to the selected show's screen and has a matching `ShowSeat`.
5. Confirm every requested show-seat is `Available`.
6. Calculate the total on the server using trusted show/seat data.
7. Create the `Booking` and its `BookingSeat` records.
8. Change the corresponding `ShowSeat` records to `Booked`.
9. Save all related changes in one database transaction.
10. Return a booking confirmation DTO.

**Cancellation workflow**
1. Load the booking and its seat details.
2. Confirm it exists and is currently `Confirmed`.
3. In one transaction, mark it `Cancelled`.
4. Set the associated `ShowSeat` records back to `Available`.
5. Preserve the booking and booking-seat history; do not delete it.

**Endpoints**
- `POST /api/bookings`
- `GET /api/bookings`
- `GET /api/bookings/{id}`
- `GET /api/bookings/show/{showId}`
- `PUT /api/bookings/{id}/cancel`

**Critical limitation**
A basic availability check and transaction do not by themselves prevent two simultaneous requests from booking the same seat. Advanced concurrency protection and temporary seat holds are future-version work.

**Deliverable**
- End-to-end booking and cancellation work with transaction consistency.

**Suggested commit**
```bash
git add .
git commit -m "feat: implement booking and cancellation workflows"
```

### Phase 8 — Validation and centralized exception handling

**Tasks**
- Complete validation for all modules.
- Add suitable custom exceptions where useful.
- Add centralized exception handling using `IExceptionHandler`.
- Return consistent `ProblemDetails` responses.
- Add structured logging.
- Never expose stack traces or raw internal exception details to API clients.

**Expected status codes**
- `200 OK`: successful read/update/operation with response.
- `201 Created`: resource created; use `CreatedAtAction` where practical.
- `204 No Content`: successful deletion without response body.
- `400 Bad Request`: invalid input.
- `404 Not Found`: resource does not exist.
- `409 Conflict`: business conflict, such as unavailable seats or deletion blocked by dependencies.
- `500 Internal Server Error`: unexpected failure.

**Suggested commit**
```bash
git add .
git commit -m "feat: add validation and centralized exception handling"
```

### Phase 9 — Integration testing and Version 1 completion

**End-to-end sequence**
1. Create movie.
2. Create theatre.
3. Create screen.
4. Bulk-create seats.
5. Create show.
6. Get show seat layout.
7. Create booking.
8. Get booking details.
9. Cancel booking.
10. Get show seat layout again and verify released seats.

**Additional tests**
- Invalid IDs.
- Duplicate seat IDs in a booking.
- Seat already booked.
- Invalid show time.
- Negative price.
- Screen scheduling overlap.
- Delete parent records with dependent records.
- Cancel a booking twice.
- Verify failed booking leaves no partial data.

**Deliverable**
- Tested Version 1 backend, API documentation and updated learning/revision documents.

**Suggested commit**
```bash
git add .
git commit -m "test: verify movie booking api end to end"
```

## 5. Documentation maintenance rules

Update `PROJECT-LEARNING.md` after each completed phase:
- Date and phase completed.
- Files/folders added or changed.
- What was implemented.
- Why the design was chosen.
- Commands executed.
- Build/test result.
- API requests and responses tested.
- Git commit hash and message.
- Problems encountered and their fixes.
- Next phase.

Update `PROJECT-REVISION.md` whenever a new concept is taught or a useful mistake is solved:
- Concept and simple definition.
- Why it is used in this project.
- Relevant code example.
- Step-by-step code explanation.
- Related database/API example.
- Common interview question and answer.
- Common error and solution.

Do not mark a phase complete until its build and verification steps pass.

## 6. Version boundaries

| Version | Planned work |
|---|---|
| V1 | Core backend modules and basic transaction-based booking |
| V2 | Concurrency protection, temporary seat holds and expiry |
| V3 | Identity, JWT, customer/admin authorization |
| V4 | Payment workflow and gateway integration |
| V5 | Angular frontend |
| V6 | Deployment, CI/CD, monitoring and production hardening |

## 7. Current project progress

| Phase | Status | Completion date | Commit |
|---|---|---|---|
| 1. Setup and planning | Not started | — | — |
| 2. Entities and EF Core | Not started | — | — |
| 3. Migrations/database | Not started | — | — |
| 4. Movie APIs | Not started | — | — |
| 5. Theatre/Screen/Seat APIs | Not started | — | — |
| 6. Show APIs | Not started | — | — |
| 7. Booking/cancellation | Not started | — | — |
| 8. Validation/errors | Not started | — | — |
| 9. Integration testing | Not started | — | — |

Update this table as work is completed. Do not assume a phase is complete merely because its files exist.

## 8. Reference design reminders

- Keep controllers thin; business rules belong in services.
- Repositories encapsulate data access.
- DTOs are API contracts, not EF entities.
- Use async methods for database I/O and suffix them with `Async`.
- Use `decimal(10,2)` for money.
- Use restrictive delete behavior when history must be preserved.
- The server calculates booking totals; never trust a client-supplied total.
- Use a consistent time-zone strategy for show schedules and persisted timestamps.
- Version 1 endpoints are unauthenticated and intended only for a controlled learning environment.


## Operation 1: Create Movie Entity

**File:** `MovieTicketBooking/Models/Movie.cs`

**Purpose:** Create the Movie entity to represent movie information stored in the database.

### Properties implemented

| Property | Purpose |
|---|---|
| MovieId | Primary key |
| Title | Movie title |
| Description | Optional movie description |
| DurationMinutes | Movie duration |
| ReleaseDate | Movie release date |
| Language | Movie language |
| Genre | Movie genre |

### Concepts learned

- A C# class represents an entity.
- Properties use `get` and `set` accessors.
- `string?` allows a nullable string.
- `string.Empty` initializes a string with an empty value.
- EF Core recognizes `MovieId` as the primary key by convention.

### Verification

- Created `Movie.cs` inside the Models folder.
- Saved the file.
- Visual Studio Build Solution succeeded.

### Git commit

`feat: add movie entity`


## Phase 2 – Entities and EF Core Configuration

### Operation 1: Create Movie Entity

**File:** `MovieTicketBooking/Models/Movie.cs`

**Objective:** Create a C# entity class to represent movie information that will eventually be stored in the `Movies` database table.

### Properties implemented

| Property | Data type | Purpose |
|---|---|---|
| MovieId | int | Unique identifier and primary key |
| Title | string | Movie title |
| Description | string? | Optional movie description |
| DurationMinutes | int | Movie duration in minutes |
| ReleaseDate | DateTime | Movie release date |
| Language | string | Movie language |
| Genre | string | Movie genre |

### Code concepts learned

1. **Class:** A class is a blueprint for creating objects. The `Movie` class represents a movie.
2. **Property:** Properties store information belonging to an object.
3. **get; set;:** `get` reads a property value, while `set` assigns or changes its value.
4. **Nullable string (`string?`):** Allows the Description property to contain null.
5. **string.Empty:** Initializes a string property with an empty string.
6. **Primary key convention:** EF Core recognizes `MovieId` as the primary key because it follows the `<EntityName>Id` naming convention.
7. **Entity:** An entity is a class that EF Core maps to a database table.

### Why did we create this entity?

The Movie entity is the starting point for movie management. Later, we will use it to implement:

- Adding a movie.
- Viewing all movies.
- Finding a movie by ID.
- Updating movie information.
- Deleting a movie.
- Searching movies by title.

### Verification

- Created `Models/Movie.cs` using Visual Studio.
- Added all seven properties.
- Saved the file.
- Built the solution successfully.

**Build Result:** Succeeded.

### Git history

**Commit type:** feat

**Commit message:**
`feat: add movie entity`

### Next operation

Create the Theatre entity with its properties and prepare it for the Theatre–Screen relationship.

### Operation 2: Create Theatre Entity

**File:** `MovieTicketBooking/Models/Theatre.cs`

**Objective:** Create an entity to represent theatres where movies are screened.

### Properties implemented

| Property | Data type | Purpose |
|---|---|---|
| TheatreId | int | Unique theatre identifier and primary key |
| Name | string | Name of the theatre |
| Location | string | Theatre location or address |

### Concepts learned

1. A Theatre entity represents a theatre in the movie booking system.
2. `TheatreId` is recognized as the primary key by EF Core convention.
3. `Name` stores the theatre name.
4. `Location` stores the theatre's address or location.
5. `string.Empty` initializes string properties with an empty value.

### Why do we need the Theatre entity?

A theatre contains one or more screens. Each screen belongs to a theatre.

This entity will later be connected to the Screen entity through a one-to-many relationship.

**Example:**

- TheatreId: 1
- Name: PVR Cinemas
- Location: Thiruvananthapuram

### Verification

- Created `Models/Theatre.cs` using Visual Studio.
- Added TheatreId, Name and Location properties.
- Saved the file.
- Visual Studio Build Solution succeeded.

**Build Result:** Succeeded.

### Git history

**Commit type:** feat

**Commit message:**
`feat: add theatre entity`

### Next operation

Create the Screen entity, which will contain the TheatreId foreign key.

### Operation 3: Create Screen Entity

**File:** `MovieTicketBooking/Models/Screen.cs`

**Objective:** Create an entity to represent individual screens inside a theatre.

### Properties implemented

| Property | Data type | Purpose |
|---|---|---|
| ScreenId | int | Unique screen identifier and primary key |
| TheatreId | int | Foreign key connecting Screen to Theatre |
| ScreenName | string | Name or number of the screen |

### Concepts learned

1. A Screen entity represents an individual cinema screen.
2. `ScreenId` is recognized as the primary key by EF Core convention.
3. `TheatreId` identifies the theatre to which the screen belongs.
4. `TheatreId` acts as a foreign key.
5. One theatre can contain multiple screens.

### Example

Theatre: PVR Cinemas

| ScreenId | TheatreId | ScreenName |
|---|---|---|
| 1 | 1 | Screen 1 |
| 2 | 1 | Screen 2 |
| 3 | 1 | Screen 3 |

All three screens belong to TheatreId 1.

### Verification

- Created `Models/Screen.cs` using Visual Studio.
- Added ScreenId, TheatreId and ScreenName properties.
- Saved the file.
- Visual Studio Build Solution succeeded.

**Build Result:** Succeeded.

### Git history

**Commit type:** feat

**Commit message:**
`feat: add screen entity`

### Next operation

Create the Seat entity, which represents individual physical seats inside a screen.

### Operation 4: Create Seat Entity

**File:** `MovieTicketBooking/Models/Seat.cs`

**Objective:** Create an entity to represent individual physical seats inside a cinema screen.

### Properties implemented

| Property | Data type | Purpose |
|---|---|---|
| SeatId | int | Unique seat identifier and primary key |
| ScreenId | int | Foreign key connecting Seat to Screen |
| RowName | string | Identifies the seat row |
| SeatNumber | int | Identifies the seat number within a row |
| SeatType | string | Represents the seat category |

### Concepts learned

1. A Seat entity represents an individual physical seat.
2. `SeatId` is recognized as the primary key by EF Core convention.
3. `ScreenId` connects a seat to its screen through a foreign key.
4. `RowName` identifies the row, such as A, B or C.
5. `SeatNumber` identifies the position within a row.
6. `SeatType` stores the category, such as Regular or Premium.
7. A screen can contain multiple physical seats.

### Example seating arrangement

| ScreenId | RowName | SeatNumber | SeatType |
|---|---|---|---|
| 1 | A | 1 | Regular |
| 1 | A | 2 | Regular |
| 1 | A | 3 | Premium |
| 1 | B | 1 | Regular |

### Important database rule

The combination of `ScreenId`, `RowName` and `SeatNumber` must be unique.

This prevents duplicate physical seat positions within the same screen.

We will configure this unique constraint later using EF Core.

### Verification

- Created `Models/Seat.cs` using Visual Studio.
- Added all five properties.
- Saved the file.
- Visual Studio Build Solution succeeded.

**Build Result:** Succeeded.

### Git history

**Commit type:** feat

**Commit message:**
`feat: add seat entity`

### Next operation

Create the Show entity to represent movie screening schedules.

### Operation 5: Create Show Entity

**File:** `MovieTicketBooking/Models/Show.cs`

**Objective:** Create an entity to represent a movie screening scheduled on a particular screen at a specific date and time.

### Properties implemented

| Property | Data type | Purpose |
|---|---|---|
| ShowId | int | Unique show identifier and primary key |
| MovieId | int | Foreign key connecting Show to Movie |
| ScreenId | int | Foreign key connecting Show to Screen |
| StartTime | DateTime | Movie screening start date and time |
| EndTime | DateTime | Movie screening end date and time |
| TicketPrice | decimal | Price of one ticket |

### Concepts learned

1. A Show entity represents a scheduled movie screening.
2. `ShowId` is recognized as the primary key by EF Core convention.
3. `MovieId` connects a show to a movie.
4. `ScreenId` connects a show to a screen.
5. `DateTime` stores both date and time.
6. `decimal` is suitable for storing monetary values.

### Example

| Property | Example value |
|---|---|
| ShowId | 1 |
| MovieId | 1 |
| ScreenId | 1 |
| StartTime | 2026-11-05 18:00 |
| EndTime | 2026-11-05 20:45 |
| TicketPrice | 200.00 |

### Important business rules

The following rules will be implemented during validation and service development:

- EndTime must be later than StartTime.
- TicketPrice cannot be negative.
- Two shows must not overlap on the same screen.

### Verification

- Created `Models/Show.cs` using Visual Studio.
- Added all six properties.
- Saved the file.
- Visual Studio Build Solution succeeded.

**Build Result:** Succeeded.

### Git history

**Commit type:** feat

**Commit message:**
`feat: add show entity`

### Next operation

Create the ShowSeat entity to track the availability of physical seats for individual shows.

### Operation 6: Create ShowSeat Entity

**File:** `MovieTicketBooking/Models/ShowSeat.cs`

**Objective:** Create an entity to track the availability of each physical seat for an individual movie show.

### Properties implemented

| Property | Data type | Purpose |
|---|---|---|
| ShowSeatId | int | Unique identifier and primary key |
| ShowId | int | Foreign key connecting Show to ShowSeat |
| SeatId | int | Foreign key connecting physical Seat to ShowSeat |
| Status | string | Stores the current seat availability status |

### Concepts learned

1. A ShowSeat entity represents a physical seat's availability for a particular show.
2. `ShowSeatId` is recognized as the primary key by EF Core convention.
3. `ShowId` connects the availability record to a movie show.
4. `SeatId` connects the availability record to a physical seat.
5. `Status` stores whether the seat is Available or Booked.
6. The default value of Status is initialized as `"Available"`.

### Why do we need ShowSeat?

A physical seat can have different availability statuses for different shows.

Example:

| Show | Physical Seat | Status |
|---|---|---|
| 6:00 PM | A1 | Booked |
| 9:00 PM | A1 | Available |

The Seat entity stores physical seat information, whereas ShowSeat stores availability for an individual show.

### Important database rule

The combination of `ShowId` and `SeatId` must be unique.

This prevents duplicate availability records for the same physical seat in the same show.

### Verification

- Created `Models/ShowSeat.cs` using Visual Studio.
- Added ShowSeatId, ShowId, SeatId and Status properties.
- Saved the file.
- Visual Studio Build Solution succeeded.

**Build Result:** Succeeded.

### Git history

**Commit type:** feat

**Commit message:**
`feat: add show seat entity`

### Next operation

Create the Booking entity to store customer booking information.
