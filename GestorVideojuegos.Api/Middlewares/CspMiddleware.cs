namespace GestorVideojuegos.Api.Middlewares;

public class CspMiddleware
{
    private readonly RequestDelegate _next;

    public CspMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Agregamos la cabecera CSP a la respuesta
        // - default-src 'self': Solo permite recursos del propio dominio.
        // - script-src 'self': Bloquea la inyección de scripts externos (XSS).
        // - img-src 'self' data: https:: Permite cargar portadas de juegos desde URLs externas (https).
        context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; connect-src 'self' ws: wss: http://localhost:* https://localhost:*;");
        await _next(context);
    }
}