using GestorVideojuegos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorVideojuegos.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Game>().HasKey(g => g.Id);
       
        modelBuilder.Entity<User>().HasKey(u => u.Id);
    }
}