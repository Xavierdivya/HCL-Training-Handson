# Course Management System

A full-stack course management application built with **ASP.NET Core Web
API**, **Entity Framework Core**, **SQL Server**, and a lightweight
**HTML, CSS, and JavaScript** frontend. It lets administrators manage
courses and learners, enroll learners in available courses, review
enrollment records, and cancel enrollments.

## Features

### Course management

-   List all courses and retrieve a course by its course code.
-   List courses with available seats.
-   Create, update, and delete courses.
-   Validate course details, including fee, seat count, duration, and
    unique course codes.
-   Prevent deleting a course that has enrollment history.

### Learner management

-   List and retrieve learners.
-   Create, update, and delete learner records.
-   Validate learner IDs, names, and email addresses.
-   Prevent duplicate learner IDs and email addresses.

### Enrollment management

-   Enroll a learner in a course.
-   Check that the learner and course exist and that seats are
    available.
-   Prevent duplicate active enrollments for the same learner and
    course.
-   Apply a **10% discount** to the course fee and store the base fee,
    discount, and final fee.
-   Cancel an active enrollment and update the course's enrolled-seat
    count.
-   Display enrollment status and related learner/course details.

### Browser interface

-   Dashboard counters for courses, learners, and enrollments.
-   Forms to add courses and learners and enroll learners.
-   Tables displaying courses, learners, and enrollments.
-   Refresh data and cancel active enrollments from the page.
-   Responsive layout for desktop and smaller screens.

## Technology Stack

-   **Backend:** C#, ASP.NET Core Web API (`net10.0`)
-   **Data access:** Entity Framework Core
-   **Database:** Microsoft SQL Server / LocalDB
-   **API documentation:** Swagger / OpenAPI
-   **Frontend:** HTML5, CSS3, vanilla JavaScript Fetch API

## Project Structure

``` text
Day14/
├── CourseManagementWeb/
│   ├── CourseManagement.Api/
│   │   ├── Controllers/
│   │   │   ├── CoursesController.cs
│   │   │   ├── LearnersController.cs
│   │   │   └── EnrollmentsController.cs
│   │   ├── Data/
│   │   │   └── ApplicationDbContext.cs
│   │   ├── DTOs/
│   │   │   ├── CreateCourseDto.cs
│   │   │   ├── CreateLearnerDto.cs
│   │   │   └── CreateEnrollmentDto.cs
│   │   ├── Migrations/
│   │   ├── Models/
│   │   │   ├── Course.cs
│   │   │   ├── Learner.cs
│   │   │   ├── Enrollment.cs
│   │   │   └── EnrollmentStatus.cs
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── CourseManagement.Api.csproj
│   └── frontend/
│       ├── index.html
│       ├── style.css
│       └── app.js
└── DemoIntegration/
    ├── ApiCheck.html
    ├── ApiCheck.css
    └── ApiCheck.js
```

`DemoIntegration` is a separate browser-based demonstration of CRUD
requests against the public `https://api.restful-api.dev/objects`
endpoint. It is separate from the Course Management API.

## Prerequisites

Install or prepare the following:

-   .NET 10 SDK
-   SQL Server LocalDB (or a SQL Server instance)
-   Visual Studio 2022 with suitable .NET tooling, or another
    editor/terminal
-   A modern browser

The API project targets `net10.0`; use a compatible .NET SDK.

## Configuration

The default connection string in `CourseManagement.Api/appsettings.json`
uses SQL Server LocalDB:

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

If you use a different SQL Server instance, update
`ConnectionStrings:DefaultConnection` to match your environment. Do not
commit production credentials or secrets to source control.

The frontend currently sends API requests to:

``` javascript
const API_URL = "https://localhost:7201/api";
```

The backend launch settings define HTTPS at `https://localhost:7201` and
HTTP at `http://localhost:5202`. If your local ports differ, update
`API_URL` in `frontend/app.js` to match the running API.

## Run the Application

### 1. Set up the database

Open a terminal in the API project directory:

``` bash
cd Day14/CourseManagementWeb/CourseManagement.Api
```

Ensure the connection string points to a working SQL Server instance,
then apply the existing Entity Framework Core migrations:

``` bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

If `dotnet-ef` is already installed, update it as needed rather than
installing it again. The project includes an initial migration.

### 2. Start the API

From `Day14/CourseManagementWeb/CourseManagement.Api`, run:

``` bash
dotnet restore
dotnet run
```

With the included launch settings, the API is configured for:

-   HTTPS: `https://localhost:7201`
-   HTTP: `http://localhost:5202`

When running in Development mode, Swagger UI is enabled. Open:

``` text
https://localhost:7201/swagger
```

If the browser reports a local HTTPS certificate warning, trust the
ASP.NET Core development certificate using the .NET CLI and restart the
API:

``` bash
dotnet dev-certs https --trust
```

### 3. Open the frontend

Open `Day14/CourseManagementWeb/frontend/index.html` in a browser, or
serve the `frontend` folder through a local static web server. Keep the
API running while using the page.

The frontend is configured for the API's HTTPS URL. If browser requests
fail, verify the API URL, HTTPS certificate, and CORS settings. Serving
the frontend over a local HTTP server is generally more reliable than
opening it as a `file://` page.

## API Endpoints

The API uses the `/api` prefix.

### Courses --- `/api/courses`

  --------------------------------------------------------------------------
  Method                  Endpoint                   Purpose
  ----------------------- -------------------------- -----------------------
  GET                     `/api/courses`             List courses

  GET                     `/api/courses/available`   List courses with
                                                     available seats

  GET                     `/api/courses/{code}`      Get a course by code

  POST                    `/api/courses`             Create a course

  PUT                     `/api/courses/{code}`      Update a course

  DELETE                  `/api/courses/{code}`      Delete a course when it
                                                     has no enrollment
                                                     history
  --------------------------------------------------------------------------

Example course payload:

``` json
{
  "courseCode": "CS101",
  "courseTitle": "Introduction to Programming",
  "fee": 15000,
  "totalSeats": 30,
  "durationHours": 40
}
```

### Learners --- `/api/learners`

  -----------------------------------------------------------------------
  Method                  Endpoint                Purpose
  ----------------------- ----------------------- -----------------------
  GET                     `/api/learners`         List learners

  GET                     `/api/learners/{id}`    Get a learner by ID

  POST                    `/api/learners`         Create a learner

  PUT                     `/api/learners/{id}`    Update a learner

  DELETE                  `/api/learners/{id}`    Delete a learner,
                                                  subject to enrollment
                                                  constraints
  -----------------------------------------------------------------------

Example learner payload:

``` json
{
  "learnerId": "L001",
  "learnerName": "Alex Morgan",
  "email": "alex@example.com"
}
```

### Enrollments --- `/api/enrollments`

  Method   Endpoint                         Purpose
  -------- -------------------------------- ------------------------------
  GET      `/api/enrollments`               List enrollment records
  GET      `/api/enrollments/{id}`          Get an enrollment by ID
  POST     `/api/enrollments`               Enroll a learner in a course
  PUT      `/api/enrollments/{id}/cancel`   Cancel an active enrollment

Example enrollment payload:

``` json
{
  "learnerId": "L001",
  "courseCode": "CS101"
}
```

The API validates referenced learners and courses, seat availability,
and duplicate active enrollments. Enrollment creation applies the 10%
discount in the backend.

## Data Model

-   **Course:** course code, title, fee, total seats, enrolled count,
    and duration.
-   **Learner:** learner ID, name, and email.
-   **Enrollment:** generated enrollment ID, learner and course
    references, date, base fee, discount amount, final fee, and status
    (`Active` or `Cancelled`).

Entity Framework Core maps these entities to SQL Server. The database
context configures primary keys, field lengths, monetary precision,
relationships, and constraints.

## DemoIntegration: REST API Practice

The `Day14/DemoIntegration` folder contains a standalone page for
practicing REST API operations with
`https://api.restful-api.dev/objects`:

-   Load all objects
-   Get an object by ID
-   Add an object using `POST`
-   Update an object using `PUT`
-   Delete an object using `DELETE`

Open `ApiCheck.html` in a browser with an internet connection. This demo
calls a public service and is not connected to the SQL Server database
or the Course Management backend.

## Troubleshooting

-   **Database connection failure:** Check that SQL Server LocalDB or
    your chosen SQL Server instance is installed and running, and verify
    the connection string.
-   **Missing tables:** Run `dotnet ef database update` from the API
    project directory.
-   **API connection failure:** Confirm the backend is running and
    `API_URL` in `frontend/app.js` matches its HTTPS address.
-   **HTTPS/certificate errors:** Run `dotnet dev-certs https --trust`
    and restart the API.
-   **CORS errors:** Ensure the frontend requests the expected API
    origin and that the configured CORS policy is active.
-   **No data in the page:** Use Swagger to check the API endpoints and
    confirm the database is reachable. The repository does not visibly
    include a dedicated seed-data script, so the database may initially
    be empty.
-   **DemoIntegration does not load data:** Confirm internet access and
    that the public API endpoint is reachable.

## Notes

-   The API is configured with `AllowAnyOrigin`, `AllowAnyHeader`, and
    `AllowAnyMethod` for its frontend CORS policy. Review and restrict
    this policy before deploying to production.
-   The default database configuration is intended for local
    development; configure a secure production connection separately.
-   The API project targets .NET 10, so ensure your environment supports
    that target framework.

## License

No license file was found in the provided project folder. Add a license
file if you intend to distribute or reuse this project publicly.
