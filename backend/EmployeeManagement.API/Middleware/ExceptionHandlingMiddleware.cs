using EmployeeManagement.Application.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Text.Json;

namespace EmployeeManagement.API.Middleware
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
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex,
                    "Resource not found while processing {Method} {Path}", 
                    context.Request.Method,
                    context.Request.Path);

                await WriteErrorResponse(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception while processing {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await WriteErrorResponse(context, HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = JsonSerializer.Serialize( new { status = (int)statusCode, message });
            //await context.Response.WriteAsJsonAsync(errorResponse);
            await context.Response.WriteAsync(errorResponse);
        }
    }
}
