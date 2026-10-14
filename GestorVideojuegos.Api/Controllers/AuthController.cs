using GestorVideojuegos.Application.DTOs;
using GestorVideojuegos.Application.Interfaces;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorVideojuegos.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // Genera la ruta: https://localhost:7036/api/auth
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous] // Permitimos registro sin estar autenticado
    [IgnoreAntiforgeryToken] // <-- ¡VITAL! Evita el Error 400 al registrarse
    public async Task<ActionResult> Register([FromBody] RegisterDto dto)
    {
        var success = await _authService.RegisterAsync(dto);
        if (!success)
        {
            return BadRequest(new { message = "El nombre de usuario ya está en uso." });
        }

        return Ok(new { message = "¡Usuario registrado exitosamente!" });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken] // <-- ¡VITAL! Evita el Error 400 al iniciar sesión
    public async Task<ActionResult> Login([FromBody] LoginDto dto, [FromServices] IAntiforgery antiforgery)
    {
        // 1. Verificamos las credenciales
        var token = await _authService.LoginAsync(dto);
        if (token == null)
        {
            return Unauthorized(new { message = "Usuario o contraseña incorrectos." });
        }

        // 2. CUMPLIMIENTO DE SEGURIDAD: Configuramos la Cookie HttpOnly para el JWT
        var jwtCookieOptions = new CookieOptions
        {
            HttpOnly = true, // El navegador esconde esta cookie de JavaScript (Evita XSS)
            Secure = true,   // Solo viaja por conexiones seguras HTTPS
            SameSite = SameSiteMode.None, // Permite cross-origin local (Vite y .NET en distintos puertos)
            Expires = DateTime.UtcNow.AddHours(2)
        };

        // Inyectamos el JWT en la cookie de respuesta
        Response.Cookies.Append("jwt_token", token, jwtCookieOptions);

        // 3. ROTACIÓN DE CSRF: Generamos un nuevo token ahora que el usuario se autenticó
        var csrfTokens = antiforgery.GetAndStoreTokens(HttpContext);
        Response.Cookies.Append("XSRF-TOKEN", csrfTokens.RequestToken!, new CookieOptions
        {
            HttpOnly = false, // Debe ser false para que Axios/JS pueda leerla y enviarla de vuelta
            Secure = true,
            SameSite = SameSiteMode.None, // Debe coincidir con la configuración de tu JWT
            Path = "/"
        });

        return Ok(new { message = "¡Login exitoso!" });
    }

    [HttpPost("logout")]
    [IgnoreAntiforgeryToken] // Opcional, pero recomendado para que siempre puedan cerrar sesión
    public IActionResult Logout()
    {
        // Eliminamos la cookie de identidad (JWT)
        Response.Cookies.Delete("jwt_token");

        // Eliminamos la cookie CSRF
        Response.Cookies.Delete("XSRF-TOKEN");

        return Ok(new { message = "Sesión cerrada de forma segura." });
    }

    [HttpGet("csrf-token")]
    [AllowAnonymous]
    public IActionResult GetCsrfToken([FromServices] IAntiforgery antiforgery)
    {
        // Esto genera el token y lo inyecta automáticamente como una cookie ("XSRF-TOKEN")
        // No necesitamos devolverlo en el JSON porque el interceptor de Axios ya sabe
        // buscarlo directamente en document.cookie
        antiforgery.GetAndStoreTokens(HttpContext);

        return NoContent(); // 204 No Content (petición exitosa, sin cuerpo en la respuesta)
    }
}