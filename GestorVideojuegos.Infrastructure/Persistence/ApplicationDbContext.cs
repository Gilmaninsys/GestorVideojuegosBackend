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

    // NUEVAS TABLAS PARA EL TIER LIST
    public DbSet<TierList> TierLists { get; set; }
    public DbSet<TierListItem> TierListItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de llaves primarias
        modelBuilder.Entity<Game>().HasKey(g => g.Id);
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<TierList>().HasKey(t => t.Id);
        modelBuilder.Entity<TierListItem>().HasKey(ti => ti.Id);

        // Configuración de Relaciones

        // 1. Relación: TierList -> TierListItem
        modelBuilder.Entity<TierListItem>()
            .HasOne(ti => ti.TierList)
            .WithMany(t => t.Items)
            .HasForeignKey(ti => ti.TierListId)
            .OnDelete(DeleteBehavior.Cascade); // Si borras la lista completa, se borran sus items

        // 2. Relación: TierListItem -> Game
        modelBuilder.Entity<TierListItem>()
            .HasOne(ti => ti.Game)
            .WithMany()
            .HasForeignKey(ti => ti.GameId)
            .OnDelete(DeleteBehavior.Restrict); // Evita que borrar un juego por error rompa la base de datos
    }
}