namespace GestorVideojuegos.Domain.Entities;

public class TierList
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; // Ej: "Mis RPGs Favoritos"
    public string UserId { get; set; } = string.Empty; // El dueño del tablero
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relación: Una Tier List tiene muchos elementos dentro
    public ICollection<TierListItem> Items { get; set; } = new List<TierListItem>();
}