using System.Net;
using System.Text.Json;

namespace PracticaMaresa.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            _logger.LogError(ex, "Ha ocurrido una excepción no controlada.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = exception.Message,
            details = exception.InnerException?.Message
        };

        switch (exception)
        {
            case ArgumentException:
                // Errores de validación de datos
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            default:
                if (exception.Message.Contains("servicio externo", StringComparison.OrdinalIgnoreCase))
                {
                    // Errores de servicio externo simulado
                    context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                }
                else
                {
                    // Excepciones generales / base de datos
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new { error = "Ocurrió un error interno en el servidor.", details = (string?)null };
                }
                break;
        }

        var result = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(result);
    }
}
