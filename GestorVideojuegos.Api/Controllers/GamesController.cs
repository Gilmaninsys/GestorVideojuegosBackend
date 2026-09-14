using GestorVideojuegos.Application.DTOs;
using GestorVideojuegos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorVideojuegos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
    {
        var games = await _gameService.GetAllGamesAsync();
        return Ok(games);
    }

    [HttpPost]
    public async Task<ActionResult> CreateGame([FromBody] GameDto gameDto)
    {
        var createdGame = await _gameService.CreateGameAsync(gameDto);
        return Ok(new { message = "¡Videojuego creado exitosamente!", game = createdGame });
    }
}