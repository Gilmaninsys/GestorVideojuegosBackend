using GestorVideojuegos.Application.DTOs;
using GestorVideojuegos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestorVideojuegos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TierListsController : ControllerBase
{
    private readonly ITierListService _service;

    public TierListsController(ITierListService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TierListDto>>> GetTierLists()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(await _service.GetUserTierListsAsync(userId!));
    }

    [HttpPost]
    public async Task<ActionResult> CreateTierList([FromBody] TierListDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Agrega dto.DatosJSON como parámetro
        var created = await _service.CreateTierListAsync(dto.Name, dto.DatosJSON, userId!);
        return Ok(created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTierList(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _service.DeleteTierListAsync(id, userId!);
        return NoContent();
    }
}