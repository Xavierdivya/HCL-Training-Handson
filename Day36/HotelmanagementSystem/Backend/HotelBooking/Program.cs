using HotelBooking.Repositories;
using HotelBooking.Services;
using System;
using System.Security.Claims;
using System.Text;
using HotelBooking.Middleware;

using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Repositories.Implementations;
using HotelBooking.Repositories.Interfaces;
using HotelBooking.Services.Implementations;
using HotelBooking.Services.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1. Controllers
builder.Services.AddControllers();

// 2. SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. ASP.NET Core Identity
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// 4. JWT Authentication
var jwtKey = builder.Configuration["JwtSettings:SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey is missing.");

var jwtIssuer = builder.Configuration["JwtSettings:Issuer"]
    ?? throw new InvalidOperationException("JWT Issuer is missing.");

var jwtAudience = builder.Configuration["JwtSettings:Audience"]
    ?? throw new InvalidOperationException("JWT Audience is missing.");

if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException(
        "JWT SecretKey must contain at least 32 bytes.");
}

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

            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

// 5. Authorization
builder.Services.AddAuthorization();

// 6. CORS for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 7. Repository registrations
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IRoomInventoryRepository, RoomInventoryRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// 8. Service registrations
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IRoomInventoryService, RoomInventoryService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 9. OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();


app.UseMiddleware<GlobalExceptionMiddleware>();
// 10. Seed roles and initial Admin account
await DbSeeder.SeedAsync(app.Services, app.Configuration);

// 11. HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AngularPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();