using System.Net;
using System.Security.Authentication;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unexpected error occurred.");      

        ExceptionResponse response = exception switch
        {
            AuthenticationException => new ExceptionResponse(HttpStatusCode.BadRequest, "Wrong login or password"),
            ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
            ArgumentNullException => new ExceptionResponse(HttpStatusCode.BadRequest, "ArgumentNullException: " + exception.Message),
            ArgumentException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Wrong arguments"),
            KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found."),
            UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}