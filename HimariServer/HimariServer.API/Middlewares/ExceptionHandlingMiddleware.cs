using HimariServer.Service.Exceptions;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using HimariServer.API.Models;

namespace HimariServer.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (DefaultException ex)
            {
                await HandleDefaultExceptionAsync(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (NotExistException ex)
            {
                await HandleNotExistExceptionAsync(context, ex, StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new BaseResponseModel
            {
                StatusCode = statusCode,
                Message = exception.Message,
            };

            var jsonResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });
            return context.Response.WriteAsync(jsonResponse);
        }

        private static Task HandleDefaultExceptionAsync(HttpContext context, DefaultException exception, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new BaseResponseModel
            {
                StatusCode = statusCode,
                Message = exception.Message,
            };

            var jsonResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });
            return context.Response.WriteAsync(jsonResponse);
        }

        private static Task HandleNotExistExceptionAsync(HttpContext context, NotExistException exception, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new BaseResponseModel
            {
                StatusCode = statusCode,
                Message = exception.Message,
            };

            var jsonResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
