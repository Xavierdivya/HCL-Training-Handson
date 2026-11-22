
using Enrollora.Api.Models;
using Enrollora.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Enrollora.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await db.Database.MigrateAsync();

        if (await db.AdminUsers.AnyAsync())
            return;

        var seed = configuration.GetSection("SeedAdmin");

        var email = seed["Email"];
        var password = seed["Password"];
        var name = seed["Name"];

        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidOperationException(
                "Seed administrator email is missing.");

        if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
            throw new InvalidOperationException(
                "Seed administrator password must contain at least 12 characters.");

        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException(
                "Seed administrator name is missing.");

        var hasher = scope.ServiceProvider
            .GetRequiredService<IPasswordHasher>();

        db.AdminUsers.Add(new AdminUser
        {
            Name = name.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = hasher.Hash(password)
        });

        await db.SaveChangesAsync();
    }
}