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

    public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
    {
        var games = await _repository.GetAllAsync();

        // El mapeo ahora vive en la capa correcta
        return games.Select(g => new GameDto
        {
            Id = g.Id,
            Title = g.Title,
            BoxArtUrl = g.BoxArtUrl,
            IsFavorite = g.IsFavorite
        }).ToList();
    }

    public async Task<GameDto> CreateGameAsync(GameDto gameDto)
    {
        var newGame = new Game
        {
            Title = gameDto.Title,
            BoxArtUrl = gameDto.BoxArtUrl,
            IsFavorite = gameDto.IsFavorite,
            TwitchId = "test-twitch-id"
        };

        await _repository.AddAsync(newGame);
        gameDto.Id = newGame.Id; // Asignamos el ID generado

        return gameDto;
    }
}