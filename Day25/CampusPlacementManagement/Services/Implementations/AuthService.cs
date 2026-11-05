using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Auth;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CampusPlacementManagement.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        // Register a college and its representative together.
        public async Task<UserResponseDto>
            RegisterCollegeRepresentativeAsync(
                CollegeRepresentativeRegistrationDto registrationDto)
        {
            var collegeCode = registrationDto.CollegeCode
                .Trim()
                .ToUpperInvariant();

            var email = registrationDto.Email.Trim();

            // Check duplicate college code.
            var existingCollege = await _context.Colleges
                .AnyAsync(c => c.CollegeCode == collegeCode);

            if (existingCollege)
            {
                throw new InvalidOperationException(
                    "A college with this code already exists.");
            }

            // Check duplicate representative email.
            var existingUser = await _userManager
                .FindByEmailAsync(email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "An account with this email already exists.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var college = new College
                {
                    CollegeName = registrationDto.CollegeName.Trim(),
                    CollegeCode = collegeCode,
                    UniversityName =
                        registrationDto.UniversityName?.Trim(),
                    OfficialEmail =
                        registrationDto.OfficialEmail.Trim(),
                    PhoneNumber = registrationDto.PhoneNumber.Trim(),
                    Address = registrationDto.Address.Trim(),
                    Website = registrationDto.Website?.Trim(),
                    Status = CollegeStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Colleges.Add(college);

                await _context.SaveChangesAsync();

                var representative = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = registrationDto.PhoneNumber.Trim(),
                    FullName = registrationDto.FullName.Trim(),
                    CollegeId = college.CollegeId,
                    AccountStatus = AccountStatus.Pending,
                    CreatedAt = DateTime.UtcNow,

                    // No email-confirmation workflow is implemented yet.
                    // This will need revisiting when email verification is added.
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(
                    representative,
                    registrationDto.Password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(errors);
                }

                var roleResult = await _userManager.AddToRoleAsync(
                    representative,
                    AppRoles.PlacementOfficer);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(errors);
                }

                // Create the PlacementOfficer profile.
                var placementOfficer = new PlacementOfficer
                {
                    UserId = representative.Id,
                    CollegeId = college.CollegeId,
                    Designation = string.IsNullOrWhiteSpace(registrationDto.Designation)
                        ? "Placement Officer"
                        : registrationDto.Designation.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                _context.PlacementOfficers.Add(placementOfficer);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await MapToUserResponseAsync(representative);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Authenticate a user and generate a JWT.
        public async Task<LoginResponseDto> LoginAsync(
            LoginRequestDto loginDto)
        {
            var email = loginDto.Email.Trim();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            var passwordValid = await _userManager
                .CheckPasswordAsync(user, loginDto.Password);

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            // Only approved accounts can log in.
            if (user.AccountStatus != AccountStatus.Approved)
            {
                throw new UnauthorizedAccessException(
                    "Your account has not been approved or is not active.");
            }

            // College representatives must belong to an approved college.
            if (user.CollegeId.HasValue)
            {
                var college = await _context.Colleges
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        c => c.CollegeId == user.CollegeId.Value);

                if (college == null ||
                    college.Status != CollegeStatus.Approved)
                {
                    throw new UnauthorizedAccessException(
                        "Your college is not approved.");
                }
            }

            var roles = await _userManager.GetRolesAsync(user);

            var expiresAt = DateTime.UtcNow.AddMinutes(
                GetJwtExpiryMinutes());

            var claims = new List<Claim>
            {
                new(
                    ClaimTypes.NameIdentifier,
                    user.Id),

                new(
                    ClaimTypes.Name,
                    user.FullName),

                new(
                    ClaimTypes.Email,
                    user.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (user.CollegeId.HasValue)
            {
                claims.Add(
                    new Claim(
                        "CollegeId",
                        user.CollegeId.Value.ToString()));
            }

            var key = GetJwtSigningKey();

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return new LoginResponseDto
            {
                Token = tokenString,
                ExpiresAt = expiresAt,
                User = await MapToUserResponseAsync(user)
            };
        }

        // Retrieve the authenticated user's profile.
        public async Task<UserResponseDto?> GetCurrentUserAsync(
            string userId)
        {
            var user = await _context.Users
                .Include(u => u.College)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                AccountStatus = user.AccountStatus.ToString(),
                CollegeId = user.CollegeId,
                CollegeName = user.College?.CollegeName,
                Roles = roles.ToList()
            };
        }

        private async Task<UserResponseDto> MapToUserResponseAsync(
            ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var collegeName = user.CollegeId.HasValue
                ? await _context.Colleges
                    .Where(c => c.CollegeId == user.CollegeId.Value)
                    .Select(c => c.CollegeName)
                    .FirstOrDefaultAsync()
                : null;

            return new UserResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                AccountStatus = user.AccountStatus.ToString(),
                CollegeId = user.CollegeId,
                CollegeName = collegeName,
                Roles = roles.ToList()
            };
        }

        private int GetJwtExpiryMinutes()
        {
            var value = _configuration["Jwt:ExpiryMinutes"];

            if (!int.TryParse(value, out var minutes) || minutes <= 0)
            {
                throw new InvalidOperationException(
                    "Jwt:ExpiryMinutes configuration is invalid.");
            }

            return minutes;
        }

        private SymmetricSecurityKey GetJwtSigningKey()
        {
            var keyValue = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(keyValue))
            {
                throw new InvalidOperationException(
                    "JWT signing key is not configured.");
            }

            try
            {
                var keyBytes = Convert.FromBase64String(keyValue);

                if (keyBytes.Length < 32)
                {
                    throw new InvalidOperationException(
                        "JWT signing key must contain at least 256 bits.");
                }

                return new SymmetricSecurityKey(keyBytes);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException(
                    "JWT signing key must be a valid Base64 value.");
            }
        }
    }
}