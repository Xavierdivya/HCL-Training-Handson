
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FoodMenuManagement.Api.DTOs.Auth;
using FoodMenuManagement.Api.Entities;
using FoodMenuManagement.Api.Repositories.Interfaces;
using FoodMenuManagement.Api.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FoodMenuManagement.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;

        private const int Iterations = 210000;

        public AuthService(
            IAuthRepository repository,
            IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        // REGISTER
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            string email = dto.Email.Trim().ToLowerInvariant();

            var existingUser = await _repository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                throw new ArgumentException("Email is already registered.");
            }

            var user = new AppUser
            {
                FullName = dto.FullName.Trim(),
                Email = email,
                PasswordHash = HashPassword(dto.Password),
                Role = UserRoles.Customer,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            return GenerateToken(user);
        }

        // LOGIN
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            string email = dto.Email.Trim().ToLowerInvariant();

            var user = await _repository.GetByEmailAsync(email);

            if (user == null || !VerifyPassword(
                dto.Password,
                user.PasswordHash))
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            return GenerateToken(user);
        }

        // PASSWORD HASHING USING PBKDF2
        private static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                32);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        // PASSWORD VERIFICATION
        private static bool VerifyPassword(
            string password,
            string storedHash)
        {
            try
            {
                var parts = storedHash.Split('.');

                if (parts.Length != 2)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] expectedHash = Convert.FromBase64String(parts[1]);

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    Iterations,
                    HashAlgorithmName.SHA256,
                    32);

                return CryptographicOperations.FixedTimeEquals(
                    actualHash,
                    expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // JWT TOKEN GENERATION
        private AuthResponseDto GenerateToken(AppUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            string issuer = jwtSettings["Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is missing.");

            string audience = jwtSettings["Audience"]
                ?? throw new InvalidOperationException("JWT Audience is missing.");

            string secretKey = jwtSettings["Key"]
                ?? throw new InvalidOperationException("JWT Key is missing.");

            int expiryMinutes = int.TryParse(
                jwtSettings["ExpiryMinutes"],
                out int minutes) ? minutes : 60;

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            DateTime expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

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
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
