using GestorVideojuegos.Application.DTOs;

namespace GestorVideojuegos.Application.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllGamesAsync(string userId);
    Task<GameDto> CreateGameAsync(GameDto gameDto, string userId);
    Task UpdateGameAsync(GameDto gameDto, string userId);
    Task DeleteGameAsync(Guid id, string userId);
}