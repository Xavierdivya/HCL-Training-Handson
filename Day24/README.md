# Movie Ticket Booking System – Angular + ASP.NET Core

## Project Overview

The Movie Ticket Booking System is a full-stack web application that allows customers to browse movies, view theatres and shows, select seats, and book movie tickets.

The backend is developed using **ASP.NET Core Web API (.NET 10)** with Entity Framework Core, SQL Server, ASP.NET Core Identity, and JWT Authentication.

The frontend is developed using **Angular**, TypeScript, HTML, and CSS.

The application supports two roles:

- **Admin** – Manages movies, theatres, screens, seats, and shows.
- **Customer** – Views movies, selects shows and seats, and manages bookings.

The application also implements authentication and authorization diagnostic middleware to monitor request authentication, endpoint metadata, and authorization responses during development.

## Technologies Used

### Backend
- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core
- SQL Server Express
- ASP.NET Core Identity
- JWT Authentication
- Role-Based Authorization
- Resource-Based Authorization
- Repository Pattern
- Service Layer Architecture
- DTOs
- Custom ASP.NET Core Middleware

### Frontend
- Angular
- TypeScript
- HTML5
- CSS3
- Angular Router
- Angular HttpClient
- RxJS
- Reactive Forms
- Angular Route Guards
- HTTP Interceptors

### Development Tools
- Visual Studio
- Visual Studio Code
- SQL Server Management Studio
- Postman
- Git and GitHub
- Node.js and npm
- Angular CLI

## Project Architecture

The application follows a frontend-backend architecture.

```text
MovieTicketBooking/
│
├── Backend/
│   └── MovieTicketBooking/
│       ├── Controllers/
│       ├── Authorization/
│       ├── DTOs/
│       ├── Models/
│       ├── Middleware/
│       │   ├── AuthenticationDiagnosticsMiddleware.cs
│       │   ├── AuthorizationDiagnosticsMiddleware.cs
│       │   └── EndpointMetadataMiddleware.cs
│       ├── Services/
│       │   ├── Interfaces/
│       │   └── Implementations/
│       ├── Repositories/
│       │   ├── Interfaces/
│       │   └── Implementations/
│       ├── Data/
│       ├── Program.cs
│       └── appsettings.json
│
└── Frontend/
    └── movie-ticket-frontend/
        └── src/
            └── app/
                ├── components/
                ├── services/
                ├── guards/
                ├── interceptors/
                └── app.routes.ts
```

*Note: The actual folder names and locations may differ slightly from this logical architecture.*

## Main Features

### 1. Authentication
- Customer registration.
- User login.
- JWT token generation.
- Password validation.
- Secure API access.

### 2. Role-Based Authorization

**Admin permissions:**
- Add, update, and delete movies.
- Manage theatres.
- Manage screens and seats.
- Create and manage shows.

**Customer permissions:**
- View available movies.
- View theatres and shows.
- Select seats.
- Create bookings.
- View booking details.
- Cancel bookings.

### 3. Resource-Based Authorization
- Verify whether a customer owns a particular booking.
- Allow Admin users to access authorized booking resources.
- Prevent customers from accessing or cancelling other customers' bookings.
- Return `403 Forbidden` when a user is authenticated but does not have permission.

### 4. Middleware Diagnostics

Three custom middleware classes are implemented to understand and monitor the ASP.NET Core request pipeline.

**AuthenticationDiagnosticsMiddleware**
- Logs whether the user is authenticated.
- Logs the authentication type.
- Displays the claims associated with the authenticated user.

**AuthorizationDiagnosticsMiddleware**
- Logs the authorization process.
- Records the final HTTP response status code.

**EndpointMetadataMiddleware**
- Logs the HTTP request method and path.
- Displays the selected endpoint.
- Checks whether the endpoint allows anonymous access.
- Displays the authorization metadata count.

These diagnostic middleware components are registered only in the Development environment.

### 5. Movie Management
- Display all movies.
- View movie details.
- Search movies.
- Add new movies (Admin).
- Update movie details (Admin).
- Delete movies (Admin).

### 6. Theatre and Show Management
- View theatres.
- View screens.
- View available shows.
- Manage show schedules (Admin).

### 7. Seat Selection and Booking
- Display available seats.
- Select seats for a particular show.
- Calculate booking amount.
- Confirm bookings.
- Retrieve booking details.
- Cancel bookings.

## Angular Frontend Structure

The Angular application contains the following major areas:

| Folder | Responsibility |
|---|---|
| components | Login, registration, movies, booking, and dashboard pages |
| services | Communicate with backend API endpoints |
| guards | Protect authenticated and role-specific routes |
| interceptors | Attach JWT tokens to API requests |
| app.routes.ts | Configure frontend navigation |

### Important Angular Services

- `auth.service.ts` – Registration, login, and logout.
- `movie.service.ts` – Movie-related API requests.
- `show.service.ts` – Show-related API requests.
- `booking.service.ts` – Booking-related API requests.

## Backend API Integration

The Angular frontend communicates with the ASP.NET Core backend using HTTP requests.

**Backend Base URL**

```text
http://localhost:5156
```

### Example API Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/Auth/register` | Register customer |
| POST | `/api/Auth/login` | Login |
| GET | `/api/Movies` | Get all movies |
| GET | `/api/Movies/{id}` | Get movie by ID |
| POST | `/api/Movies` | Add movie (Admin) |
| PUT | `/api/Movies/{id}` | Update movie (Admin) |
| DELETE | `/api/Movies/{id}` | Delete movie (Admin) |
| GET | `/api/Shows` | Get shows |
| POST | `/api/Bookings` | Create booking |
| GET | `/api/Bookings/{id}` | Get booking details |
| PUT | `/api/Bookings/{id}/cancel` | Cancel booking |

*The endpoint list is illustrative. Refer to the actual backend controllers for the complete API contract.*

## JWT Authentication Flow

1. Customer enters email and password in the Angular login page.
2. Angular sends the login request to the backend.
3. ASP.NET Core Identity validates the credentials.
4. The backend generates a JWT token.
5. Angular receives and stores the token.
6. Angular HTTP Interceptor attaches the token to protected API requests.
7. ASP.NET Core validates the JWT.
8. Authentication middleware populates `HttpContext.User`.
9. Authorization middleware checks the user's permissions.
10. Resource-based authorization verifies access to the requested booking when required.

Example authorization header:

```http
Authorization: Bearer <JWT_TOKEN>
```

**Important:** Angular route guards improve frontend navigation and user experience. Actual security must always be enforced by backend authorization.

## Middleware Request Pipeline

The backend uses the following middleware execution order:

1. HTTPS Redirection
2. Endpoint Metadata Diagnostics Middleware (Development only)
3. Authentication Middleware
4. Authentication Diagnostics Middleware (Development only)
5. Authorization Diagnostics Middleware (Development only)
6. Authorization Middleware
7. Controller Endpoint Execution

### Middleware Execution Responsibilities

| Middleware | Responsibility |
|---|---|
| UseHttpsRedirection | Redirects HTTP requests to HTTPS |
| EndpointMetadataMiddleware | Logs endpoint and authorization metadata |
| UseAuthentication | Validates JWT and populates user identity |
| AuthenticationDiagnosticsMiddleware | Logs authentication status and claims |
| AuthorizationDiagnosticsMiddleware | Logs the final response status |
| UseAuthorization | Enforces authorization rules |
| MapControllers | Maps API controller endpoints |

## Development Setup

### Backend Setup

1. Open the backend project.
2. Configure the SQL Server connection.
3. Configure JWT settings and User Secrets.
4. Apply EF Core migrations if required.
5. Run the backend API.

Build command:

```bash
dotnet build -p:UseAppHost=false
```

Run command:

```bash
dotnet run -p:UseAppHost=false
```

Backend URL:

```text
http://localhost:5156
```

### Frontend Setup

Install Node.js (LTS) and Angular CLI.

Check installation:

```bash
node -v
npm -v
ng version
```

Install Angular CLI if required:

```bash
npm install -g @angular/cli
```

Navigate to the frontend project:

```bash
cd movie-ticket-frontend
```

Install dependencies:

```bash
npm install
```

Run the Angular application:

```bash
ng serve
```

Open the frontend:

```text
http://localhost:4200
```

## CORS Configuration

The ASP.NET Core backend must allow requests from the Angular development server.

Configure a CORS policy for:

```text
http://localhost:4200
```

Allow the required HTTP methods and headers, including the `Authorization` header.

Use an appropriately restricted origin configuration rather than allowing every origin in production.

## Testing

### Backend Testing – Postman

Test the following operations:

- Customer registration and login.
- Admin login.
- Movie CRUD operations.
- JWT authentication.
- Role-based authorization.
- Resource-based booking authorization.
- Booking creation and cancellation.
- Accessing another customer's booking.
- Authentication and authorization diagnostic logs.

### Authorization Test Cases

| Test Case | Expected Result |
|---|---|
| Customer accesses their own booking | Allowed |
| Customer cancels their own booking | Allowed |
| Customer attempts to cancel another customer's booking | 403 Forbidden |
| Request without a valid JWT | 401 Unauthorized |
| Admin accesses an authorized booking | Allowed |

### Middleware Testing

Verify the console logs for:

- Authentication status.
- Authentication type.
- User claims.
- Endpoint display name.
- AllowAnonymous metadata.
- Authorization metadata count.
- Final HTTP response status.

## Development Roadmap

| Step | Task | Status |
|---|---|---|
| 1 | Develop ASP.NET Core Web API | Completed |
| 2 | Implement Entity Framework Core and SQL Server | Completed |
| 3 | Implement DTOs and service/repository architecture | Completed |
| 4 | Implement Identity and JWT authentication | Completed |
| 5 | Implement role-based authorization | Implemented |
| 6 | Implement resource-based authorization | Implemented and tested |
| 7 | Implement authentication diagnostics middleware | Completed |
| 8 | Implement authorization diagnostics middleware | Completed |
| 9 | Implement endpoint metadata middleware | Completed |
| 10 | Develop Angular frontend | In progress |
| 11 | Configure Angular API integration | Pending |
| 12 | Implement login and registration | Pending |
| 13 | Implement movie and show pages | Pending |
| 14 | Implement seat selection and booking | Pending |
| 15 | Implement Admin dashboard | Pending |
| 16 | Complete full-stack integration testing | Pending |

## Learning Outcomes

Through this project, the following concepts are practised:

- ASP.NET Core Web API development.
- Layered architecture and separation of concerns.
- Interfaces and implementations.
- Dependency Injection.
- Entity Framework Core and database relationships.
- DTO-based API communication.
- ASP.NET Core Identity.
- JWT authentication.
- Role-based authorization.
- Resource-based authorization.
- Custom ASP.NET Core middleware.
- HTTP request pipeline and middleware execution order.
- Endpoint metadata.
- Angular components and services.
- Angular routing and route guards.
- HTTP communication between Angular and ASP.NET Core.
- Full-stack application development.
- Git branching and conventional commits.

## Conclusion

The Movie Ticket Booking System demonstrates full-stack application development using Angular and ASP.NET Core Web API.

The backend handles business logic, database operations, authentication, authorization, and middleware diagnostics, while Angular provides the user interface and communicates with the backend through REST API endpoints.

This project provides practical experience in building secure, maintainable, and scalable web applications using modern .NET and Angular technologies.