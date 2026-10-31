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
