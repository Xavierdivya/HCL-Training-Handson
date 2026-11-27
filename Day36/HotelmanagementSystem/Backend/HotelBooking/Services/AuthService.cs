using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using HotelBooking.DTOs.Auth;
using HotelBooking.Models;
using HotelBooking.Services.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HotelBooking.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(
        RegisterRequestDto request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            throw new InvalidOperationException(
                "Password and confirm password do not match.");
        }

        var email = request.Email.Trim();

        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        const string customerRole = "Customer";

        if (!await _roleManager.RoleExistsAsync(customerRole))
        {
            var roleResult = await _roleManager.CreateAsync(
                new IdentityRole(customerRole));

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        roleResult.Errors.Select(e => e.Description)));
            }
        }

        var user = new ApplicationUser
        {
            FullName = request.FullName.Trim(),
            Email = email,
            UserName = email,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await _userManager.CreateAsync(
            user, request.Password);

        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ",
                    createResult.Errors.Select(e => e.Description)));
        }

        var addRoleResult = await _userManager.AddToRoleAsync(
            user, customerRole);

        if (!addRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            throw new InvalidOperationException(
                string.Join(", ",
                    addRoleResult.Errors.Select(e => e.Description)));
        }

        return await GenerateTokenAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(
        LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Count == 0)
        {
            throw new UnauthorizedAccessException(
                "User does not have an assigned role.");
        }

        return await GenerateTokenAsync(user);
    }

    public async Task<CurrentUserResponseDto?> GetCurrentUserAsync(
        string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new CurrentUserResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            Role = roles.FirstOrDefault() ?? string.Empty
        };
    }

    private async Task<AuthResponseDto> GenerateTokenAsync(
        ApplicationUser user)
    {
        var key = _configuration["JwtSettings:SecretKey"]
            ?? throw new InvalidOperationException(
                "JWT SecretKey is missing.");

        var issuer = _configuration["JwtSettings:Issuer"]
            ?? throw new InvalidOperationException(
                "JWT Issuer is missing.");

        var audience = _configuration["JwtSettings:Audience"]
            ?? throw new InvalidOperationException(
                "JWT Audience is missing.");

        var expiryMinutes = int.TryParse(
            _configuration["JwtSettings:ExpiryMinutes"],
            out var configuredExpiry)
            ? configuredExpiry
            : 60;

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(
            roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            Role = roles.FirstOrDefault() ?? string.Empty
        };
    }
}