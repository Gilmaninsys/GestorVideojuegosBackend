using GestorVideojuegos.Application.Interfaces;
using GestorVideojuegos.Domain.Entities;
using GestorVideojuegos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestorVideojuegos.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}