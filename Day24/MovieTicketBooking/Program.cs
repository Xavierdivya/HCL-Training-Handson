using MovieTicketBooking.Middleware;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using MovieTicketBooking.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBooking.Data;
using MovieTicketBooking.Data.Seed;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Implementations;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Implementations;
using MovieTicketBooking.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Configure SQL Server database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure ASP.NET Core Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register repository implementations using Dependency Injection
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<
    IAuthorizationHandler,
    BookingAuthorizationHandler>();

// Register application services
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ITheatreService, TheatreService>();
builder.Services.AddScoped<IScreenService, ScreenService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IShowService, ShowService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookingService, BookingService>();

// Configure authorization policies and handlers
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BookingAccess", policy =>
    {
        policy.RequireAuthenticatedUser();

        policy.AddRequirements(
            new OperationAuthorizationRequirement
            {
                Name = BookingOperations.Access
            });
    });
});

builder.Services.AddScoped<
    IAuthorizationHandler,
    BookingAuthorizationHandler>();

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key is missing.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("JWT Issuer is missing.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("JWT Audience is missing.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })

.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)),

        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role,

        ClockSkew = TimeSpan.Zero
    };
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();

    // Create Admin role if it does not exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(
            new IdentityRole("Admin"));
    }

    // Find existing Admin account
    var adminUser = await userManager.FindByEmailAsync(
        "Admin@gmail.com");

    if (adminUser == null)
    {
        throw new Exception("Admin user not found.");
    }

    // Remove Customer role
    if (await userManager.IsInRoleAsync(adminUser, "Customer"))
    {
        await userManager.RemoveFromRoleAsync(
            adminUser, "Customer");
    }

    // Assign Admin role
    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(
            adminUser, "Admin");
    }
}

// Create Admin and Customer roles and provision Admin account.
await IdentitySeeder.SeedAsync(app.Services);

// Configure the HTTP request pipeline.

// OpenAPI configuration
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 1. HTTPS Redirection
app.UseHttpsRedirection();

// 2. Routing
// Explicit routing middleware is optional with WebApplication,
// because routing is automatically added.

// 3. Endpoint Metadata Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<EndpointMetadataMiddleware>();
}

// 4. Authentication
// Validates JWT and populates HttpContext.User
app.UseAuthentication();

// 5. Authentication Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthenticationDiagnosticsMiddleware>();
}

// 6. Authorization Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthorizationDiagnosticsMiddleware>();
}

// 7. Authorization
// Checks endpoint access permissions
app.UseAuthorization();

// 8. Map Controllers
app.MapControllers();

// 9. Run Application
app.Run();
