
using System.Text;
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Data.Seed;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Repositories.Implementations;
using CampusPlacementManagement.Services.Interfaces;
using CampusPlacementManagement.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<
    IPlacementOfficerRepository,
    PlacementOfficerRepository>();

builder.Services.AddScoped<
    IPlacementOfficerService,
    PlacementOfficerService>();

// Add controllers.
builder.Services.AddControllers();

// Configure SQL Server.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure ASP.NET Core Identity.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure JWT authentication.
var jwtKeyValue = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKeyValue) ||
    string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException(
        "JWT configuration is missing.");
}

byte[] jwtKeyBytes;

try
{
    jwtKeyBytes = Convert.FromBase64String(jwtKeyValue);
}
catch (FormatException)
{
    throw new InvalidOperationException(
        "JWT signing key must be a valid Base64 value.");
}

if (jwtKeyBytes.Length < 32)
{
    throw new InvalidOperationException(
        "JWT signing key must contain at least 256 bits.");
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultForbidScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(jwtKeyBytes),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),

                NameClaimType =
                    System.Security.Claims.ClaimTypes.Name,

                RoleClaimType =
                    System.Security.Claims.ClaimTypes.Role
            };
    });

// Register repositories.
builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();

// Register services.
builder.Services.AddScoped<ICollegeService, CollegeService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// OpenAPI.
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed Identity roles and SuperAdmin.
await IdentitySeeder.SeedAsync(
    app.Services,
    app.Configuration);

// Configure HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();