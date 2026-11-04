# Day22 – Movie Ticket Booking System (Angular + ASP.NET Core)

## Project Overview

The Movie Ticket Booking System is a full-stack web application designed to allow customers to browse movies, view theatres and shows, select seats, and book movie tickets.

The backend is developed using **ASP.NET Core Web API (.NET 10)** with **Entity Framework Core**, **SQL Server**, **ASP.NET Core Identity**, and **JWT Authentication**.

The frontend will be developed using **Angular**, **TypeScript**, HTML, and CSS.

The application supports role-based authorization with two roles:

- **Admin** – Manages movies, theatres, screens, seats, and shows.
- **Customer** – Views movies, selects shows and seats, and manages bookings.

## Technologies Used

### Backend

- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core
- SQL Server Express
- ASP.NET Core Identity
- JWT Authentication
- Role-Based Authorization
- Repository Pattern
- Service Layer Architecture
- DTOs

### Frontend

- Angular
- TypeScript
- HTML5
- CSS3
- Bootstrap (optional)
- Angular Router
- Angular HttpClient
- RxJS
- Reactive Forms
- Angular Route Guards
- HTTP Interceptors

### Development Tools

- Visual Studio / Visual Studio Code
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
│       ├── DTOs/
│       ├── Models/
│       ├── Services/
│       │   ├── Interfaces/
│       │   └── Implementations/
│       ├── Repositories/
│       ├── Data/
│       └── Program.cs
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

*Note: The frontend structure represents the planned Angular application. The existing backend remains in the MovieTicketBooking project.*

## Main Features

### 1. Authentication

- Customer registration
- User login
- JWT token generation
- Password validation
- Secure API access

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

### 3. Movie Management

- Display all movies.
- View movie details.
- Search movies.
- Add new movies (Admin).
- Update movie details (Admin).
- Delete movies (Admin).

### 4. Theatre and Show Management

- View theatres.
- View screens.
- View available shows.
- Manage show schedules (Admin).

### 5. Seat Selection and Booking

- Display available seats.
- Select seats for a particular show.
- Calculate booking amount.
- Confirm bookings.
- Retrieve booking details.
- Cancel bookings.

## Angular Frontend Structure

The Angular application will contain the following modules and features:

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

The Angular frontend will communicate with the ASP.NET Core backend through HTTP requests.

**Backend Base URL:**

```text
http://localhost:5156
```

### Example API Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| POST | /api/Auth/register | Register customer |
| POST | /api/Auth/login | Login |
| GET | /api/Movies | Get all movies |
| GET | /api/Movies/{id} | Get movie by ID |
| POST | /api/Movies | Add movie (Admin) |
| PUT | /api/Movies/{id} | Update movie (Admin) |
| DELETE | /api/Movies/{id} | Delete movie (Admin) |
| GET | /api/Shows | Get shows |
| POST | /api/Bookings | Create booking |
| GET | /api/Bookings/{id} | Get booking details |
| PUT | /api/Bookings/{id}/cancel | Cancel booking |

*The endpoint list is illustrative. Refer to the actual backend controllers for the complete API contract.*

## JWT Authentication Flow

1. Customer enters email and password in the Angular login page.
2. Angular sends the login request to the backend.
3. ASP.NET Core Identity validates the credentials.
4. The backend generates a JWT token.
5. Angular receives and stores the token.
6. Angular HTTP Interceptor attaches the token to protected API requests.
7. ASP.NET Core validates the JWT.
8. Role-based authorization checks whether the user has permission to access the requested endpoint.

Example authorization header:

```http
Authorization: Bearer <JWT_TOKEN>
```

**Important:** Angular route guards improve frontend navigation and user experience. Actual security must always be enforced by backend authorization.

## Development Setup

### Backend Setup

1. Open the backend project.
2. Configure the SQL Server connection.
3. Configure JWT settings and User Secrets.
4. Apply EF Core migrations if required.
5. Run the backend API.

Build command:

```powershell
dotnet build -p:UseAppHost=false
```

Run command:

```powershell
dotnet run -p:UseAppHost=false
```

Backend URL:

```text
http://localhost:5156
```

### Frontend Setup

Install Node.js (LTS) and Angular CLI.

Check installation:

```powershell
node -v
npm -v
ng version
```

Install Angular CLI if required:

```powershell
npm install -g @angular/cli
```

Create the Angular application:

```powershell
cd /d D:\HCL\Day22
ng new movie-ticket-frontend
```

Navigate to the frontend:

```powershell
cd movie-ticket-frontend
```

Run the Angular application:

```powershell
ng serve
```

Open the frontend:

```text
http://localhost:4200
```

## CORS Configuration

The ASP.NET Core backend must allow requests from the Angular development server.

Configure a CORS policy in `Program.cs` for:

```text
http://localhost:4200
```

Allow the required HTTP methods and headers. For JWT authentication, allow the `Authorization` header.

Use an appropriate restricted origin configuration rather than allowing every origin in production.

## Testing

### Backend Testing

Use Postman to test:

- Customer registration and login.
- Admin login.
- Movie CRUD operations.
- JWT authentication.
- Role-based authorization.
- Booking operations.

### Frontend Testing

Test:

- Customer registration and login.
- Movie listing and details.
- Show selection.
- Seat selection.
- Booking creation.
- Booking cancellation.
- Admin dashboard access.
- Customer and Admin route permissions.

## Development Roadmap

| Step | Task | Status |
|---|---|---|
| 1 | Develop ASP.NET Core Web API | Completed |
| 2 | Implement Entity Framework Core and SQL Server | Completed |
| 3 | Implement DTOs and service/repository architecture | Completed |
| 4 | Implement Identity and JWT authentication | Completed |
| 5 | Implement role-based authorization | Implemented; integration testing ongoing |
| 6 | Install Node.js and Angular CLI | Pending |
| 7 | Create Angular frontend | Pending |
| 8 | Configure Angular API integration | Pending |
| 9 | Implement login and registration | Pending |
| 10 | Implement movie and show pages | Pending |
| 11 | Implement seat selection and booking | Pending |
| 12 | Implement Admin dashboard | Pending |
| 13 | Complete full-stack integration testing | Pending |

## Learning Outcomes

Through this project, the following concepts will be practised:

- ASP.NET Core Web API development.
- Layered architecture and separation of concerns.
- Interfaces and implementations.
- Dependency Injection.
- Entity Framework Core and database relationships.
- DTO-based API communication.
- ASP.NET Core Identity.
- JWT authentication.
- Role-based authorization.
- Angular components and services.
- Angular routing and route guards.
- HTTP communication between Angular and ASP.NET Core.
- Full-stack application development.
- Git branching and conventional commits.

## Conclusion

This project demonstrates how to develop a full-stack Movie Ticket Booking application using Angular and ASP.NET Core Web API.

The backend manages business logic, authentication, authorization, and database operations, while Angular provides the user interface and communicates with the backend through REST API endpoints.
