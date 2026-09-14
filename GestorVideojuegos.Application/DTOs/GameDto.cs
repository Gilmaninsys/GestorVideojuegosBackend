namespace GestorVideojuegos.Application.DTOs;

public class GameDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string BoxArtUrl { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    
    // Nota: Omitimos el TwitchId y el CreatedAt porque el Frontend 
    // no los necesita para renderizar el catálogo con scroll infinito.
}