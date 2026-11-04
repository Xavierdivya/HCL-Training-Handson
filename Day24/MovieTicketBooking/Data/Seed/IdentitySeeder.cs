
using Microsoft.AspNetCore.Identity;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        var configuration = scope.ServiceProvider
            .GetRequiredService<IConfiguration>();

        // 1. Create roles if they do not exist.
        string[] roles = { "Admin", "Customer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult =
                    await roleManager.CreateAsync(
                        new IdentityRole(role));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(

                        $"Failed to create role {role}: " +
                        string.Join(", ",
                            roleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        // 2. Read Admin credentials from User Secrets.
        var adminEmail = configuration["AdminUser:Email"];
        var adminPassword = configuration["AdminUser:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException(
                "Admin email or password is missing from User Secrets.");
        }

        // 3. Find the Admin account.
        var adminUser =
            await userManager.FindByEmailAsync(adminEmail);

        // 4. Create the Admin account if it does not exist.
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Administrator",
                EmailConfirmed = true
            };

            var createResult =
                await userManager.CreateAsync(
                    adminUser, adminPassword);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Failed to create Admin account: " +
                    string.Join(", ",
                        createResult.Errors.Select(e => e.Description)));
            }
        }

        // 5. Ensure the account has the Admin role.
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            var roleResult =
                await userManager.AddToRoleAsync(adminUser, "Admin");

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Failed to assign Admin role: " +
                    string.Join(", ",
                        roleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}
