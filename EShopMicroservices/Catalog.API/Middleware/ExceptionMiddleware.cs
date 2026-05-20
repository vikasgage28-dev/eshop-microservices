using FluentValidation;
using System.Text.Json;

namespace Catalog.API.Middleware
{
    /// <summary>
    /// Global error handler — catches all unhandled exceptions and returns clean JSON responses.
    /// ValidationException → 400 Bad Request  (invalid input)
    /// Everything else    → 500 Internal Server Error
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                // FluentValidation failure → 400 Bad Request
                _logger.LogWarning("Validation failed: {Errors}",
                    string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));

                context.Response.StatusCode  = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                var response = new
                {
                    status  = 400,
                    message = "Validation failed.",
                    errors
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            }
            catch (Exception ex)
            {
                // Unexpected error → 500
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                context.Response.StatusCode  = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status  = 500,
                    message = "An unexpected error occurred."
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            }
        }
    }
}
