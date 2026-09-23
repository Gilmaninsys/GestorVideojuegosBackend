using GestorVideojuegos.Application.DTOs;
using GestorVideojuegos.Application.Interfaces;
using GestorVideojuegos.Domain.Entities;

namespace GestorVideojuegos.Application.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _repository;

    public GameService(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GameDto>> GetAllGamesAsync(string userId)
    {
        var allGames = await _repository.GetAllAsync();

        // Filtramos para devolver solo los juegos de este usuario
        var userGames = allGames.Where(g => g.UserId == userId);

        return userGames.Select(g => new GameDto
        {
            Id = g.Id,
            Title = g.Title,
            BoxArtUrl = g.BoxArtUrl,
            IsFavorite = g.IsFavorite,
            Category = g.Category,
            Year = g.Year
        }).ToList();
    }

    public async Task<GameDto> CreateGameAsync(GameDto gameDto, string userId)
    {
        var newGame = new Game
        {
            Title = gameDto.Title,
            BoxArtUrl = gameDto.BoxArtUrl,
            IsFavorite = gameDto.IsFavorite,
            Category = gameDto.Category,
            Year = gameDto.Year,
            UserId = userId // ¡Aquí vinculamos el juego al dueño!
        };

        await _repository.AddAsync(newGame);
        gameDto.Id = newGame.Id;

        return gameDto;
    }

    public async Task UpdateGameAsync(GameDto gameDto, string userId)
    {
        var game = await _repository.GetByIdAsync(gameDto.Id);

        // Validamos que exista y que le pertenezca a quien intenta modificarlo
        if (game == null || game.UserId != userId)
        {
            throw new Exception("El videojuego no existe o no tienes permiso.");
        }

        game.Title = gameDto.Title;
        game.BoxArtUrl = gameDto.BoxArtUrl;
        game.IsFavorite = gameDto.IsFavorite;
        game.Category = gameDto.Category;
        game.Year = gameDto.Year;

        await _repository.UpdateAsync(game);
    }

    public async Task DeleteGameAsync(Guid id, string userId)
    {
        var game = await _repository.GetByIdAsync(id);

        if (game == null || game.UserId != userId)
        {
            throw new Exception("El videojuego no existe o no tienes permiso.");
        }

        await _repository.DeleteAsync(id);
    }
}