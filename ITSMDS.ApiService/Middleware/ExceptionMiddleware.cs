using ITSMDS.Application.CustomExceptions;
using System.Text.Json;

namespace ITSMDS.ApiService.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Success = false,
            Message = "An error occurred while processing your request"
        };

        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = validationException.StatusCode;
                response.Message = validationException.Message;
                response.ErrorCode = validationException.ErrorCode;
                response.AdditionalData = validationException.AdditionalData;
                break;

            case NotFoundException notFoundException:
                context.Response.StatusCode = notFoundException.StatusCode;
                response.Message = notFoundException.Message;
                response.ErrorCode = notFoundException.ErrorCode;
                break;

            case UnauthorizedException unauthorizedException:
                context.Response.StatusCode = unauthorizedException.StatusCode;
                response.Message = unauthorizedException.Message;
                response.ErrorCode = unauthorizedException.ErrorCode;
                break;

            case AppException appException:
                context.Response.StatusCode = appException.StatusCode;
                response.Message = appException.Message;
                response.ErrorCode = appException.ErrorCode;
                response.AdditionalData = appException.AdditionalData;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = "Internal server error";
                response.ErrorCode = "INTERNAL_ERROR";
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    response.AdditionalData = new { Detail = exception.Message, StackTrace = exception.StackTrace };
                }
                break;
        }

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

public class ErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string ErrorCode { get; set; }
    public object AdditionalData { get; set; }
}