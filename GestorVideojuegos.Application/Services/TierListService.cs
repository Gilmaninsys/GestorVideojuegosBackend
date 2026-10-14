using GestorVideojuegos.Application.DTOs;
using GestorVideojuegos.Application.Interfaces;
using GestorVideojuegos.Domain.Entities;

namespace GestorVideojuegos.Application.Services;

public class TierListService : ITierListService
{
    private readonly ITierListRepository _repository;

    public TierListService(ITierListRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TierListDto>> GetUserTierListsAsync(string userId)
    {
        var lists = await _repository.GetByUserIdAsync(userId);

        // MAGIA 1: Asegurarnos de mapear el DatosJSON para enviarlo a Vue
        return lists.Select(t => new TierListDto
        {
            Id = t.Id,
            Name = t.Name,
            DatosJSON = t.DatosJSON, // <-- FUNDAMENTAL PARA EL FRONTEND
            CreatedAt = t.CreatedAt
        });
    }

    // MAGIA 2: Agregar "string datosJson" a los parámetros
    public async Task<TierListDto> CreateTierListAsync(string name, string datosJson, string userId)
    {
        var tierList = new TierList
        {
            Name = name,
            DatosJSON = datosJson, // <-- ASIGNARLO A LA ENTIDAD
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(tierList);

        return new TierListDto
        {
            Id = tierList.Id,
            Name = tierList.Name,
            DatosJSON = tierList.DatosJSON,
            CreatedAt = tierList.CreatedAt
        };
    }

    public async Task DeleteTierListAsync(Guid id, string userId)
    {
        var tierList = await _repository.GetByIdAsync(id);
        if (tierList == null || tierList.UserId != userId)
            throw new Exception("No autorizado o la lista no existe.");

        await _repository.DeleteAsync(tierList);
    }
}