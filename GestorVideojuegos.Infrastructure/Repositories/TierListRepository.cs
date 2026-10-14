using GestorVideojuegos.Application.Interfaces;
using GestorVideojuegos.Domain.Entities;
using GestorVideojuegos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestorVideojuegos.Infrastructure.Repositories;

public class TierListRepository : ITierListRepository
{
    private readonly ApplicationDbContext _context;

    public TierListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TierList>> GetByUserIdAsync(string userId)
        => await _context.TierLists.Where(t => t.UserId == userId).OrderByDescending(t => t.CreatedAt).ToListAsync();

    public async Task<TierList?> GetByIdAsync(Guid id)
        => await _context.TierLists.FindAsync(id);

    public async Task AddAsync(TierList tierList)
    {
        _context.TierLists.Add(tierList);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TierList tierList)
    {
        _context.TierLists.Remove(tierList);
        await _context.SaveChangesAsync();
    }
}