namespace GestorVideojuegos.Domain.Entities;

public class Game
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string BoxArtUrl { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public string Category { get; set; } = string.Empty; // Ej: RPG, Shooter, Plataformas
    public int Year { get; set; } // Ej: 2018
    public string UserId { get; set; } = string.Empty;
}