using System.Net;
using System.Text.Json;
using TaskListApi.DataContracts;
using TaskListApi.Exceptions;

namespace TaskListApi.Middleware;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            TaskListNotFoundException => (int)HttpStatusCode.NotFound,
            TaskListAccessDeniedException => (int)HttpStatusCode.Forbidden,
            DuplicateSharedUserException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var response = new ApiResponse<object>
        {
            StatusCode = statusCode,
            Data = null,
            Error = exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(result);
    }
}