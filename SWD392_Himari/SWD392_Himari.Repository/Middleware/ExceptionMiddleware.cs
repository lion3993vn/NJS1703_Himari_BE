using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backend_reactNative_Shoppee_Data.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public class BadRequestException : Exception
        {
            public BadRequestException(string message) : base(message) { }
        }

        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }

        public class UnauthorizedException : Exception
        {
            public UnauthorizedException(string message) : base(message) { }
        }

        public class ForbiddenException : Exception
        {
            public ForbiddenException(string message) : base(message) { }
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Tiếp tục chuỗi middleware
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                _logger.LogError(ex, $"An error occurred while processing the request: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex); // Xử lý lỗi trả về client
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode;
            string errorMessage = exception.Message;
            string errorType; // Định nghĩa chuỗi thông báo lỗi thân thiện

            switch (exception)
            {
                case BadRequestException:
                    statusCode = StatusCodes.Status400BadRequest;
                    errorType = "Bad Request";
                    break;
                case NotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    errorType = "Not Found";
                    break;
                case UnauthorizedException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    errorType = "Unauthorized";
                    break;
                case ForbiddenException:
                    statusCode = StatusCodes.Status403Forbidden;
                    errorType = "Forbidden";
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorType = "Internal Server Error";
                    errorMessage = "An unexpected error occurred."; 
                    break;
            }

            context.Response.StatusCode = statusCode;
            var response = new
            {
                statusCode = statusCode,
                message = errorMessage
            };

            _logger.LogError(exception, $"Error: {errorMessage}");

            var responseText = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(responseText);
        }

    }
}
