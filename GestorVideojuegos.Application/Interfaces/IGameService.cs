using GestorVideojuegos.Application.DTOs;

namespace GestorVideojuegos.Application.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllGamesAsync();
    Task<GameDto> CreateGameAsync(GameDto gameDto);
}