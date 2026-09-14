using GestorVideojuegos.Application.DTOs;

namespace GestorVideojuegos.Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
}