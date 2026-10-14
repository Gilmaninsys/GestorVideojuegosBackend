namespace GestorVideojuegos.Application.DTOs;

public class TierListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DatosJSON { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}