
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(
        RegisterRequestDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingUser != null)
            throw new InvalidOperationException("Email is already registered.");

        var customerRole = "Customer";

        if (!await _roleManager.RoleExistsAsync(customerRole))
        {
            var roleResult = await _roleManager.CreateAsync(
                new IdentityRole(customerRole));

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ",
                    roleResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }
        }

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var createResult = await _userManager.CreateAsync(
            user, dto.Password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ",
                createResult.Errors.Select(e => e.Description));

            throw new InvalidOperationException(errors);
        }

        var addRoleResult = await _userManager.AddToRoleAsync(
            user, customerRole);

        if (!addRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            var errors = string.Join(", ",
                addRoleResult.Errors.Select(e => e.Description));

            throw new InvalidOperationException(errors);
        }

        return await GenerateTokenAsync(user);
    }

    public async Task<AuthResponseDto?> LoginAsync(
        LoginRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return null;

        var passwordValid = await _userManager.CheckPasswordAsync(
            user, dto.Password);

        if (!passwordValid)
            return null;

        return await GenerateTokenAsync(user);
    }

    private async Task<AuthResponseDto> GenerateTokenAsync(
        ApplicationUser user)
    {
        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key is missing.");

        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer is missing.");

        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT Audience is missing.");

        var expiryMinutes = _configuration.GetValue<int>(
            "Jwt:ExpiryMinutes", 60);

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            securityKey,
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
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            AccessToken = new JwtSecurityTokenHandler()
                .WriteToken(token),
            ExpiresAt = expiresAt,
            Roles = roles.ToList()
        };
    }
}
