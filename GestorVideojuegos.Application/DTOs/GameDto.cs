namespace GestorVideojuegos.Application.DTOs;

public class GameDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string BoxArtUrl { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public string Category { get; set; } = string.Empty;
    public int Year { get; set; }
}