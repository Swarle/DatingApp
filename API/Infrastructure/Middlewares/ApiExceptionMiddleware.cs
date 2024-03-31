using System.Net;
using DatingApp.BL.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API.Infrastructure.Middlewares
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (HttpException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleInternalServerError(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpException ex)
        {
            if (ex.StatusCode == HttpStatusCode.InternalServerError)
            {
                await HandleInternalServerError(context, ex);
            }
            else
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int) ex.StatusCode;

                await context.Response.WriteAsync(ex.Message);
            }
        }

        private async Task HandleInternalServerError(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                ? new ApiExceptionResponse((HttpStatusCode)context.Response.StatusCode, ex.Message,
                    ex.StackTrace?.ToString())
                : new ApiExceptionResponse((HttpStatusCode)context.Response.StatusCode, "Internal Server Error");

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            };
            
            var json = JsonConvert.SerializeObject(response, settings);

            await context.Response.WriteAsync(json);
        }
        
        
    }


    public static class ApiExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiExceptionMiddleware>();
        }
    }
}
