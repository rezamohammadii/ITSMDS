using ITSMDS.Domain.Enums;
using ITSMDS.Domain.Tools;
using ITSMDS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;

namespace ITSMDS.ApiService.Middleware
{
    public class HandleIPMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandleIPMiddleware> _logger;
        private readonly List<string> _allowedRanges;
        public HandleIPMiddleware(RequestDelegate next, ILogger<HandleIPMiddleware> logger, 
            IEnumerable<string> cidrRanges)
        {
            _next = next;
            _logger = logger;
            _allowedRanges = cidrRanges.ToList();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var path = httpContext.Request.Path.Value ?? string.Empty;

                var regex = new Regex(@"^/api/auth/login$", RegexOptions.IgnoreCase);
                if (regex.IsMatch(path))
                {
                    IPAddress REQUEST_IP_ADDRESS = httpContext.Connection.RemoteIpAddress;
                    REQUEST_IP_ADDRESS = IpRangeHelper.ToIPv4(REQUEST_IP_ADDRESS);
                    if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    {
                        if (IPAddress.TryParse(httpContext.Request.Headers["X-Forwarded-For"], out var forwardedIp))
                        {
                            REQUEST_IP_ADDRESS = forwardedIp;
                            _logger.LogInformation($"Request from IP: {REQUEST_IP_ADDRESS}");

                        }
                    }

                    if (string.IsNullOrEmpty(REQUEST_IP_ADDRESS.ToString()))
                    {
                        await WriteResponse(httpContext, HttpStatusCode.NotAcceptable, ErrorCode.InvalidIpAddress);

                        return;
                    }
                    bool allowed = _allowedRanges.Any(c => IpRangeHelper.IsIpRange(REQUEST_IP_ADDRESS!, c));
                    if (!allowed)
                    {
                        await WriteResponse(httpContext, HttpStatusCode.Forbidden, ErrorCode.IpNotRange);

                        return;
                    }
                    else
                    {
                        using (var scope = httpContext.RequestServices.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                            var checkIpUser = await db.Users
                                .AnyAsync(x => x.IpAddress == REQUEST_IP_ADDRESS.ToString());

                            if (!checkIpUser)
                            {
                                await WriteResponse(httpContext, HttpStatusCode.Forbidden, ErrorCode.IpNotAllowed);

                                return;
                            }
                        }

                        await _next(httpContext);
                    }
                }
                else
                {
                    await _next(httpContext);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred");
                await WriteResponse(httpContext, HttpStatusCode.InternalServerError, ErrorCode.InvalidIpAddress);
            }
        }


        private async Task WriteResponse(HttpContext context, HttpStatusCode statusCode, ErrorCode errorCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = ApiResponse<object>.Fail(errorCode);
            var json = System.Text.Json.JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
        public async Task HandleRequestValidation(HttpContext context, HttpStatusCode statusCode, string title)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                title = title,
                statusCode = statusCode

            });
            await context.Response.WriteAsync(json);
            return;
        }

    }
}
