using System.Net;
using System.Text.Json;

namespace GestorVideojuegos.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Deja que la petición continúe su flujo normal
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si algo explota en cualquier parte de la API, cae aquí
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Estructura JSON solicitada por los lineamientos
            var errorResponse = new
            {
                message = "Ha ocurrido un error interno en el servidor.",
                detail = ex.Message // Muestra el detalle del error (puedes quitarlo en producción)
            };

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }
}