# Day23 Movie Ticket Booking – Angular Frontend

This is an Angular standalone frontend starter for the existing ASP.NET Core MovieTicketBooking API.

## What is included
- Customer registration and login
- JWT storage and HTTP interceptor
- Authentication and Admin route guards
- Movie listing and movie details
- Shows listing
- Seat selection and booking creation
- Customer booking lookup/list
- Admin dashboard with basic movie management
- Configurable backend API URL

## Important backend assumptions
The frontend is configured for `http://localhost:5156` and uses these routes:
- `POST /api/Auth/register`
- `POST /api/Auth/login`
- `GET /api/Movies`
- `GET /api/Movies/{id}`
- `POST /api/Movies`
- `PUT /api/Movies/{id}`
- `DELETE /api/Movies/{id}`
- `GET /api/Shows`
- `GET /api/Bookings`
- `POST /api/Bookings`
- `GET /api/Bookings/{id}`
- `PUT /api/Bookings/{id}/cancel`

Your backend may not expose every listed route or may use different DTO property names. In particular, check the actual `ShowsController`, `BookingsController`, `SeatsController`, and response DTOs. Update the corresponding service methods and TypeScript interfaces if needed. The booking form uses the known `CreateBookingDto` shape:
`ShowId`, `SeatIds`, `CustomerName`, `CustomerEmail`, `CustomerPhone`.

## Add this frontend to your generated Angular project

1. Stop `ng serve` with `Ctrl+C`.
2. Extract this ZIP.
3. Copy the ZIP's `src` folder into:
   `D:\HCL\Day23\movie-ticket-frontend\src`
   Choose **merge/replace** when prompted. Keep your existing `package.json`, `angular.json`, and `tsconfig*.json`.
4. Open a terminal in `D:\HCL\Day23\movie-ticket-frontend`.
5. Run:
   ```powershell
   npm install
   ng build
   ng serve
   ```
6. Open `http://localhost:4200`.

## Backend requirements

Run the ASP.NET Core API at `http://localhost:5156`.

Configure CORS in backend `Program.cs` (before `builder.Build()`):

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

After `var app = builder.Build();`, add this before mapping controllers:

```csharp
app.UseCors("AngularFrontend");
```

If you already have a CORS policy, modify it rather than registering duplicate policies. Keep CORS origins restricted in production.

## API URL
Edit `src/environments/environment.ts` if your backend runs on another URL.

## Authentication notes
- Login expects the backend response to contain `token` or `accessToken`. If your response uses another property, update `AuthService.login()`.
- The token is stored in `localStorage` for this learning project. For production, consider a more secure token/session design.
- The Admin UI guard is only a user-experience feature. Backend `[Authorize]` and role authorization remain the actual security boundary.
- Registration currently follows your backend behavior where new registrations receive the Customer role.

## Troubleshooting
- **CORS error:** verify backend CORS setup and restart the API.
- **Connection refused:** start the backend and verify `http://localhost:5156`.
- **401:** login again; token may be missing or expired.
- **403:** the logged-in user lacks the required backend role.
- **404:** compare the endpoint with your actual controller route.
- **Build errors after merge:** check Angular version compatibility and share the full error output.
