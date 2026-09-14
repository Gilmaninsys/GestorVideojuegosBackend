using GestorVideojuegos.Domain.Entities;

namespace GestorVideojuegos.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}