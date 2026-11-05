
using CampusPlacementManagement.Authorization;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Data.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles =
            {
                AppRoles.SuperAdmin,
                AppRoles.PlacementOfficer,
                AppRoles.CompanyHR,
                AppRoles.Student
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            var email = configuration["SuperAdmin:Email"];
            var password = configuration["SuperAdmin:Password"];
            var fullName = configuration["SuperAdmin:FullName"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName))
            {
                throw new InvalidOperationException(
                    "SuperAdmin configuration is missing.");
            }

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    EmailConfirmed = true,
                    AccountStatus = AccountStatus.Approved,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(
                    admin, password);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(
                    admin, AppRoles.SuperAdmin))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    admin, AppRoles.SuperAdmin);

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            roleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
