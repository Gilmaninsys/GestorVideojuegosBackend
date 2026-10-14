namespace GestorVideojuegos.Api.Middlewares;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "X-API-KEY";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Solo protegemos las rutas que empiecen con /api/public
        if (context.Request.Path.StartsWithSegments("/api/public"))
        {
            if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { message = "API Key no proporcionada en los headers." });
                return;
            }

            var apiKey = Environment.GetEnvironmentVariable("API_KEY");

            if (string.IsNullOrEmpty(apiKey) || apiKey != extractedApiKey.ToString())
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new { message = "API Key inválida o no autorizada." });
                return;
            }
        }

        await _next(context);
    }
}