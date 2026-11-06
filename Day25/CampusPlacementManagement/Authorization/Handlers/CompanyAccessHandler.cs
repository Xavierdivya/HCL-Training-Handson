
using System.Security.Claims;
using CampusPlacementManagement.Authorization.Requirements;
using CampusPlacementManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace CampusPlacementManagement.Authorization.Handlers
{
    public class CompanyAccessHandler
        : AuthorizationHandler<CompanyAccessRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CompanyAccessRequirement requirement)
        {
            // SuperAdmin can access every company's resources.
            if (context.User.IsInRole(AppRoles.SuperAdmin))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Only CompanyHR users can access company-specific resources.
            if (!context.User.IsInRole(AppRoles.CompanyHR))
            {
                return Task.CompletedTask;
            }

            // Read the CompanyId claim from the JWT.
            var companyIdClaim = context.User.FindFirst("CompanyId");

            if (companyIdClaim == null ||
                !int.TryParse(companyIdClaim.Value, out var userCompanyId))
            {
                return Task.CompletedTask;
            }

            // Extract the CompanyId from the resource.
            int? resourceCompanyId = context.Resource switch
            {
                int id => id,
                Company company => company.CompanyId,
                CompanyHR companyHR => companyHR.CompanyId,
                _ => null
            };

            // Grant access only when both company IDs match.
            if (resourceCompanyId.HasValue &&
                resourceCompanyId.Value == userCompanyId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
