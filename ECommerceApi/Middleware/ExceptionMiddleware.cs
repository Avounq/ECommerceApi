using ECommerceApi.Exceptions;
using System.Text.Json;

namespace ECommerceApi.Middleware
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
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                {
                    _logger.LogError(
                        ex,
                        "Beklenmeyen hata oluştu. Method: {Method}, Path: {Path}",
                        context.Request.Method,
                        context.Request.Path);
                }
                else
                {
                    _logger.LogWarning(
                        ex,
                        "İstek hata ile sonuçlandı. StatusCode: {StatusCode}, Method: {Method}, Path: {Path}",
                        context.Response.StatusCode,
                        context.Request.Method,
                        context.Request.Path);
                }

                var response = new
                {
                    success = false,
                    statusCode = context.Response.StatusCode,
                    message = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
