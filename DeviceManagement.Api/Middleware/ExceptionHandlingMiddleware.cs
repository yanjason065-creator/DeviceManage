using DeviceManagement.Api.Services;
using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch(UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 
                    StatusCodes.Status401Unauthorized;

                var response = new
                {
                    success = false,
                    message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch(ValidationException ex)
            {
                _logger.LogWarning(ex, "Bad Request");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode =
                    StatusCodes.Status400BadRequest;

                var response = new
                {
                    success = false,
                    message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode =
                    StatusCodes.Status404NotFound;

                var response = new
                {
                    success = false,
                    message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch(NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode =
                    StatusCodes.Status404NotFound;

                var response = new
                {
                    success = false,
                    message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var response = new
                {
                    success = false,
                    message = "Internal Server Error",
                    error = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
