namespace GestorVideojuegos.Domain.Entities;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Datos provenientes de la API de Twitch
    public string TwitchId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string BoxArtUrl { get; set; } = string.Empty;
    
    // Relación para la sección de favoritos de tu proyecto
    public bool IsFavorite { get; set; } = false;
    
    // Metadatos de auditoría básica
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}