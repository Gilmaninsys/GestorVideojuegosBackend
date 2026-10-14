using GestorVideojuegos.Application.DTOs;

namespace GestorVideojuegos.Application.Interfaces;

public interface ITierListService
{
    Task<IEnumerable<TierListDto>> GetUserTierListsAsync(string userId);
    Task<TierListDto> CreateTierListAsync(string name, string datosJson, string userId);
    Task DeleteTierListAsync(Guid id, string userId);
}