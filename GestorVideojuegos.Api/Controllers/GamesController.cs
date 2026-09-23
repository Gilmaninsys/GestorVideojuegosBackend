using GestorVideojuegos.Application.DTOs;
using GestorVideojuegos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Necesario para leer los claims del JWT

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
        // Extraemos el ID del usuario directamente del Token JWT
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var games = await _gameService.GetAllGamesAsync(userId!);
        return Ok(games);
    }

    [HttpPost]
    public async Task<ActionResult> CreateGame([FromBody] GameDto gameDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var createdGame = await _gameService.CreateGameAsync(gameDto, userId!);
        return Ok(new { message = "¡Videojuego creado exitosamente!", game = createdGame });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGame(Guid id, [FromBody] GameDto updatedGame)
    {
        if (id != updatedGame.Id) return BadRequest(new { message = "El ID no coincide." });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _gameService.UpdateGameAsync(updatedGame, userId!);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _gameService.DeleteGameAsync(id, userId!);
        return NoContent();
    }
}