using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using POS_ITS.API.Middlewares;
using System;
using System.Net;
using System.Threading.Tasks;

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Internal server error: ";

        switch (exception)
        {
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "You are not authorized to access this resource: ";
                break;

            case ArgumentNullException:
                statusCode = HttpStatusCode.BadRequest;
                message = "A required argument was null:";
                break;

            case ArgumentException:
                statusCode = HttpStatusCode.BadRequest;
                message = "An argument was invalid: ";
                break;

            case InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                message = "The operation is not valid due to the current state of the object: ";
                break;

            case InvalidDataException:
                statusCode = HttpStatusCode.BadRequest;
                message = "The data provided is not valid: ";
                break;

            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = "Not found: ";
                break;

            default:
                // Log specific exception type
                _logger.LogError(exception, "Unhandled exception: " + exception.Message);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = new
        {
            StatusCode = statusCode,
            Message = message + exception.Message
        };

        return context.Response.WriteAsJsonAsync(result);
    }
}
