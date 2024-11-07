using System.Net;
using Newtonsoft.Json;

namespace Emne_7___Arbeidskrav_2.Middleware;

public class StudentBloggExceptionHandling
{
    private readonly RequestDelegate _next;
    private readonly ILogger<StudentBloggExceptionHandling> _logger;

    public StudentBloggExceptionHandling(RequestDelegate next, ILogger<StudentBloggExceptionHandling> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "An internal server error occurred. Please try again later.",
            Details = exception.Message // Consider hiding this in production environments
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}