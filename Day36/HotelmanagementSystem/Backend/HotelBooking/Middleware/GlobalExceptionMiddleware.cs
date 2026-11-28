
using System.Net;
using Microsoft.AspNetCore.Http;

namespace HotelBooking.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException)
            when (context.RequestAborted.IsCancellationRequested)
        {
            _logger.LogDebug(
                "Request was cancelled by the client.");
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "An unhandled exception occurred while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();

            var (statusCode, message) = exception switch
            {
                ArgumentException =>
                    (StatusCodes.Status400BadRequest,
                     exception.Message),

                UnauthorizedAccessException =>
                    (StatusCodes.Status403Forbidden,
                     "You are not allowed to perform this operation."),

                KeyNotFoundException =>
                    (StatusCodes.Status404NotFound,
                     "The requested resource was not found."),

                _ =>
                    (StatusCodes.Status500InternalServerError,
                     "An unexpected error occurred. Please try again later.")
            };

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(new
            {
                statusCode,
                message,
                traceId = context.TraceIdentifier
            });
        }
    }
}
