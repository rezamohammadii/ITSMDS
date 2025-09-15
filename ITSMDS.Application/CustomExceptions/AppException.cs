namespace ITSMDS.Application.CustomExceptions;

public class AppException : Exception
{
    public int StatusCode { get; }
    public string ErrorCode { get; }
    public object AdditionalData { get; }

    public AppException(string message, int statusCode = 500, string errorCode = null, object additionalData = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        AdditionalData = additionalData;
    }
}

public class ValidationException : AppException
{
    public ValidationException(string message, object additionalData = null)
        : base(message, 400, "VALIDATION_ERROR", additionalData)
    {
    }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message, 404, "NOT_FOUND")
    {
    }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message)
        : base(message, 401, "UNAUTHORIZED")
    {
    }
}