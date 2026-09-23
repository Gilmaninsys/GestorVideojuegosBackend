namespace GestorVideojuegos.Domain.Entities;

public class TierListItem
{
    public Guid Id { get; set; }

    public Guid TierListId { get; set; }
    public TierList? TierList { get; set; }

    public Guid GameId { get; set; }
    public Game? Game { get; set; }

    // El rango asignado al juego en esta lista específica
    public string Rank { get; set; } = "Unranked";
}