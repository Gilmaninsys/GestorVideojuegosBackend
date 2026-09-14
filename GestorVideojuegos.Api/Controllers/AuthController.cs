using GestorVideojuegos.Application.DTOs;
using GestorVideojuegos.Application.Interfaces;
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
    public async Task<ActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        if (token == null)
        {
            return Unauthorized(new { message = "Usuario o contraseña incorrectos." });
        }

        // CUMPLIMIENTO DE SEGURIDAD: Configuramos la Cookie HttpOnly
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // El navegador esconde esta cookie de JavaScript (Evita XSS)
            Secure = true,   // Solo viaja por conexiones seguras HTTPS
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddHours(2)
        };

        // Inyectamos el JWT en la cookie de respuesta
        Response.Cookies.Append("jwt_token", token, cookieOptions);

        return Ok(new { message = "¡Login exitoso!" });
    }

    [HttpPost("logout")]
    public ActionResult Logout()
    {
        // Para cerrar sesión, simplemente eliminamos la cookie
        Response.Cookies.Delete("jwt_token");
        return Ok(new { message = "Sesión cerrada correctamente." });
    }
}