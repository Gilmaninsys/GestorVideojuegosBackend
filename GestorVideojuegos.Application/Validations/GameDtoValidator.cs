using FluentValidation;
using GestorVideojuegos.Application.DTOs;

namespace GestorVideojuegos.Application.Validations;

public class GameDtoValidator : AbstractValidator<GameDto>
{
    public GameDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título del videojuego es estrictamente obligatorio.")
            .MaximumLength(100).WithMessage("El título no puede exceder los 100 caracteres.");

        RuleFor(x => x.BoxArtUrl)
            .NotEmpty().WithMessage("La URL de la portada es obligatoria.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Debe enviar una URL válida y absoluta (ej. https://...).");
    }
}