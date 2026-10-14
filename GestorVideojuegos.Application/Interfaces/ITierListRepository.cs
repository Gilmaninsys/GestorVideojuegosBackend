using GestorVideojuegos.Domain.Entities;

namespace GestorVideojuegos.Application.Interfaces;

public interface ITierListRepository
{
    Task<IEnumerable<TierList>> GetByUserIdAsync(string userId);
    Task<TierList?> GetByIdAsync(Guid id);
    Task AddAsync(TierList tierList);
    Task DeleteAsync(TierList tierList);
}