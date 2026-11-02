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

### Operation 7: Create Booking Entity

**File created:** `Models/Booking.cs`

**Purpose:** The Booking entity stores customer booking information for a particular movie show.

**Properties added:**

| Property | Purpose |
|---|---|
| BookingId | Primary key for the booking |
| BookingReference | Unique reference provided for the booking |
| ShowId | Identifies the selected show |
| CustomerName | Stores the customer's name |
| CustomerEmail | Stores the customer's email |
| CustomerPhone | Stores the customer's phone number |
| BookingDate | Stores when the booking was created |
| TotalAmount | Stores the total booking amount |
| Status | Stores the booking status; defaults to Confirmed |

**Concepts learned:**

- `decimal` is suitable for storing monetary values.
- `DateTime` stores the booking date and time.
- `ShowId` connects a booking to its selected show.
- `BookingReference` will be configured with a unique database constraint.
- Booking status will be either `Confirmed` or `Cancelled`.

**Verification:** Visual Studio Build Solution completed successfully.

**Git commit:** `feat: add booking entity`


### Operation 8: Create BookingSeat Entity

**File created:** `Models/BookingSeat.cs`

**Purpose:**

The BookingSeat entity connects a booking with the individual seats selected by the customer.

**Properties implemented:**

| Property | Purpose |
|---|---|
| BookingSeatId | Primary key for the booking-seat record |
| BookingId | Identifies the related booking |
| SeatId | Identifies the selected physical seat |
| Price | Stores the price charged for this seat |

**Important concepts learned:**

- One booking can contain multiple booking-seat records.
- Each booking-seat record represents one selected seat.
- `decimal` is used for the seat price.
- `BookingId` connects the record to a booking.
- `SeatId` identifies the physical seat.
- `SeatId` must not be globally unique in this table because the same physical seat can be booked for different shows.

**Example:**

If Booking ID 101 contains seats 5, 6 and 7, three BookingSeat records are created.

**Verification:**

- Created `BookingSeat.cs` inside the Models folder.
- Built the project successfully.
- No compilation errors.

**Git commit:**

`feat: add booking seat entity`

### Operation 9: Add Navigation Property to Movie

**File modified:** `Models/Movie.cs`

**Purpose:**

Added a navigation property to connect Movie with its related Shows.

**Code added:**

```csharp
public ICollection<Show> Shows { get; set; } = new List<Show>();
```

**Important concepts learned:**

- A navigation property represents a relationship between entities.
- `ICollection<Show>` represents multiple related Show objects.
- One Movie can have many Shows.
- `new List<Show>()` initializes an empty collection.
- Navigation properties help EF Core understand entity relationships.

**Relationship:**

Movie (One) → Shows (Many)

**Verification:**

- Added the Shows navigation property to Movie.cs.
- Built the project successfully.
- No compilation errors.

**Git commit:**

`feat: add movie shows navigation property`

public ICollection<Screen> Screens { get; set; } = new List<Screen>();

### Operation 11: Add Navigation Properties to Screen

**File modified:** `Models/Screen.cs`

**Purpose:**

Added navigation properties to connect Screen with its related Theatre and Seats.

**Code added:**

```csharp
public Theatre Theatre { get; set; } = null!;

public ICollection<Seat> Seats { get; set; } = new List<Seat>();
```

**Important concepts learned:**

- A Screen belongs to one Theatre.
- A Screen can contain multiple Seats.
- `TheatreId` is the foreign key connecting Screen to Theatre.
- `Theatre` is a reference navigation property.
- `Seats` is a collection navigation property.
- `null!` suppresses the nullable-reference warning for a required navigation property that EF Core will populate.
- `new List<Seat>()` initializes the Seats collection.

**Relationships:**

- Theatre (One) → Screens (Many)
- Screen (One) → Seats (Many)

**Verification:**

- Added Theatre and Seats navigation properties to Screen.cs.
- Built the project successfully.
- No compilation errors.

**Git commit:**

`feat: add screen navigation properties`

### Operation 12: Add Navigation Properties to Seat

**File modified:** `Models/Seat.cs`

**Purpose:**

Added navigation properties to connect Seat with Screen, ShowSeat and BookingSeat.

**Code added:**

```csharp
public Screen Screen { get; set; } = null!;

public ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();

public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
```

**Important concepts learned:**

- Each Seat belongs to one Screen.
- One Seat can have multiple ShowSeat records.
- One Seat can have multiple BookingSeat records across different shows and bookings.
- `Screen` is a reference navigation property.
- `ShowSeats` and `BookingSeats` are collection navigation properties.
- `ScreenId` is the foreign key connecting Seat to Screen.
- `null!` suppresses the nullable-reference warning for a navigation property populated by EF Core.

**Relationships:**

- Screen (One) → Seats (Many)
- Seat (One) → ShowSeats (Many)
- Seat (One) → BookingSeats (Many)

**Verification:**

- Added three navigation properties to Seat.cs.
- Built the project successfully.
- No compilation errors.

**Git commit:**

`feat: add seat navigation properties`

### Operation 13: Add Navigation Properties to Show

**File modified:** `Models/Show.cs`

**Purpose:**

Added navigation properties connecting Show with Movie, Screen, ShowSeat and Booking.

**Code added:**

```csharp
public Movie Movie { get; set; } = null!;

public Screen Screen { get; set; } = null!;

public ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();

public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
```

**Important concepts learned:**

- Each Show belongs to one Movie.
- Each Show takes place on one Screen.
- One Show can have multiple ShowSeat records.
- One Show can have multiple Bookings.
- `MovieId` and `ScreenId` are foreign keys.
- `Movie` and `Screen` are reference navigation properties.
- `ShowSeats` and `Bookings` are collection navigation properties.

**Relationships:**

- Movie (One) → Shows (Many)
- Screen (One) → Shows (Many)
- Show (One) → ShowSeats (Many)
- Show (One) → Bookings (Many)

**Verification:**

- Added four navigation properties to Show.cs.
- Built the project successfully.
- No compilation errors.

**Git commit:**

`feat: add show navigation properties`

### Operation 14: Add Navigation Properties to ShowSeat

**File modified:** `Models/ShowSeat.cs`

**Purpose:**

Added navigation properties to connect ShowSeat with Show and Seat.

**Code added:**

```csharp
public Show Show { get; set; } = null!;

public Seat Seat { get; set; } = null!;
```

**Important concepts learned:**

- Each ShowSeat belongs to one Show.
- Each ShowSeat belongs to one physical Seat.
- `ShowId` is the foreign key to Show.
- `SeatId` is the foreign key to Seat.
- `Show` and `Seat` are reference navigation properties.
- `Status` stores the availability of a seat for a particular show.
- A unique constraint on `(ShowId, SeatId)` will prevent duplicate availability records for the same seat in one show.

**Relationships:**

- Show (One) → ShowSeats (Many)
- Seat (One) → ShowSeats (Many)

**Verification:**

- Added Show and Seat navigation properties to ShowSeat.cs.
- Built the project successfully.
- No compilation errors.

**Git commit:**

`feat: add show seat navigation properties`

 ### Operation 15: Add Navigation Properties to Booking

**File modified:** `Models/Booking.cs`

**Purpose:**

Added navigation properties connecting Booking with Show and BookingSeat.

**Code added:**

```csharp
public Show Show { get; set; } = null!;

public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
```

**Important concepts learned:**

- Each Booking belongs to one Show.
- One Show can have multiple Bookings.
- One Booking can contain multiple BookingSeat records.
- `ShowId` is the foreign key connecting Booking to Show.
- `Show` is a reference navigation property.
- `BookingSeats` is a collection navigation property.
- `null!` suppresses the nullable-reference warning for a required navigation property populated by EF Core.

**Relationships:**

- Show (One) → Bookings (Many)
- Booking (One) → BookingSeats (Many)

**Verification:**

- Added Show and BookingSeats navigation properties to Booking.cs.
- Built the project successfully.
- No compilation errors.

**Git commit:**

`feat: add booking navigation properties`

### Operation 16: Add Navigation Properties to BookingSeat

**File modified:** `Models/BookingSeat.cs`

**Purpose:**

Added navigation properties connecting BookingSeat with Booking and Seat.

**Code added:**

```csharp
public Booking Booking { get; set; } = null!;

public Seat Seat { get; set; } = null!;
```

**Important concepts learned:**

- Each BookingSeat belongs to one Booking.
- Each BookingSeat belongs to one physical Seat.
- `BookingId` is the foreign key connecting BookingSeat to Booking.
- `SeatId` is the foreign key connecting BookingSeat to Seat.
- `Booking` and `Seat` are reference navigation properties.
- Each BookingSeat record represents one selected seat in a booking.
- SeatId must not be globally unique because the same physical seat can be booked in different shows.

**Relationships:**

- Booking (One) → BookingSeats (Many)
- Seat (One) → BookingSeats (Many)

**Verification:**

- Added Booking and Seat navigation properties to BookingSeat.cs.
- Built the project successfully.
- No compilation errors.

**Git commit:**

`feat: add booking seat navigation properties`


### Operation 17: Create ApplicationDbContext

**File created:** `Data/ApplicationDbContext.cs`

**Purpose:**

Created the ApplicationDbContext class to connect the application's entity models with Entity Framework Core.

**Implementation details:**

- Inherited from `DbContext`.
- Added a constructor accepting `DbContextOptions<ApplicationDbContext>`.
- Used `base(options)` to pass configuration to the parent DbContext.
- Added eight DbSet properties:
  - Movies
  - Theatres
  - Screens
  - Seats
  - Shows
  - ShowSeats
  - Bookings
  - BookingSeats

**Concepts learned:**

- DbContext acts as a bridge between the application and database.
- DbSet represents a collection of entities that EF Core maps to database tables.
- DbContext tracks changes and saves them to the database.

**Verification:**

- Installed the required EF Core packages.
- Built the project successfully.
- No build errors.

**Git commit:**

`feat: create application db context`


### Operation 18: Register ApplicationDbContext with SQL Server

**File modified:** `Program.cs`

**Purpose:**

Registered ApplicationDbContext with ASP.NET Core Dependency Injection and configured SQL Server as the database provider.

**Implementation details:**

- Added `Microsoft.EntityFrameworkCore` namespace.
- Added `MovieTicketBooking.Data` namespace.
- Used `builder.Services.AddDbContext<ApplicationDbContext>()`.
- Configured SQL Server using `options.UseSqlServer()`.
- Read the connection string using `builder.Configuration.GetConnectionString("DefaultConnection")`.

**Concepts learned:**

- Dependency Injection (DI).
- DbContext registration.
- SQL Server database provider.
- Reading connection strings from appsettings.json.

**Verification:**

- Updated Program.cs.
- Built the project successfully.
- No build errors.

**Git commit:**

`feat: register sql server db context`


### Operation 19: Create OnModelCreating Method

**File modified:** `Data/ApplicationDbContext.cs`

**Purpose:**

Created the OnModelCreating method to configure database entities, properties, relationships, and constraints using EF Core Fluent API.

**Implementation:**

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
}
```

**Concepts learned:**

- `OnModelCreating()` is an override method provided by EF Core's DbContext.
- `ModelBuilder` is used to configure entity models.
- `base.OnModelCreating(modelBuilder)` calls the base DbContext implementation.
- Fluent API provides control over database table and column configuration.

**Verification:**

- Updated ApplicationDbContext.cs.
- Built the project successfully.
- No build errors.

**Git commit:**

`feat: add entity model configuration method`


## Operation 20 – Configure All Entities Using EF Core Fluent API

**Objective:** Configure all eight database entities using `OnModelCreating()` in `ApplicationDbContext.cs`.

### Implementation

Configured the following entities:

1. **Movie**
   - Configured primary key `MovieId`.
   - Made `Title`, `DurationMinutes`, `ReleaseDate`, `Language`, and `Genre` required.
   - Set maximum lengths for `Title`, `Description`, `Language`, and `Genre`.
   - Added a check constraint to ensure `DurationMinutes > 0`.

2. **Theatre**
   - Configured primary key `TheatreId`.
   - Made `Name` and `Location` required.
   - Configured maximum lengths.

3. **Screen**
   - Configured primary key `ScreenId`.
   - Configured the relationship between `Screen` and `Theatre`.
   - Used `DeleteBehavior.Restrict`.
   - Added a unique index on `TheatreId` and `ScreenName` to prevent duplicate screen names within the same theatre.

4. **Seat**
   - Configured primary key `SeatId`.
   - Configured the relationship between `Seat` and `Screen`.
   - Used `DeleteBehavior.Restrict`.
   - Added a check constraint to ensure `SeatNumber > 0`.
   - Added a unique index on `ScreenId`, `RowName`, and `SeatNumber`.

5. **Show**
   - Configured primary key `ShowId`.
   - Configured relationships with `Movie` and `Screen`.
   - Used `DeleteBehavior.Restrict` for both relationships.
   - Configured `StartTime` and `EndTime` as `datetime2`.
   - Configured `TicketPrice` with precision `(10,2)`.
   - Added check constraints to ensure `EndTime > StartTime` and `TicketPrice >= 0`.

6. **ShowSeat**
   - Configured primary key `ShowSeatId`.
   - Configured relationships with `Show` and `Seat`.
   - Used `DeleteBehavior.Restrict`.
   - Added a unique index on `ShowId` and `SeatId`.
   - Added a check constraint allowing only `Available` or `Booked` statuses.

7. **Booking**
   - Configured primary key `BookingId`.
   - Configured the relationship with `Show`.
   - Used `DeleteBehavior.Restrict`.
   - Added a unique index on `BookingReference`.
   - Configured `TotalAmount` with precision `(10,2)`.
   - Added check constraints to ensure `TotalAmount >= 0` and status is either `Confirmed` or `Cancelled`.

8. **BookingSeat**
   - Configured primary key `BookingSeatId`.
   - Configured relationships with `Booking` and `Seat`.
   - Used `DeleteBehavior.Restrict`.
   - Configured `Price` with precision `(10,2)`.
   - Added a check constraint to ensure `Price >= 0`.

### Important EF Core Concepts Learned

- **HasKey():** Configures the primary key of an entity.
- **IsRequired():** Makes a property mandatory in the database.
- **HasMaxLength():** Sets the maximum length of a string property.
- **HasPrecision():** Configures decimal precision and scale.
- **HasOne() / WithMany():** Configures one-to-many relationships.
- **HasForeignKey():** Specifies the foreign key property.
- **OnDelete(DeleteBehavior.Restrict):** Prevents deleting a parent record when dependent records exist.
- **HasIndex().IsUnique():** Prevents duplicate values for the configured index or combination of columns.
- **HasCheckConstraint():** Enforces a condition on database values.

### Build Verification

Command:

```powershell
dotnet build
```

Result: **Build succeeded.**

### Git Commit

```text
feat: configure all entities using fluent api
```

## Operation 21 – Create Initial EF Core Migration

**Objective:** Generate the initial database migration using Entity Framework Core.

### Command Executed

```powershell
dotnet ef migrations add InitialCreate
```

### Migration Files Generated

EF Core generated the following files inside the `Migrations` folder:

1. **InitialCreate.cs**
   - Contains migration instructions to create the database tables.
   - Contains table columns, primary keys, foreign keys, indexes, and constraints.

2. **InitialCreate.Designer.cs**
   - Contains migration metadata and migration information.

3. **ApplicationDbContextModelSnapshot.cs**
   - Stores a snapshot of the current EF Core model.
   - EF Core uses it to identify model changes when generating future migrations.

### Navigation Property Correction

During migration creation, a build error occurred because the `Screen` entity did not contain the `Shows` navigation property.

Added:

```csharp
public ICollection<Show> Shows { get; set; } = new List<Show>();
```

This represents the one-to-many relationship between Screen and Show.

### Important Concepts Learned

- **Migration:** A versioned set of database schema changes generated from the EF Core model.
- **Add Migration:** Generates migration files based on entity and DbContext configuration.
- **Model Snapshot:** Stores the current model structure for comparison with future changes.
- **Migration Designer:** Stores metadata associated with the migration.

### Build Verification

Command:

```powershell
dotnet build
```

Result: **Build succeeded.**

### Migration Verification

Command:

```powershell
dotnet ef migrations add InitialCreate
```

Result: **Migration created successfully.**

### Important Note

Creating a migration does not create the database. The database will be created or updated in the next operation using `dotnet ef database update`.

### Git Commit

```text
feat: create initial database migration
```


## Operation 22 – Apply Migration to SQL Server

**Objective:** Apply the initial EF Core migration to SQL Server and create the MovieTicketBookingDB database.

### Command Executed

```powershell
dotnet ef database update
```

### What Happened?

1. EF Core read the configured connection string from `appsettings.json`.
2. Connected to SQL Server Express.
3. Created the `MovieTicketBookingDB` database if it did not exist.
4. Applied the `InitialCreate` migration.
5. Created the database tables, primary keys, foreign keys, indexes, and check constraints.
6. Recorded the applied migration in the `__EFMigrationsHistory` table.

### Database Configuration

- Server: `localhost\SQLEXPRESS`
- Database: `MovieTicketBookingDB`
- Authentication: Windows Integrated Security
- Provider: SQL Server

### Important Concepts Learned

- **Database Update:** Applies pending EF Core migrations to the database.
- **Migration History:** EF Core tracks applied migrations in `__EFMigrationsHistory`.
- **Connection String:** Defines how the application connects to SQL Server.
- **Database Schema:** Contains tables, columns, relationships, indexes, and constraints.

### Verification

Command:

```powershell
dotnet ef database update
```

Result: **Succeeded.**

The initial migration was successfully applied to SQL Server.

### Difference Between Migration Commands

| Command | Purpose |
|---|---|
| `dotnet ef migrations add InitialCreate` | Generates migration files |
| `dotnet ef database update` | Applies migrations to SQL Server |
| `dotnet ef migrations list` | Lists available migrations |

### Git Commit

```text
feat: apply initial migration to sql server
```


## Operation 23 – Verify Database Tables and Relationships

**Objective:** Verify that EF Core successfully created the database tables, primary keys, foreign keys, and unique indexes in SQL Server.

### Database

- Database Name: MovieTicketBookingDB
- Server: localhost\SQLEXPRESS
- Tool: SQL Server Management Studio (SSMS)

### 1. Verify Database Tables

Executed:

```sql
USE MovieTicketBookingDB;
GO

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
```

Verified the eight application tables:

1. Movies
2. Theatres
3. Screens
4. Seats
5. Shows
6. ShowSeats
7. Bookings
8. BookingSeats

Also verified the EF Core migration history table: `__EFMigrationsHistory`.

### 2. Verify Primary Keys

Used `INFORMATION_SCHEMA.KEY_COLUMN_USAGE` to inspect primary key columns and their constraint names.

Verified that each application table has its primary key.

### 3. Verify Foreign Keys

Used SQL Server system catalog views:

- `sys.foreign_keys`
- `sys.foreign_key_columns`

Verified the following relationships:

| Child table | Parent table |
|---|---|
| Screens | Theatres |
| Seats | Screens |
| Shows | Movies |
| Shows | Screens |
| ShowSeats | Shows |
| ShowSeats | Seats |
| Bookings | Shows |
| BookingSeats | Bookings |
| BookingSeats | Seats |

### 4. Verify Unique Indexes

Used `sys.indexes` and `sys.tables` to inspect unique indexes.

Verified these unique constraints:

- Screens: TheatreId + ScreenName
- Seats: ScreenId + RowName + SeatNumber
- ShowSeats: ShowId + SeatId
- Bookings: BookingReference

### Verification Result

**Succeeded.**

The database tables, primary keys, foreign keys, and unique indexes were successfully verified using SQL Server Management Studio.

### Concepts Learned

- `INFORMATION_SCHEMA.TABLES` – Retrieves database table information.
- `INFORMATION_SCHEMA.KEY_COLUMN_USAGE` – Retrieves key column information.
- `sys.foreign_keys` – Provides foreign key information.
- `sys.foreign_key_columns` – Provides foreign key column mappings.
- `sys.indexes` – Provides index information.
- `is_unique` – Indicates whether an index enforces uniqueness.

### Git Commit

```text
docs: document database schema verification
```

## Operation 24 – Implement Complete Repository Layer

**Objective:** Implement the repository layer using Entity Framework Core to separate database operations from business logic.

### 1. Repository Architecture

Implemented the following application flow:

Controller → Service → Repository → ApplicationDbContext → SQL Server

The repository layer communicates with the database using EF Core. The service layer will use repository interfaces to execute business operations.

### 2. Repository Interfaces Created

Created six repository interfaces inside `Repositories/Interfaces/`:

1. `IMovieRepository`
2. `ITheatreRepository`
3. `IScreenRepository`
4. `ISeatRepository`
5. `IShowRepository`
6. `IBookingRepository`

Each interface defines the database operations required for its entity.

### 3. Repository Implementations Created

Created six implementation classes inside `Repositories/`:

1. `MovieRepository`
2. `TheatreRepository`
3. `ScreenRepository`
4. `SeatRepository`
5. `ShowRepository`
6. `BookingRepository`

### 4. MovieRepository

Implemented:
- Add a movie.
- Retrieve movies with pagination.
- Filter movies by title.
- Retrieve a movie by ID.
- Update a movie.
- Delete a movie.

Used `Skip()` and `Take()` for pagination.

### 5. TheatreRepository

Implemented:
- Add a theatre.
- Retrieve all theatres.
- Retrieve a theatre by ID.
- Update a theatre.
- Delete a theatre.

### 6. ScreenRepository

Implemented:
- Add a screen.
- Retrieve all screens.
- Retrieve a screen by ID.
- Retrieve screens by theatre ID.
- Update a screen.
- Delete a screen.

Used `Include()` to retrieve related theatre information.

### 7. SeatRepository

Implemented:
- Add a seat.
- Add multiple seats using `AddRangeAsync()`.
- Retrieve all seats.
- Retrieve a seat by ID.
- Retrieve seats by screen ID.
- Update a seat.
- Delete a seat.

Used `OrderBy()` and `ThenBy()` to organize seat records.

### 8. ShowRepository

Implemented:
- Add a show.
- Retrieve all shows.
- Retrieve a show by ID.
- Retrieve shows by movie ID.
- Retrieve shows by screen ID.
- Retrieve shows by date.
- Update a show.

Used `Include()` to retrieve related movie and screen information.

### 9. BookingRepository

Implemented:
- Add a booking.
- Retrieve a booking by ID.
- Retrieve a booking using its reference.
- Update a booking.

Used `Include()` and `ThenInclude()` to retrieve related show, movie, booking-seat, and seat information.

### 10. Important EF Core Methods Learned

| Method | Purpose |
|---|---|
| `AddAsync()` | Adds an entity asynchronously |
| `AddRangeAsync()` | Adds multiple entities |
| `ToListAsync()` | Retrieves a list of records |
| `FirstOrDefaultAsync()` | Retrieves the first matching record or null |
| `FindAsync()` | Finds an entity using its primary key |
| `SaveChangesAsync()` | Saves database changes |
| `Update()` | Marks an entity for updating |
| `Remove()` | Marks an entity for deletion |
| `AsNoTracking()` | Retrieves entities without change tracking |
| `Include()` | Loads related entity data |
| `ThenInclude()` | Loads nested related entity data |
| `Where()` | Filters records |
| `OrderBy()` | Sorts records |
| `Skip()` | Skips records for pagination |
| `Take()` | Limits the number of records |

### 11. Asynchronous Programming

Repository methods use `async` and `await` for database operations.

This allows database calls to execute asynchronously without blocking the request thread while waiting for the database.

### 12. Important Design Decisions

- Repository interfaces define operation contracts.
- Repository classes contain EF Core database logic.
- Constructor injection provides `ApplicationDbContext`.
- `AsNoTracking()` is used for read-only queries.
- `DeleteBehavior.Restrict` protects records that have dependent data.
- Booking cancellation and seat availability updates will be handled in the service layer using transactions.

### 13. Build Verification

Command:

```powershell
dotnet build
```

Result: **Build succeeded.**

### Git Commit

```text
feat: implement repository layer
```


## Operation 25 – Register Repositories Using Dependency Injection

### Objective
Register all six repository interfaces and their implementation classes in ASP.NET Core using Dependency Injection (DI).

### Changes made

Updated `Program.cs` with the following namespaces:

- `MovieTicketBooking.Repositories.Interfaces`
- `MovieTicketBooking.Repositories.Implementation`

Registered six repositories using `AddScoped`:

```csharp
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
```

### Understanding AddScoped

`AddScoped` creates one instance of a registered service per HTTP request. The same instance is reused within that request.

### Dependency Injection flow

1. A controller requests a repository interface.
2. ASP.NET Core checks the registered services.
3. DI creates the corresponding repository implementation.
4. The repository receives `ApplicationDbContext` through constructor injection.
5. The repository performs database operations using Entity Framework Core.

### Repository registration mapping

| Interface | Implementation |
|---|---|
| IMovieRepository | MovieRepository |
| ITheatreRepository | TheatreRepository |
| IScreenRepository | ScreenRepository |
| ISeatRepository | SeatRepository |
| IShowRepository | ShowRepository |
| IBookingRepository | BookingRepository |

### Verification

- Added all six repository registrations.
- Verified the project structure.
- Executed `dotnet build`.
- Build completed successfully.

**Result:** All six repositories are registered and available for Dependency Injection. 

**Next operation:** Implement the service layer.

## Operation 26 – Implement Movie, Theatre, Screen and Seat Services

### Objective
Implement the service layer for the first four entities in the Movie Ticket Booking System.

### Files created

**Interfaces folder**
- IMovieService.cs
- ITheatreService.cs
- IScreenService.cs
- ISeatService.cs

**Implementations folder**
- MovieService.cs
- TheatreService.cs
- ScreenService.cs
- SeatService.cs

### Architecture

Controller → Service → Repository → ApplicationDbContext → SQL Server

### Responsibilities

**MovieService**
- Add, retrieve, update and delete movies.
- Validate movie title, duration, language and genre.
- Validate pagination parameters.
- Check movie existence before updating.

**TheatreService**
- Manage theatre records.
- Validate theatre name and location.
- Check theatre existence before updating.

**ScreenService**
- Manage screens.
- Validate screen name.
- Verify that the referenced theatre exists.
- Retrieve screens belonging to a theatre.

**SeatService**
- Manage individual seats and bulk seat insertion.
- Validate row name, seat number and seat type.
- Verify that the referenced screen exists.
- Retrieve seats belonging to a screen.

### Important programming concepts

- Interface-based programming
- Constructor injection
- Asynchronous methods using Task and async/await
- Repository pattern
- Input validation
- Entity existence validation
- List-based bulk operations

### Verification

- Created four service interfaces.
- Implemented four service classes.
- Connected services to repository interfaces.
- Executed `dotnet build`.
- Build succeeded.

**Result:** The first four service modules are implemented and compile successfully.

**Next operation:** Implement ShowService and BookingService with scheduling, seat availability and booking business logic.



# Operation 29 – Implement API Controllers

## Objective
Implement the ASP.NET Core Web API Controller layer for the Movie Ticket Booking System and connect HTTP requests to the Service layer using Dependency Injection.

## Controllers Created

Six controllers were implemented:

1. **MoviesController**
   - Create a movie.
   - Retrieve all movies with pagination and optional title filtering.
   - Retrieve a movie by ID.
   - Update a movie.
   - Delete a movie.

2. **TheatresController**
   - Create a theatre.
   - Retrieve all theatres.
   - Retrieve a theatre by ID.
   - Update a theatre.
   - Delete a theatre.

3. **ScreensController**
   - Create a screen.
   - Retrieve all screens.
   - Retrieve a screen by ID.
   - Retrieve screens by theatre ID.
   - Update a screen.
   - Delete a screen.

4. **SeatsController**
   - Create a seat.
   - Create multiple seats using bulk insertion.
   - Retrieve all seats.
   - Retrieve a seat by ID.
   - Retrieve seats by screen ID.
   - Update a seat.
   - Delete a seat.

5. **ShowsController**
   - Create a show.
   - Retrieve all shows.
   - Retrieve a show by ID.
   - Retrieve shows by movie ID.
   - Retrieve shows by screen ID.
   - Retrieve shows by date.
   - Update a show.

6. **BookingsController**
   - Create a booking.
   - Retrieve a booking by ID.
   - Retrieve a booking using its reference.
   - Cancel a booking.

## Important ASP.NET Core Concepts

### 1. ApiController Attribute
`[ApiController]` identifies a class as an API controller and enables API-specific behaviors such as automatic model validation responses.

### 2. Route Attribute
`[Route("api/[controller]")]` defines the base URL using the controller name.

Examples:
- `/api/Movies`
- `/api/Theatres`
- `/api/Screens`
- `/api/Seats`
- `/api/Shows`
- `/api/Bookings`

### 3. HTTP Method Attributes
- `[HttpGet]` – Retrieve data.
- `[HttpPost]` – Create data.
- `[HttpPut]` – Update data.
- `[HttpDelete]` – Delete data.

### 4. Dependency Injection
Controllers receive their respective service interfaces through constructors instead of creating service objects manually.

Example:

```csharp
private readonly IMovieService _movieService;

public MoviesController(IMovieService movieService)
{
    _movieService = movieService;
}
```

### 5. IActionResult
`IActionResult` allows controller actions to return different HTTP responses.

Common responses:
- `Ok()` – HTTP 200.
- `CreatedAtAction()` – HTTP 201.
- `NoContent()` – HTTP 204.
- `BadRequest()` – HTTP 400.
- `NotFound()` – HTTP 404.

## Application Architecture

```text
Client / API Request
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

## Learning Outcome

- Understood controller-based Web API development.
- Implemented HTTP endpoints for six application modules.
- Connected controllers with service interfaces.
- Practised constructor-based Dependency Injection.
- Learned how to return appropriate HTTP status codes.
- Completed the initial Controller layer.

## Build Verification

Command:

```powershell
dotnet build
```

Result: Build succeeded.

**Next operation:** Improve API request models and error handling, then test the endpoints using Scalar.
