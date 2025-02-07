using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Backend_reactNative_Shoppee_Data.Middleware
{
    public class SuccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SuccessMiddleware> _logger;

        public SuccessMiddleware(RequestDelegate next, ILogger<SuccessMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var originalResponseBodyStream = httpContext.Response.Body;
            using var newResponseBodyStream = new MemoryStream();
            httpContext.Response.Body = newResponseBodyStream;

            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode >= 200 && httpContext.Response.StatusCode < 300)
                {
                    newResponseBodyStream.Seek(0, SeekOrigin.Begin);
                    var originalBodyText = await new StreamReader(newResponseBodyStream).ReadToEndAsync();

                    var successResponse = new
                    {
                        statusCode = httpContext.Response.StatusCode,
                        message = "Request completed successfully.",
                        data = JsonSerializer.Deserialize<object>(originalBodyText)
                    };

                    var responseText = JsonSerializer.Serialize(successResponse);
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.ContentLength = responseText.Length;
                    httpContext.Response.Body = originalResponseBodyStream;

                    await httpContext.Response.WriteAsync(responseText);
                }
                else
                {
                    newResponseBodyStream.Seek(0, SeekOrigin.Begin);
                    await newResponseBodyStream.CopyToAsync(originalResponseBodyStream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the success middleware.");
                throw; // Rethrow the exception for other middleware to handle
            }
            finally
            {
                // Restore the original response body
                httpContext.Response.Body = originalResponseBodyStream;
            }
        }
    }
}
