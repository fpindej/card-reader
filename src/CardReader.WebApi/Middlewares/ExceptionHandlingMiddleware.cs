using System.Net;
using System.Text.Json;

namespace CardReader.WebApi.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate _next,
    ILogger<ExceptionHandlingMiddleware> _logger,
    IHostEnvironment _env)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (KeyNotFoundException keyNotFoundEx)
        {
            _logger.LogWarning(keyNotFoundEx, "A KeyNotFoundException occurred.");
            await HandleExceptionAsync(context, keyNotFoundEx, HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, e, HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        HttpStatusCode statusCode,
        string? customMessage = null)
    {
        var errorResponse = new ErrorResponse
        {
            Message = customMessage ?? exception.Message,
            Details = _env.IsDevelopment() ? exception.StackTrace : null
        };

        var payload = JsonSerializer.Serialize(errorResponse);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(payload);
    }

    private record ErrorResponse
    {
        public string? Message { get; set; }
        public string? Details { get; set; }
    }
}
