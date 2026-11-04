using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MovieTicketBooking.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
namespace MovieTicketBooking.Authorization;

public class BookingAuthorizationHandler
    : AuthorizationHandler<OperationAuthorizationRequirement, Booking>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement,
        Booking booking)
    {
        if (requirement.Name != BookingOperations.Access)
        {
            return Task.CompletedTask;
        }

        // Admin can access every booking
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Get logged-in customer's email
        var userEmail = context.User.FindFirst(
            ClaimTypes.Email)?.Value;

        // Check whether the booking belongs to this customer
        if (!string.IsNullOrEmpty(userEmail) &&
            string.Equals(
                userEmail,
                booking.CustomerEmail,
                StringComparison.OrdinalIgnoreCase))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}